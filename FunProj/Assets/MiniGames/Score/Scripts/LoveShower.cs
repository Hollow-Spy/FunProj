using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LoveShower : MonoBehaviourPun
{
    PhotonView view;
   [SerializeField] ScoreInfoDisplay[] displays;
    [SerializeField] Transform[] Wpos;

    [SerializeField] GameObject OldCamObj, NewCamObj,TransitionOff,WheelPinkBackground;
    [SerializeField] Animator PosAnimator;
   
    

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
    }

}


