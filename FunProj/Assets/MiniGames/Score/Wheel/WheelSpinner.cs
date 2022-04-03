using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WheelSpinner : MonoBehaviourPun
{
   Rigidbody2D WheelBody;
    PhotonView view;
    bool Click;
    int currentSpin;
    int lastSpin;
    [SerializeField] Animator ArrowAnimator;
    [SerializeField] GameObject Spark,ReSpinText,DeathText,CurrentCam,OGCam,backgroundobj,transitionOff, boringText,lovetext,lovebuttons ;
    [SerializeField] SpriteRenderer wheelsprite;
    


    private void Start()
    {
        WheelBody = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
    }
    public void Spin()
    {
        float strengh = Random.Range(77777777, 99999999);
       
        view.RPC("SpinALL", RpcTarget.All,strengh);

    }

    public void Print()
    {
        float value = transform.localEulerAngles.z;

       
        switch (value)
        {
            case var expression when (value >= 0 && value < 27.4f):
                currentSpin = 0;
                break;
            case var expression when (value >= 27.4f && value < 47.77f):
                currentSpin = 1;

                break;
            case var expression when (value >= 47.77f && value < 89.6f):
                currentSpin = 2;

                break;
            case var expression when (value >= 89.6f && value < 134.5f):
                currentSpin = 3;

                break;
            case var expression when (value >= 134.5f && value < 181.4f):
                currentSpin = 4;

                break;
            case var expression when (value >= 181.4f && value < 220.2f):
                currentSpin = 5;

                break;
            case var expression when (value >= 220.2f && value < 269f):
                currentSpin = 6;

                break;
            case var expression when (value >= 269f && value < 320.4f):
                currentSpin = 7;

                break;
            case var expression when (value >= 320.4f && value < 360f):
                currentSpin = 8;

                break;
            default:
                //some code
                break;
        }

        if(lastSpin != currentSpin)
        {
            Click = true;
            lastSpin = currentSpin;
        }

    }

    public void customSpin(float strengh)
    {
        view.RPC("SpinALL", RpcTarget.All, strengh);

    }



    [PunRPC]
    public void SpinALL(float power)
    {
        StartCoroutine(SpeedUp(power / 30));

        StartCoroutine(Ticker());

    }

    IEnumerator Ticker()
    {
       
        yield return new WaitForSeconds(.2f);
        while (WheelBody.angularVelocity > 0.01f)
        {
           
            yield return null;
            Print();
            if(Click)
            {
                
                ArrowAnimator.SetFloat("speed", .5f + WheelBody.angularVelocity / 900);
                ArrowAnimator.SetTrigger("click");
                Click = false;
                Instantiate(Spark, ArrowAnimator.transform.position, Quaternion.identity);
            }
        }
        StartCoroutine( WheelOutcome() );
    }

    IEnumerator WheelOutcome()
    {
        ScoreInfoDisplay[] scoredisplays = GameObject.FindObjectsOfType<ScoreInfoDisplay>();

        yield return null;
        switch(currentSpin)
        {
            case 0:
                lovetext.SetActive(true);
                yield return new WaitForSeconds(4);
                backgroundobj.SetActive(false);
                wheelsprite.enabled = false;
                lovetext.SetActive(false);

                bool check=false;

                for (int i = 0; i < scoredisplays.Length; i++)
                {
                   check = scoredisplays[i].GetWinnerView();
                }

                if(check)
                {
                    lovebuttons.SetActive(true);
                }

                break;
            case 1:
                DeathText.SetActive(true);
                yield return new WaitForSeconds(3);
                DeathText.SetActive(false);
                wheelsprite.enabled = false;
                CurrentCam.SetActive(false);
                OGCam.SetActive(true);
                backgroundobj.SetActive(false);



                for (int i = 0; i < scoredisplays.Length; i++)
                {
                    scoredisplays[i].EmoteExternal(false,true);
                }

                yield return new WaitForSeconds(1.5f);

                for (int i = 0; i < scoredisplays.Length; i++)
                {
                    scoredisplays[i].killWinner();
                }
                yield return new WaitForSeconds(1.5f);

               

                transitionOff.SetActive(true);


                break;
            case 4:
                boringText.SetActive(true);
                yield return new WaitForSeconds(5f);
                transitionOff.SetActive(true);
                break;


            case 6:
                ReSpinText.SetActive(true);
                yield return new WaitForSeconds(1.3f);
                ReSpinText.SetActive(false);



              
                for(int i=0;i<scoredisplays.Length;i++)
                {
                    scoredisplays[i].ActivateButtons();
                }

                break;


        }
    }


  

    IEnumerator SpeedUp(float power)
    {
        
        for(int i=0;i<30;i++)
        {
            WheelBody.AddTorque(power);
            yield return new WaitForSeconds(0.1f);
           
        }
        
      
    }
}
