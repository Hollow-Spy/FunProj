using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class LoveShower : MonoBehaviourPunCallbacks
{
    PhotonView view;
   [SerializeField] ScoreInfoDisplay[] displays;
    [SerializeField] Transform[] Wpos;

    [SerializeField] GameObject OldCamObj, NewCamObj,TransitionOff,WheelPinkBackground;
    [SerializeField] Animator PosAnimator;

    [SerializeField] bool OneVOne;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }
    public void Click(int index, int winnerindex)
    {
        view.RPC("ClickALL", RpcTarget.All, index, winnerindex);
    }

    [PunRPC]
    public void ClickALL(int index, int windex)
    {
        TransitionOff.SetActive(true);
        StartCoroutine(TransitionNumerator( index,  windex));

    }

    IEnumerator TransitionNumerator(int index, int windex)
    {
        yield return new WaitForSeconds(.5f);
     
        displays[windex].Player.transform.position = Wpos[0].position;
       displays[index].Player.transform.position = Wpos[1].position;


       // displays[0].Player.transform.position = Wpos[0].position;
       // displays[1].Player.transform.position = Wpos[1].position;


        yield return new WaitForSeconds(.5f);
        if(WheelPinkBackground)
        {
            WheelPinkBackground.SetActive(false);
        }

        if(PosAnimator)
        {
            PosAnimator.enabled = true;
        }
       
        TransitionOff.SetActive(false);
        OldCamObj.SetActive(false);
        NewCamObj.SetActive(true);


        yield return new WaitForSeconds(4f);

        if (OneVOne && PhotonNetwork.IsMasterClient)
        {
            var hash = PhotonNetwork.MasterClient.CustomProperties;
            hash["OneVOne"] = true;
            hash["Player1"] = windex;
           hash["Player2"] = index;

            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);





            Player[] players = PhotonNetwork.PlayerList;
            Debug.Log(players[windex].NickName);
            Debug.Log(players[index].NickName);
         
          //  PlayerPrefs.SetInt("WinnerIndex", windex);
           // PlayerPrefs.SetInt("ChallengerIndex", index);

            PhotonNetwork.LoadLevel("TrainGame1");


        }

    }

}


