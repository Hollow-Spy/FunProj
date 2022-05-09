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
    [SerializeField] GameObject Spark,ReSpinText,DeathText,CurrentCam,OGCam,backgroundobj,transitionOff, boringText,lovetext,lovebuttons,GoldenText,ActualWinnerText,WinnerCanvas,InstaWin,ThroneCanvas,OneVThreeCam ;
    [SerializeField] SpriteRenderer wheelsprite,arrowsprite;
    [SerializeField] OneVOnePicker onevoner;
    [SerializeField] Transform[] OneVThreePos;
    [SerializeField] ScoreInfoDisplay scoredisplay0;
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

    public void customSpin()
    {
        // float strengh = 199777;// death
        // float strengh = 299777;  //1v1
        //float strengh = 459977; //golden point
        //float strengh = 899977; //loser wins
        // float strengh = 1409977;//[pure winner
        float strengh = 1639977; // 1v3
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

                    if (check)
                    {
                        lovebuttons.SetActive(true);
                    }
                }

              

                break;
            case 1:
                DeathText.SetActive(true);
                yield return new WaitForSeconds(3);
                DeathText.SetActive(false);
                wheelsprite.enabled = false;
                arrowsprite.enabled = false;
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

            case 2:
                onevoner.FindOneVOne();
                yield return new WaitForSeconds(6.5f);
                transitionOff.SetActive(true);
                break;
            case 3:
                StartCoroutine(SpeedUp(9997777 / 30));
                GoldenText.SetActive(true);
                yield return new WaitForSeconds(3);
                wheelsprite.enabled = false;
                yield return new WaitForSeconds(.5f);
                arrowsprite.enabled = false;
                yield return new WaitForSeconds(3);
                transitionOff.SetActive(true);
                break;
            case 4:
                boringText.SetActive(true);
                yield return new WaitForSeconds(5f);
                transitionOff.SetActive(true);
                break;
            case 5:
                wheelsprite.enabled = false;
                arrowsprite.enabled = false;
                backgroundobj.SetActive(false);

                FindObjectOfType<ScoreInfoDisplay>().ZoomLowestPlayer();
                ActualWinnerText.SetActive(true);
                yield return new WaitForSeconds(4);
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
            case 7:
                OGCam.SetActive(true);
                CurrentCam.SetActive(false);
                backgroundobj.SetActive(false);
                wheelsprite.enabled = false;
                arrowsprite.enabled = false;


                WinnerCanvas.SetActive(true);
                yield return new WaitForSeconds(2);

                InstaWin.SetActive(true);
                ThroneCanvas.SetActive(true);
                FindObjectOfType<ScoreInfoDisplay>().PlayThroneWinner();
                break;
            case 8:

                int playerAmout = PhotonNetwork.CurrentRoom.PlayerCount;
                int winnerindex = FindObjectOfType<ScoreInfoDisplay>().GetWinnerIndex();


                GameObject playerw = scoredisplay0.ReturnPlayerObject(winnerindex);
                playerw.GetComponent<PlayerController>().animator.SetBool("idle", false);
                playerw.GetComponent<PlayerController>().animator.SetBool("run", true);
                playerw.GetComponent<PlayerController>().animator.Play("Run");

                playerw.transform.position = OneVThreePos[0].position;
                int transformindex=1;

                GameObject[] NonWinnerPlayers = new GameObject[playerAmout-1];
                int NonWinnerIndex=0;
                 for(int i =0;i<playerAmout;i++)
                  {
                    Debug.Log(i);
;                      if(winnerindex != i && scoredisplay0.DoesPlayerExist(i))
                    {
                        Debug.Log("hi");

                        GameObject cplayer = scoredisplay0.ReturnPlayerObject(i);
                          cplayer.transform.position = OneVThreePos[transformindex].position;
                        cplayer.transform.eulerAngles = new Vector3(0, 180, 0);
                        NonWinnerPlayers[NonWinnerIndex] = cplayer;
                        NonWinnerIndex++;

                          transformindex++;
                      }
                  }
      
                OneVThreeCam.SetActive(true);
                OGCam.SetActive(false);
                CurrentCam.SetActive(false);
                wheelsprite.enabled = false;
                arrowsprite.enabled = false;
                backgroundobj.SetActive(false);
                yield return new WaitForSeconds(1.6f);
                playerw.GetComponent<PlayerController>().animator.SetBool("idle", true);
                playerw.GetComponent<PlayerController>().animator.SetBool("run", false) ;
             
                playerw.GetComponentInChildren<FaceExpressions>().Expression("suprised");

              
                for (int c =0;c < NonWinnerPlayers.Length;c++)
                {
                    yield return new WaitForSeconds(.5f);
                    switch(c)
                    {
                        case 0:
                            NonWinnerPlayers[c].GetComponent<PlayerController>().animator.Play("Angry1v3One");
                            break;
                        case 1:
                            NonWinnerPlayers[c].GetComponent<PlayerController>().animator.Play("Angry1v3Two");
                            break;
                        case 2:
                            NonWinnerPlayers[c].GetComponent<PlayerController>().animator.Play("Angry1v3Three");
                            break;
                    }
                  
                }

                yield return new WaitForSeconds(1.5f);

                OneVThreeCam.GetComponent<Animator>().Play("SpotLightReverseAnim");
                yield return new WaitForSeconds(2f);
                transitionOff.SetActive(true);
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
