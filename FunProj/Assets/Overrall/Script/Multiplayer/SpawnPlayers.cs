using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnPlayers : MonoBehaviourPunCallbacks/*, IPunObservable*/
{
    public int PlayersJoined;
    public GameObject[] playerPrefabs;
    [SerializeField] GameObject GameStarter,CountDown;
    [SerializeField] Transform[] spawnPositions;
    PhotonView view;
    [SerializeField] bool active;

    [SerializeField] int[] PlayerIds;


    /* void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
     {
         if (stream.IsWriting)
         {
             stream.SendNext(PlayersJoined);


         }
         else
         {
             if (stream.IsReading)
             {
                 PlayersJoined = (int)stream.ReceiveNext();

             }

         }
     }
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && active)
        {

            for (int i = 0; i < PlayerIds.Length; i++)
            {
               
                if (PlayerIds[i] == collision.gameObject.GetComponent<PhotonView>().InstantiationId)
                {
                    i = PlayerIds.Length;
                   
                }
                else
                {
                   
                    if (PlayerIds[i] == -1)
                    {
                    
                        PlayerIds[i] = collision.gameObject.GetComponent<PhotonView>().InstantiationId;
                        PlayersJoined++;
                        i = PlayerIds.Length;
                    }

                }

            }
          
            if (PlayersJoined == PlayerPrefs.GetInt("PlayerNumber") )
            {

                active = false;
                view.RPC("StartGame", RpcTarget.All);
               
            }

        }
    }

   


    private void Start()
    {

       


        view = GetComponent<PhotonView>();
        PlayerIds = new int[PlayerPrefs.GetInt("PlayerNumber")];
        for(int i =0;i<PlayerIds.Length;i++)
        {
            PlayerIds[i] = -1;
        }
       
  
  
        /*Vector3 spawnPos = new Vector3(spawnPositions[PlayersJoined].position.x - 0.7570001f, spawnPositions[PlayersJoined].position.y, spawnPositions[PlayersJoined].position.z);

        if(PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == null) 
        {
            PhotonNetwork.Instantiate(playerPrefabs[0].name, spawnPos, Quaternion.identity);

        }
        else
        {
            GameObject playertoSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
            PhotonNetwork.Instantiate(playertoSpawn.name, spawnPos, Quaternion.identity);

        }*/
        active = true;

        StartCoroutine(SpawnPlayer());


    }



    IEnumerator SpawnPlayer()
    {
        yield return new WaitForSeconds(.1f);
        int pos = 0;
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == PhotonNetwork.LocalPlayer)
            {
                pos = i;
            }
        }




        Vector3 spawnPos = new Vector3(spawnPositions[pos].position.x - 0.7570001f, spawnPositions[pos].position.y, spawnPositions[pos].position.z);

        if (PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == null)
        {
            PhotonNetwork.Instantiate(playerPrefabs[0].name, spawnPos, Quaternion.identity);

        }
        else
        {
            GameObject playertoSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
            PhotonNetwork.Instantiate(playertoSpawn.name, spawnPos, Quaternion.identity);

        }
    }



    IEnumerator waitCountdown()
    {
     
        yield return new WaitForSeconds(5);
        GetComponent<PhotonView>().RPC("OwnerStuff", RpcTarget.MasterClient);
        
    }  

    [PunRPC]
    void StartGame()
    {
        CountDown.SetActive(true);
       if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(waitCountdown());
        }

    }
   


    [PunRPC]
  void  OwnerStuff()
    {
        GameStarter.SetActive(true);

    }
}
