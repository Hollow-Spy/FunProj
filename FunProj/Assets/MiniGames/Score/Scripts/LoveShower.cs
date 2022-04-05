using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LoveShower : MonoBehaviourPun
{
    PhotonView view;
   [SerializeField] ScoreInfoDisplay[] displays;
    [SerializeField] Transform[] Wpos;

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
        displays[windex].Player.transform.position = Wpos[0].position;
        displays[index].Player.transform.position = Wpos[1].position;
    }

}


