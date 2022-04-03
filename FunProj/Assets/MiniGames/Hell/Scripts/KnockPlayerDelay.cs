using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockPlayerDelay : MonoBehaviour
{
    bool cooldown;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !cooldown)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            cooldown = true;
            StartCoroutine(waitNumerator());
        }
    }

    IEnumerator waitNumerator()
    {
        yield return new WaitForSeconds(3);
        cooldown = false;
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
