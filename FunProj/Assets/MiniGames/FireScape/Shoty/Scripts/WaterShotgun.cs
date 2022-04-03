using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaterShotgun : MonoBehaviourPun
{


    float offsetX;
    [SerializeField] float shootDelay;
    float delay;
    [SerializeField] PlayerController controller;
    [SerializeField] Animator animator;
    [SerializeField] float Dashstrengh;
    PhotonView view;
    private void Start()
    {
        animator = GetComponent<Animator>();

        view = GetComponent<PhotonView>();
    }


    // Update is called once per frame
    void Update()
    {




        if (controller.view.IsMine)
        {

            Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if (controller.flipdir == 1)
            {

                offsetX = 0;

            }
            else
            {

                offsetX = 180;


            }
            float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            if (controller.flipdir == -1)
            {
                rotZ *= -1;

            }


            if (Input.GetMouseButtonDown(0) && delay <= 0)
            {
            
                delay = shootDelay;
                view.RPC("ShotEffect", RpcTarget.All);
                if (controller.is_Airborne)
                {
                    controller.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    controller.GetComponent<Rigidbody2D>().Sleep();

                    controller.GetComponent<Rigidbody2D>().AddForce(-transform.right * Dashstrengh  , ForceMode2D.Impulse);
                }

            }


            if (delay > 0)
            {
                delay -= Time.deltaTime;
            }






            transform.rotation = Quaternion.Euler(offsetX, 0, rotZ);

        }


    }

   [PunRPC]
    void ShotEffect()
    {
        animator.SetTrigger("shoot");
    }

}
