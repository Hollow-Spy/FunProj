using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPitch : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float min, max;
    void Start()
    {
        GetComponent<AudioSource>().pitch = Random.Range(min, max);   
    }

  
}
