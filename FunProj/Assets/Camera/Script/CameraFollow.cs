using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;

    public float smoothSpeed;
    public Vector3 offset;

    

    private void FixedUpdate()
    {
        if(target)
        {
            Vector3 desiredPos = target.position + offset;
            Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothPos;
        }
       


    }

}
