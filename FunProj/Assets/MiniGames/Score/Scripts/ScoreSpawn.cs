using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class ScoreSpawn : MonoBehaviourPunCallbacks /*, IPunObservable*/
{
    public int PlayersJoined;

    public GameObject[] playerPrefabs;
    public int pos;
    [SerializeField] Transform[] spawnPositions;
    PhotonView view;

    [SerializeField] bool active;

    [SerializeField] int[] PlayerIds;

    [SerializeField] ScoreInfoDisplay[] displays;

    /*
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(pos);


        }
        else
        {
            if (stream.IsReading)
            {
                pos = (int)stream.ReceiveNext();
              
            }

        }
    }*/

 


    private void Start()
    {
       
        view = GetComponent<PhotonView>();
        PlayerIds = new int[PlayerPrefs.GetInt("PlayerNumber")];
        for (int i = 0; i < PlayerIds.Length; i++)
        {
            PlayerIds[i] = -1;
        }


        active = true;


        StartCoroutine(SpawnPlayer());
    }

  

    IEnumerator SpawnPlayer()
    {
        yield return new WaitForSeconds(.1f );
        int pos = 0;
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
          if(  players[i] == PhotonNetwork.LocalPlayer  )
            {
                pos = i;
            }
        }
      


        
        Vector3 spawnPos = new Vector3(spawnPositions[pos].position.x - 0.7570001f, spawnPositions[pos].position.y - 60f, spawnPositions[pos].position.z);
      
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
          
           if(PlayersJoined == PlayerPrefs.GetInt("PlayerNumber"))
            {
                view.RPC("SecondCount", RpcTarget.All);
            }
           

        }
    }


    [PunRPC]
    void SecondCount()
    {
        StartCoroutine(SecondCountNumerator());
    }
    IEnumerator SecondCountNumerator()
    {
        yield return new WaitForSeconds(1.5f);
        for(int i = 0;i<displays.Length;i++)
        {
            displays[i].SetNewBar();
        }

    }


}
