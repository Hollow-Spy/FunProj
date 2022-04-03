using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotBreaker : MonoBehaviour
{
    [SerializeField] Animator potanimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            potanimator.SetTrigger("pop");
            Destroy(this);
        }
    }
}
