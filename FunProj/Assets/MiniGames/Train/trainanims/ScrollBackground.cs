using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
   [SerializeField] Vector3 spawnPos,endPos;
    [SerializeField] float speed;
    


    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
        if(Vector2.Distance(transform.position,endPos) < .002f)
        {
            transform.position = spawnPos;
        }
    }
}
