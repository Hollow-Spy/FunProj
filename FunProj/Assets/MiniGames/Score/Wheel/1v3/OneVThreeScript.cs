using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OneVThreeScript : MonoBehaviourPun
{
    [SerializeField] GameObject TransitionOFF;

    private void Start()
    {
        StartCoroutine(CutScene());
    }
    IEnumerator CutScene()
    {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        for(int i=0;i<Players.Length;i++)
        {
            int playerNum = (int)PhotonNetwork.PlayerList[i].CustomProperties["playerAvatar"];
            PlayerController controller = Players[i].GetComponent<PlayerController>();



            switch (playerNum)
            {
                case 0:


                    controller.animator.Play("Greet");
                    controller.face.Expression("happy");


                    break;
                case 1:
                    controller.animator.Play("HappyEmoteNatasha");
                    controller.face.Expression("happy");

                    break;
                case 2:
                    controller.animator.Play("SamHappyEmote");
                    controller.face.Expression("happy");
                    break;
                case 3:
                    controller.animator.Play("HappyDioAnim");
                    controller.face.Expression("happy");
                    break;

                case 4:
                    controller.animator.Play("HappyEli");
                    controller.face.Expression("happy");
                    break;

            }
        }
             

            yield return new WaitForSeconds(4);
        TransitionOFF.SetActive(true);
        yield return new WaitForSeconds(2.3f);
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("CharacterSelect");
        }


    }
}
