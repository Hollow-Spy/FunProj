using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ScoreCounter : MonoBehaviourPunCallbacks
{
    bool Counting;
    GameObject Player;
    GameObject[] Players;
    PlayerController controller;
    [SerializeField] int ScorePerSecond;
    [SerializeField] int maxPlayersAliveAllowed;
   public int playersAlive;
    [SerializeField] GameObject TimeCanvas,TransitionCanvas;
    PhotonView view;

   // ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    int initialScore;

    float TransitionTime=1;

   [SerializeField] bool DyingRemovesScore;

  

    void Start()
    {
       /* 
         PhotonNetwork.LocalPlayer.CustomProperties["NewScore"]); Get the property
        var hash = PhotonNetwork.LocalPlayer.CustomProperties;    get the hash or package with all info attached to local player
        hash["NewScore"] = 100;       set a variables value
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);     upload it 
      
        */
        initialScore = 0;
        view = GetComponent<PhotonView>();

      
    
        Players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0;i< Players.Length;i++)
        {
           if (Players[i].GetComponent<PhotonView>().IsMine  )
            {
                Player = Players[i];
                controller = Player.GetComponent<PlayerController>();
            }

        }


        playersAlive = FindObjectOfType<SpawnPlayers>().necessaryplayers;
       

        StartCoroutine( ScoreIncrease() );
    }




    public void StopCount()
    {
        Counting = false;
    }
   
    public void PlayerDiedAll()
    {
        view.RPC("PlayerDied", RpcTarget.All);
    }


    [PunRPC]
     void PlayerDied()
    {
        PhotonView theview=null;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for(int i=0;i<players.Length;i++)
        {
            if(players[i].GetComponent<PhotonView>().IsMine)
            {
                theview = players[i].GetComponent<PhotonView>();
            }
        }


        if(theview.IsMine)
        {
            playersAlive--;
          
            if (DyingRemovesScore && playersAlive > 0 && theview.GetComponent<PlayerController>().is_dead)
            {
                var hash = PhotonNetwork.LocalPlayer.CustomProperties;
                hash["NewScore"] = 0;
                //PlayerPrefs.SetInt("NewScore", 0);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            }

            if (maxPlayersAliveAllowed >= playersAlive)
            {

                TimeCanvas.SetActive(true);

            }
        }
      
    }


    private void Update()
    {
        if(TimeCanvas.activeSelf)
        {
            TransitionTime -= Time.deltaTime;

            if(TransitionTime < 0)
            {
                TransitionCanvas.SetActive(true);
                Time.timeScale = 0;
                TransitionTime -= .1f;
            }
           
            if (TransitionTime < -7)
            {
                TransitionTime = 999;
                PhotonNetwork.LoadLevel("ScoreShow");
            }

        }
    }

    IEnumerator ScoreIncrease()
    {
        Counting = true;
        while (Counting)
        {
           
            initialScore += ScorePerSecond;

            var hash = PhotonNetwork.LocalPlayer.CustomProperties;
            hash["NewScore"] = initialScore;
           
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
           
            yield return new WaitForSeconds(1);

        }
       

    }



}
