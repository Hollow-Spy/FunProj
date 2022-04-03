using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpFire : MonoBehaviour
{
    [SerializeField] Vector2 Direction;
    [SerializeField] float SpeedCap;
    [SerializeField] float MoveSpeed;
    [SerializeField] float IncrementAmout;
    [SerializeField] float IncrementTime;
    void Start()
    {
        StartCoroutine(MoveUp());
        StartCoroutine(SpeedIncrement());
    }
    IEnumerator MoveUp()
    {
        yield return new WaitForSeconds(5);
        while(true)
        {
            yield return null;
            transform.position = new Vector2(transform.position.x + Direction.x * MoveSpeed * Time.deltaTime , transform.position.y + Direction.y * MoveSpeed * Time.deltaTime);
        }

    }
    IEnumerator SpeedIncrement()
    {
        while(true)
        {
            yield return new WaitForSeconds(IncrementTime);
            float newSpeed = MoveSpeed + IncrementAmout;
            if(newSpeed < SpeedCap)
            {
                MoveSpeed += IncrementAmout;
            }

        }
    }
    
}
