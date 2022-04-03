using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilSamDeath : MonoBehaviour
{
    [SerializeField] Transform restPos;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EvilSamChase evil;
        if( collision.TryGetComponent<EvilSamChase>(out evil) )
        {
            evil.transform.position = new Vector3(restPos.position.x,restPos.position.y,evil.transform.position.z)  ;

        }
    }
}
