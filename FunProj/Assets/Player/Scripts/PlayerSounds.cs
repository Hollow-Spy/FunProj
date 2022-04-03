using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSounds : MonoBehaviourPun
{

    AudioSource audioplayer;
    PlayerController controller;
    Rigidbody2D body;
    [SerializeField] GameObject BounceSound;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        body = GetComponent<Rigidbody2D>();
        audioplayer = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(controller.GetComponent<PhotonView>().IsMine && collision.relativeVelocity.magnitude > .15f && controller.is_knocked)
        {
            controller.InstantiateAll(BounceSound.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(controller.view.IsMine && controller.is_running )
        {
           
            if (!audioplayer.isPlaying)
            {
                audioplayer.pitch = Random.Range(.9f, 1.2f);
                audioplayer.Play();
            }
        }
       


      
    }
}
