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
    [SerializeField] public GameObject ScoreCounterObj;
  public int necessaryplayers;

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
          
            if (PlayersJoined == necessaryplayers )
            {

                active = false;
                view.RPC("StartGame", RpcTarget.All);
               
            }

        }
    }


    private void Awake()
    {
      


        if ((bool)PhotonNetwork.MasterClient.CustomProperties["OneVOne"] == true)
        {
          
            if(FindObjectOfType<ScoreCounter>() )
            {
                FindObjectOfType<ScoreCounter>().playersAlive = 2;
            }
         

            necessaryplayers = 2;
            return;
        }

        if ((bool)PhotonNetwork.MasterClient.CustomProperties["OneVThreeOver"] == true)
        {
            if((int)PhotonNetwork.MasterClient.CustomProperties["Player1"] != -1)
            {
              
                necessaryplayers = 1;
            }
            else
            {
                necessaryplayers = PhotonNetwork.CurrentRoom.PlayerCount - 1;
            }
            return;
        }



            necessaryplayers = PlayerPrefs.GetInt("PlayerNumber");


    }

    private void Start()
    {
        Time.timeScale = 1;
      



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

    





        if ((bool)PhotonNetwork.MasterClient.CustomProperties["OneVOne"]==true)
        {
        

            ResetOldScore();


            int winindex = (int)PhotonNetwork.MasterClient.CustomProperties["Player1"];
            int index = (int)PhotonNetwork.MasterClient.CustomProperties["Player2"];

            if (PhotonNetwork.LocalPlayer == players[winindex] || PhotonNetwork.LocalPlayer == players[index])
            {
              

                PhotonInstantiatePlayer(pos);
            }
            yield break;
        }

        if ((bool)PhotonNetwork.MasterClient.CustomProperties["GoldenPoint"] == true)
        {

            ResetOldScore();
        }

        if((bool)PhotonNetwork.MasterClient.CustomProperties["OneVThreeOver"] )
        {
            if((int)PhotonNetwork.MasterClient.CustomProperties["Player1"] != -1 )
            {
                Debug.Log("HI IS NOT -1");
                if(PhotonNetwork.LocalPlayer == players[(int)PhotonNetwork.MasterClient.CustomProperties["Player1"]])
                {
                    PhotonInstantiatePlayer(pos);
                }


            }
            else
            {
                if (PhotonNetwork.LocalPlayer != players[(int)PhotonNetwork.MasterClient.CustomProperties["Player2"]])
                {
                    PhotonInstantiatePlayer(pos);
                }
                
            }
            yield break;

        }

     
            PhotonInstantiatePlayer(pos);
        

    }

    void ResetOldScore()
    {
        var hash = PhotonNetwork.LocalPlayer.CustomProperties;
        hash["OldScore"] = 0;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }


    void PhotonInstantiatePlayer(int pos)
    {
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

        ScoreCounterObj.SetActive(true);

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
