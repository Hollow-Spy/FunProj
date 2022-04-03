using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LoveButtons : MonoBehaviourPun
{
    [SerializeField] GameObject[] ButtonList;

    [SerializeField] ScoreInfoDisplay[] displays;

    [SerializeField] Transform[] Wpos;

    PhotonView view;

    int winnerindex;

    private void Start()
    {
        view = GetComponent<PhotonView>();
       for(int i=0;i<displays.Length;i++)
        {
            if(displays[i].Player != null && displays[i].Player.GetComponent<PhotonView>().IsMine)
            {
                winnerindex = i;
                i = 10;
                ButtonList[i].SetActive(true);
            }
        }
    }

    public void Click(int index)
    {
        view.RPC("ClickALL", RpcTarget.All, index, winnerindex);
    }

    [PunRPC]
    public void ClickALL(int index, int windex )
    {
        displays[windex].Player.transform.position = Wpos[0].position;
        displays[index].Player.transform.position = Wpos[1].position;
    }

}
