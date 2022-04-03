using UnityEngine;

public class SpawnOnCam : MonoBehaviour
{
   
    void Start()
    {
        transform.position = GameObject.FindGameObjectWithTag("MainCamera").transform.position;

    }

    
}
