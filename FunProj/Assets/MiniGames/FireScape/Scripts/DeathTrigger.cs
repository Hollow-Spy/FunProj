using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DeathTrigger : MonoBehaviourPun
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && collision.GetComponent<PhotonView>().IsMine )
        {
            collision.GetComponent<PlayerController>().DeathALL();
        }
    }
}
