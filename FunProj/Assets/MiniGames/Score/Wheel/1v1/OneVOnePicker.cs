using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OneVOnePicker : MonoBehaviourPun
{
    

    [SerializeField] ScoreInfoDisplay[] displays;
    PhotonView view;
    int winnerindex;
    int randomtarget=0;
    [SerializeField] LoveShower loveshower;
    public void FindOneVOne()
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

        int possiblePlayers = 0;
        for (int i = 0; i < displays.Length; i++)
        {
            if (displays[i].Player != null)
            {
                possiblePlayers++;
            }
        }
        bool foundduel=false;
       

        if(possiblePlayers > 1)
        {
            while (!foundduel)
            {
                randomtarget = Random.Range(0, possiblePlayers);
                if (randomtarget != winnerindex)
                {
                    foundduel = true;
                }

            }

        }

       if(PhotonNetwork.IsMasterClient)
        {
          
            loveshower.Click(randomtarget, winnerindex);
        }
        


    }

}
