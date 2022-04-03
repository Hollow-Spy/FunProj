using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindScript : MonoBehaviour
{
    [SerializeField] Vector3 WindDir;
    [SerializeField] bool Active;

    private void Start()
    {
        StartCoroutine(InitialDelay());
    }
    IEnumerator InitialDelay()
    {
        yield return new WaitForSeconds(1.5f);
        Active = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && Active)
        {
            collision.GetComponent<Rigidbody2D>().AddForce(WindDir, ForceMode2D.Force);
        }
    }
}
