using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SetShotgun : MonoBehaviourPun
{
    [SerializeField] bool Shotgun;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && Shotgun)
        {
            collision.GetComponent<PlayerController>().is_shotgun = true;

            collision.GetComponent<PlayerController>().EquipShotgun();

          
        }
    }
}
