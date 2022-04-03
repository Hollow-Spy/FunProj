using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHuskie : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && collision.GetComponent<InputCollector>())
        {
            collision.GetComponent<InputCollector>().locked=true;
            collision.transform.localScale = new Vector3(300, 300, 1);
            collision.GetComponent<Rigidbody2D>().gravityScale = 8;
            collision.GetComponent<PlayerController>().SolidHair();

        }
    }

}
