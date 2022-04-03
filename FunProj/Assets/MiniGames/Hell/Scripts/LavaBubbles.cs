using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LavaBubbles : MonoBehaviourPun
{
    [SerializeField] GameObject Particles;
    [SerializeField] GameObject Trigger;
    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        if(PhotonNetwork.IsMasterClient )
        {
            StartCoroutine(WaitRandom());

        }
    }
    IEnumerator WaitRandom()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(4, 7));

            view.RPC("Shoot", RpcTarget.All);
            yield return new WaitForSeconds(1.3f);
            view.RPC("Knock", RpcTarget.All);



        }


    }

    [PunRPC]
    void Shoot()
    {
        Particles.SetActive(false);
        Particles.SetActive(true);


    }

    [PunRPC]
    void Knock()
    {
        Instantiate(Trigger, transform.position, Quaternion.identity);

    }

   
}
