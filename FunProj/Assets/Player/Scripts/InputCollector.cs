using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InputCollector : MonoBehaviour
{
    [SerializeField] PlayerController controller;
    float horizontalRaw;
    float verticalRaw;
    public bool locked;
    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
      if (view.IsMine && !controller.is_knocked && !locked)
        {
            horizontalRaw = Input.GetAxisRaw("Horizontal");
        verticalRaw = Input.GetAxisRaw("Vertical");

       

       

       if(Input.GetKeyDown(KeyCode.Space))
        {
            controller.JumpbufferTimer = controller.JumpbufferTime;
            controller.Jump();
        }
       else
        {
            controller.JumpbufferTimer -= Time.deltaTime;
        }

    
      
        if (Input.GetKeyUp(KeyCode.Space))
        {
         
               
         
            controller.isJumping = false;
        }
          
       controller.Movement(horizontalRaw,verticalRaw);
        }


    }
}
