using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormSound : MonoBehaviour
{
    AudioSource sound;
    [SerializeField] Transform wormSpot;
    [SerializeField] float distanceDivider;
    private void Start()
    {
        sound = GetComponent<AudioSource>();
        if(wormSpot == null)
        {
            wormSpot = GameObject.Find("WormSpot").transform;
        }
    }
    // Update is called once per frame
    void Update()
    {
        sound.volume = 1 - Vector2.Distance(transform.position, wormSpot.position) / distanceDivider;
        //bigger number louder sound
    }
}
