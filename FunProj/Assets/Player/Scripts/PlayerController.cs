using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerController : MonoBehaviourPun, IPunObservable
{
    public int flipdir;
    public float accelaration;

    public float speed;
    public float maxspeed;
    [SerializeField] float FallSpeedCap;

    public bool isJumping;
    public float jumpheight;
    public float JumpHoldIncrement;
    public float JumpTimer;
     float jumpTimeCount;

    public float coyoteTime;
   public float coyoteTimer;

    public float JumpbufferTime;
     public float JumpbufferTimer;

    public float AnimfeetRadius;
    public float feetRadius;
    public float OGgravity;
    [SerializeField] Transform FeetPos;
    [SerializeField] LayerMask whatIsGround;

    Rigidbody2D body;
    public bool is_crouching;
    public bool is_Airborne;
    public bool is_grounded;
   public bool is_running;
  public  bool is_idle;
    public bool is_knocked;
    public bool is_noding;
    public bool is_emoting;
    public bool is_shotgun;
    public bool is_dead;

    public Animator animator;
    public GameObject[] DustParticles;

    [SerializeField] BoxCollider2D BodyCollider;
    [SerializeField] Transform GroundBlock;

    [SerializeField] bool flipped;
    [SerializeField] LayerMask PlayerLayer;
    [SerializeField] float HeadKnockoutRadius;

    [SerializeField] PhysicsMaterial2D NoFrictionMat,BounceMat;

    [SerializeField] GameObject JumpSound,hurtsound,landsound;
    
    public float KnocktimeIncrement;
    public float KnockStrenghIncrement;
    public float Knocktime;
    public float KnockStrengh;
    bool KnockImmune;
     public PhotonView view;

    float grav=1;

    CameraFollow camfollow;

     public FaceExpressions face;
    [SerializeField] GameObject[] NormalIKArms;
    [SerializeField] GameObject Shotgun;
    [SerializeField] GameObject SolidHairObj,DynamicHairObj;
    [SerializeField] GameObject skeleton;

    bool canSuicide=false;
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(flipdir);
            stream.SendNext(animator.gameObject.transform.rotation);
            stream.SendNext(is_crouching);
            stream.SendNext(is_Airborne);
            stream.SendNext(is_grounded);
            stream.SendNext(is_knocked);
            stream.SendNext(is_idle);
            stream.SendNext(is_noding);
            stream.SendNext(is_running);
            stream.SendNext(is_dead);
            stream.SendNext(Shotgun.gameObject.transform.rotation);

            stream.SendNext(grav);
           


        }
        else
        {
            if (stream.IsReading)
            {
                flipdir = (int)stream.ReceiveNext();
                animator.gameObject.transform.rotation = (Quaternion)stream.ReceiveNext();
                is_crouching = (bool)stream.ReceiveNext();
                is_Airborne = (bool)stream.ReceiveNext();
                is_grounded = (bool)stream.ReceiveNext();
                is_knocked = (bool)stream.ReceiveNext();
                is_idle = (bool)stream.ReceiveNext();
                is_noding = (bool)stream.ReceiveNext();
                is_running = (bool)stream.ReceiveNext();
                is_dead = (bool)stream.ReceiveNext();

                Shotgun.gameObject.transform.rotation = (Quaternion)stream.ReceiveNext();

                grav = (float)stream.ReceiveNext();
             
            }

        }
    }
    public void SolidHair()
    {
        SolidHairObj.SetActive(true);
        DynamicHairObj.SetActive(false);
    }


    public void DynamicHair()
    {
        SolidHairObj.SetActive(false);
        DynamicHairObj.SetActive(true);

    }

    public void DeathALL()
    {

        view.RPC("Death", RpcTarget.All);
    }
    
    [PunRPC]
    void Death()
    {
        if(!is_dead)
        {
            is_dead = true;
            gameObject.layer = 8;

            GetComponent<InputCollector>().locked = true;
            skeleton.SetActive(true);
            animator.gameObject.SetActive(false);

            Rigidbody2D[] bodies = skeleton.GetComponentsInChildren<Rigidbody2D>();
            for (int i = 0; i < bodies.Length; i++)
            {

                bodies[i].AddForce(new Vector2(Random.Range(-.6f, .6f), Random.Range(2.5f, 3.3f)), ForceMode2D.Impulse);
                bodies[i].AddTorque(Random.Range(-1.5f, 1.5f));


            }

            if(GameObject.FindGameObjectWithTag("Score") && view.IsMine)
            {
                GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreCounter>().StopCount();
                GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreCounter>().PlayerDiedAll();
            }

        }


    }


    [PunRPC]
    public void Knockout(Vector2 Dir)
    {
      


      if(is_knocked || KnockImmune)
        {
            
            return;
        }
      if(is_shotgun)
        {
            UnEquipShotgun();
        }


     
        is_knocked = true;
        is_idle = false;
        is_Airborne = false;

        animator.SetBool("idle", false);
        animator.SetBool("airborne", false);

        Instantiate(hurtsound, transform.position, Quaternion.identity);

        animator.SetFloat("spinspeed", KnockStrengh * .7f);

        Dir *= KnockStrengh;

        animator.SetBool("knockout", true);

        body.drag = 0;
        body.gravityScale = OGgravity;
        grav = OGgravity;

        body.velocity = Vector2.zero;
        body.AddForce(Dir,ForceMode2D.Impulse);

       

        BodyCollider.sharedMaterial = BounceMat;
        body.sharedMaterial = BounceMat;
       

        KnockStrengh += KnocktimeIncrement;
        Knocktime += KnocktimeIncrement;

        face.Expression("suprised");

        // IEnumerator knockcoroutine= KnockNumerator();

        //  view.RPC("KnockNumerator", RpcTarget.All);

        StartCoroutine("KnockNumerator");

    }

 
    IEnumerator KnockNumerator()
    {
        KnockImmune = true;
        yield return new WaitForSeconds(0.0001f);
        animator.SetBool("spin", true);

        yield return new WaitForSeconds(Knocktime);
        animator.SetBool("spin", false);


        BodyCollider.sharedMaterial = NoFrictionMat;
        body.sharedMaterial = NoFrictionMat;

        face.Expression("normal");


      
        is_knocked = false;
       animator.SetBool("knockout", false);

        if(is_shotgun)
        {
            EquipShotgun();
        }

        yield return new WaitForSeconds(1);
        KnockImmune = false;



    }

    void Start()
    {
        PhotonNetwork.SendRate = 120;
        PhotonNetwork.SerializationRate = 120;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        view = GetComponent<PhotonView>();

        if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>() )
        {
            camfollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
            if (camfollow.target == null && view.IsMine)
            {
                camfollow.GetComponent<CameraFollow>().target = gameObject.transform;
            }

        }



       

        body = GetComponent<Rigidbody2D>();
        OGgravity = body.gravityScale;

        StartCoroutine(SuicideDelayNumerator());
    }

    IEnumerator SuicideDelayNumerator()
    {
        yield return new WaitForSeconds(4);
        canSuicide = true;
    }

    void OnDrawGizmos()
    {/*
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GroundBlock.position,HeadKnockoutRadius);*/
    }

    public void EquipShotgun()
    {
      
            NormalIKArms[0].SetActive(false);
            NormalIKArms[1].SetActive(false);
            Shotgun.SetActive(true);
        
    }

    public void UnEquipShotgun()
    {
        NormalIKArms[0].SetActive(true);
        NormalIKArms[1].SetActive(true);
        Shotgun.SetActive(false);
    }

    public void Emoting(bool stat)
    {
        is_emoting = stat;
     
        animator.SetBool("idle", false);
        if(!stat)
        {
            face.Expression("normal");
            animator.Play("Idle");
        }
    }

    public void KnockoutSend(Vector2 dir)
    {
        view.RPC("Knockout", RpcTarget.All, dir);
    }

   

    public void InstantiateAll(string name)
    {
        PhotonNetwork.Instantiate(name, transform.position, Quaternion.identity);
    }

    void Update()
    {
        

     if(is_Airborne && body.velocity.y < FallSpeedCap)
        {
     
            body.velocity = new Vector2(body.velocity.x, FallSpeedCap );
        }
   
      

        if(!is_knocked )
        {
            animator.SetBool("knockout", false);
        }
        

        if(view.IsMine && !is_knocked)
        {


            /*
           if(Input.GetKeyDown(KeyCode.P))
            {
                body.velocity = new Vector2(body.velocity.x, 0);
                body.gravityScale = 0;
                grav = 0;
            }
         
            if (Input.GetKeyDown(KeyCode.O))
            {
                grav = OGgravity;
                body.gravityScale = OGgravity;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                transform.position = GameObject.Find("WormSpot").transform.position;
            }
                 */
            if (canSuicide && Input.GetKeyDown(KeyCode.I) )
            {
                DeathALL();
            }
       
            is_grounded = Physics2D.OverlapCircle(FeetPos.position, feetRadius, whatIsGround);

          

           //anim handle
            
        if (Physics2D.OverlapCircle(FeetPos.position, AnimfeetRadius, whatIsGround))
        {
            animator.SetBool("airborne", false);
        }
        else
        {
            animator.SetBool("airborne", true);
        }
        // actual groundcheck
        if (!is_grounded)
        {
            coyoteTimer -= Time.deltaTime;
            is_Airborne = true;
           
        }
        else
        {
            if(is_Airborne && JumpbufferTimer < -.7f) 
            {
               PhotonNetwork.Instantiate(DustParticles[Random.Range(0, DustParticles.Length)].name , new Vector2(FeetPos.position.x, FeetPos.position.y + .05f), Quaternion.identity);
                 Instantiate(landsound, transform.position, Quaternion.identity);
            }


          
           coyoteTimer = coyoteTime;
            animator.ResetTrigger("jump");
            is_Airborne = false;
       
            
        }


           if(isJumping || is_Airborne)
         {

                GroundBlock.GetChild(0).transform.gameObject.SetActive(false);


                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(GroundBlock.position, HeadKnockoutRadius,PlayerLayer);
                foreach (var hitCollider in hitColliders)
                {
                    if(hitCollider.gameObject.transform.position  != gameObject.transform.position)
                    {
                      
                        PlayerController targetcontroller = hitCollider.gameObject.GetComponent<PlayerController>();
                        if(Vector2.Distance(targetcontroller.FeetPos.position,GroundBlock.position ) <= HeadKnockoutRadius && (targetcontroller.isJumping || targetcontroller.is_Airborne))
                        {
                            Vector2 dir = new Vector2(body.velocity.x, maxspeed );
                           
                            targetcontroller.view.RPC("Knockout", RpcTarget.All,dir);
                        }
                    }

                    
                }


            }
            else
            {
                GroundBlock.GetChild(0).transform.gameObject.SetActive(true);
            }


        }

        if (is_crouching)
        {
            BodyCollider.size = new Vector2(0.10304445f, 0.135483772f);
            BodyCollider.offset = new Vector2(0.0101106763f, 0.228622243f);

            GroundBlock.localPosition = new Vector2(GroundBlock.localPosition.x, 0.323f);
        }
        else
        {
            GroundBlock.localPosition = new Vector2(GroundBlock.localPosition.x, 0.416f);

            BodyCollider.size = new Vector2(0.10304445f, 0.278239369f);
            BodyCollider.offset = new Vector2(0.0101106763f, 0.300000012f);
        }

    }


    private void FixedUpdate()
    {
        JumpHeight();
    }
    public void JumpHeight()
    {
        if (jumpTimeCount >= 0 && isJumping)
        {
            jumpTimeCount -= Time.fixedDeltaTime;
            body.AddForce(Vector2.up * JumpHoldIncrement);
        }
        else
        {
            if(isJumping)
            {
                body.gravityScale = body.gravityScale / 2;

                IEnumerator ResetGravity = ResetJumpGravity();
                StopCoroutine(ResetGravity);
                StartCoroutine(ResetGravity);
            }

            isJumping = false;

            

        }

    }

    IEnumerator ResetJumpGravity()
    {
        yield return new WaitForSeconds(.13f);
        body.gravityScale = OGgravity;
    }

    public void Jump()
    {
        if (coyoteTimer > 0 && JumpbufferTimer > 0)
        {
           PhotonNetwork.Instantiate(DustParticles[Random.Range(0, DustParticles.Length)].name, new Vector2(FeetPos.position.x,FeetPos.position.y+.05f), Quaternion.identity);

            Instantiate(JumpSound, transform.position, Quaternion.identity);

            JumpbufferTimer = 0;
            isJumping = true;
            jumpTimeCount = JumpTimer;

            animator.SetBool("nod", false);
            animator.SetBool("crouch", false);

            animator.SetTrigger("jump");
           

            body.velocity = new Vector2(body.velocity.x, 0);

            body.AddForce(Vector2.up * jumpheight);

         
        }
    }

    public void Movement(float Horizontal,float Vertical)
    {
        speed = Vector2.Dot(transform.right, body.velocity);
        if (Horizontal >0)
        {
            flipdir = 1;
           
            if (flipped)
            {
                 animator.gameObject.transform.localPosition = new Vector2(0, animator.gameObject.transform.localPosition.y);

                animator.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

                body.velocity = new Vector2(-0.5f, body.velocity.y);
                    
                flipped = false;
            }

        }
       if(Horizontal < 0)
        {
            flipdir = -1;
            if (!flipped)
            {

                  animator.gameObject.transform.localPosition = new Vector2(0.02f, animator.gameObject.transform.localPosition.y);
                animator.gameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
                body.velocity = new Vector2(.5f, body.velocity.y);




                flipped = true;
            }
        }
     

            
        
        if(!is_Airborne && !isJumping && !is_knocked)
        {
            if (Horizontal == 0)
            {
                body.drag = Mathf.Lerp(body.drag, 20, 50 * Time.deltaTime);
                animator.SetBool("run", false);
                if(!is_crouching && !is_noding && !is_emoting)
                {
                    animator.SetBool("idle", true);
                    is_idle = true;
                }
             
                is_running = false;

                if(Vertical > 0)
                {
                   if(!is_noding)
                    {
                        if (camfollow.target == gameObject.transform)
                        {
                            camfollow.offset = new Vector3(camfollow.offset.x, 1f, camfollow.offset.z);
                        }
                    }
                    is_noding = true;
                    is_idle = false;
                    
                    animator.SetBool("nod", true);
                   
                }
                else
                {
                    if (is_noding)
                    {
                        if (camfollow.target == gameObject.transform)
                        {
                            camfollow.offset = new Vector3(camfollow.offset.x, 0.1f, camfollow.offset.z);
                        }
                    }

                    is_noding = false;
                    animator.SetBool("nod", false);
                }

                if(Vertical < 0)
                {
                    is_crouching = true;
                    is_idle = false;

                    animator.SetBool("crouch", true);
                }
                else
                {
                   animator.SetBool("crouch", false);
                    is_crouching = false;
                }
               

            }
            else
            {
              
                animator.SetBool("crouch", false);
                animator.SetBool("nod", false);
                animator.SetBool("run", true);
                animator.SetBool("idle", false);
                is_idle = false;
                is_running = true;
                is_crouching = false;
                is_noding = false;

                if (camfollow.target == gameObject.transform)
                {
                    camfollow.offset = new Vector3(camfollow.offset.x, 0.1f, camfollow.offset.z);
                }

                body.drag = .15f;

                

            }
        }
        else
        {
            animator.SetBool("run", false);
            animator.SetBool("idle", false);
            animator.SetBool("nod", false);
            animator.SetBool("crouch", false);

            is_crouching = false;
            is_noding = false;
            is_idle = false;
            is_running = false;



            if (Vertical < 0)
            {
                body.gravityScale = Mathf.Lerp(body.gravityScale, 2, 2 * Time.deltaTime);
            }

            body.drag = 0.3f;
            
        }
      

       
       


        if (speed > maxspeed && Horizontal > 0 && flipdir == 1)
        {
            return;
        }

       
        if (speed < -maxspeed && Horizontal < 0 && flipdir == -1)
        {
            return;
        }


      
      

        



        Vector2 force = transform.right * Horizontal * accelaration ;
      

        body.AddForce(force, ForceMode2D.Force);
    }

  

    // Update is called once per frame

}
