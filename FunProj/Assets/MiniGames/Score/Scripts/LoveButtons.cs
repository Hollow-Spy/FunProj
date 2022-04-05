using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LoveButtons : MonoBehaviourPun
{
    [SerializeField] GameObject[] ButtonList;

    [SerializeField] ScoreInfoDisplay[] displays;

 

    PhotonView view;

    [SerializeField] Canvas uiCanvas;
    [SerializeField] Camera cam1;
    int winnerindex;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        for (int i = 0; i < displays.Length; i++)
        {
            if (displays[i].Player != null && displays[i].Player.GetComponent<PhotonView>().IsMine)
            {
                winnerindex = i;
                i = 10;
               // ButtonList[i].SetActive(true);
            }
        }

        for (int a = 0; a < displays.Length; a++)
        {
            if (displays[a].Player != null && !displays[a].Player.GetComponent<PhotonView>().IsMine )
            {

                ButtonList[a].SetActive(true);
            }
            else
            {
                ButtonList[a].SetActive(false);

            }

        }

        uiCanvas.worldCamera = cam1;
    }

    public void Click(int index)
    {
        FindObjectOfType<LoveShower>().Click(index, winnerindex);
    }

   

}
