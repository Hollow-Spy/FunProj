using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Emotewheel : MonoBehaviourPun
{
    [SerializeField] PlayerController controller;
    [SerializeField] PhotonView view;
    [SerializeField] Animator wheelanimator;
    [SerializeField] Animator playeranimator;
    [SerializeField] FaceExpressions face;

    bool busy, on;
    IEnumerator EmoteCoroutine;


    public void Emoting(string id)
    {
        if(controller.is_idle)
        {
            controller.Emoting(true);
            bool loopable = false;

            switch (id)
            {
                case "happy":
                    
                    playeranimator.Play("Happy");
                    face.Expression("happy");

                    break;
                case "greet":
                    break;
                case "defaultdance":
                    break;
                case "fboy":

                    break;
                    
            }
            if(!loopable)
            {
                AnimationClip[] clips = playeranimator.runtimeAnimatorController.animationClips;
                foreach (AnimationClip clip in clips)
                {
                    if (clip.name == id)
                    {
                        EmoteCoroutine = WaitAnimationNumerator(clip.length);
                        StartCoroutine(EmoteCoroutine);


                    }
                }

            }
          

        }
       
        
    }

    IEnumerator WaitAnimationNumerator(float time)
    {
        Debug.Log(time);
        yield return new WaitForSeconds(time);
        if(controller.is_emoting)
        {
            controller.Emoting(false);
        }
     
    }
   

    // Update is called once per frame
    void Update()
    {
        if(view.IsMine)
        {
            if(Input.GetKeyDown(KeyCode.R) && !busy)
            {
                if(on)
                {
                    Close();
                }
                else
                {
                    on = true;
                    wheelanimator.SetBool("show", true);


                }

            }
            if(!controller.is_idle && on)
            {
                Close();
            }

            if(!controller.is_emoting && EmoteCoroutine != null)
            {
               
                StopCoroutine(EmoteCoroutine);
            }

        }
    }

    public void wheelanimating()
    {
        if(busy)
        {
            busy = false;
        }
        else
        {
            busy = true;

        }

    }
   
    public void Close()
    {
        on = false;
        wheelanimator.SetBool("show",false);

    }
}
