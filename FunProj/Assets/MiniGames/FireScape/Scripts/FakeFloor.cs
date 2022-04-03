using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FakeFloor : MonoBehaviour
{
    bool active=true;
    PhotonView view;
    [SerializeField] GameObject floorPiece;
    [SerializeField] GameObject particles;
    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
     
        if(collision.CompareTag("Player") && collision.GetComponent<PhotonView>().IsMine && active)
        {
          
            int rand = Random.Range(0, 2);
            if(rand == 0)
            {
                
                view.RPC("FloorBreak", RpcTarget.All);
            }
          
        }
    }

    [PunRPC]
    private void FloorBreak()
    {
        active = false;
        particles.SetActive(true);
        floorPiece.SetActive(false);
    }
    


}
