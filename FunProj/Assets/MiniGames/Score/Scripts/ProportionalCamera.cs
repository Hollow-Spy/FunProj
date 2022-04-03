using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProportionalCamera : MonoBehaviour
{
    public bool active=true;
   public float y, z;
   public float targetY;
    [SerializeField] Transform CameraTransform;
    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            if (targetY > y)
            {
                targetY = y;
            }


            float targetZ = (z * targetY) / y;

            if (targetY == 0)
            {
                targetZ = -10;
            }

            CameraTransform.position = new Vector3(0, targetY, targetZ);
        }
       
    }
}
