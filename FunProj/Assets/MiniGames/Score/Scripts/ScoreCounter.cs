using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ScoreCounter : MonoBehaviourPun
{
    bool Counting;
    GameObject Player;
    GameObject[] Players;
    PlayerController controller;
    [SerializeField] int ScorePerSecond;
    [SerializeField] int maxPlayersAliveAllowed;
    int playersAlive;
    [SerializeField] GameObject TimeCanvas,TransitionCanvas;
    PhotonView view;

    int initialScore;

    float TransitionTime=1;

   [SerializeField] bool DyingRemovesScore;

    void Start()
    {
        
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
        playersAlive = PhotonNetwork.CurrentRoom.PlayerCount;
        Debug.Log(playersAlive);

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
            Debug.Log(playersAlive);
            if (DyingRemovesScore && playersAlive > 0 && theview.GetComponent<PlayerController>().is_dead)
            {

                PlayerPrefs.SetInt("NewScore", 0);
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
            PlayerPrefs.SetInt("NewScore", initialScore);
            yield return new WaitForSeconds(1);

        }
       

    }



}
