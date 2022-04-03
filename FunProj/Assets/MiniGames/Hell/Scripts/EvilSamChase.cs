using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EvilSamChase : MonoBehaviour
{
    Rigidbody2D body;
    [SerializeField] Transform target;
    bool flipped,grounded;
    [SerializeField] float velocityCap;
    [SerializeField] float Speed,Jumpheight;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask GroundMask;
    [SerializeField] Transform TorsoPos;
    [SerializeField] Animator animator;
  

    void Start()
    {
    

        body = GetComponent<Rigidbody2D>();
        animator.SetBool("run", true);

    }

    // Update is called once per frame
    void Update()
    {
        if(target == null )
        {
            GameObject priority = null;
             List<GameObject> players = new List<GameObject>();
          GameObject[] possibleplayers  = GameObject.FindGameObjectsWithTag("Player");

            for (int a=0;a< possibleplayers.Length;a++)
            {
                if(possibleplayers[a].GetComponent<PlayerController>().animator.enabled )
                {
                    players.Add( possibleplayers[a]  ); 
                }
            }
            
            if(players.Count > 0)
            {
                priority = players[0];
                for (int i = 0;i<players.Count;i++)
            {
              

                if(Vector2.Distance(priority.transform.position,transform.position) > Vector2.Distance(transform.position, players[i].transform.position)  )
                    {
                        priority = players[i];
                    }
                {
                    

                }
            }

            }


            target = priority.GetComponent<PlayerController>().animator.transform;
          
        }

        if (target.position.x > transform.position.x)
        {
            if (flipped)
            {
                body.velocity = body.velocity / 4;

                transform.eulerAngles = new Vector3(0, 0, 0);
            }

            flipped = false;
         

        
        }
        else
        {
            if (!flipped)
            {
                body.velocity = body.velocity / 4;

                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            flipped = true;
        }


        if(Physics2D.OverlapCircle(groundCheck.position, .24f, GroundMask))
        {
            grounded = true;

        }
      
        if(grounded && Physics2D.Raycast(TorsoPos.position, transform.right, .3f, GroundMask))
        {
            body.AddForce(transform.up * Jumpheight, ForceMode2D.Impulse);
        }


     
       
        if (body.velocity.magnitude < velocityCap && Vector2.Distance(target.position,transform.position ) > .15f)
        {
            body.AddForce(transform.right * Speed * Time.deltaTime, ForceMode2D.Force);
        }



    }
}
