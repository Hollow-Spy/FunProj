using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FaceExpressions : MonoBehaviour
{
    Animator animator;


  
    float blinktimer;
    [SerializeField] float blinktime;
   

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (animator.GetBool("expressing"))
        {
            blinktimer = blinktime;
        }
        else
        {

            blinktimer -= Time.deltaTime;

            if(blinktimer <= 0)
            {
                blinktimer = blinktime;

                int rand = Random.Range(0, 2);
             if(animator.GetBool("blinkR"))
                {

                    if(rand==0)
                    {
                        animator.SetBool("Rblink", true);
                    }
                    else
                    {
                        animator.SetBool("blinkR", false);
                        animator.SetBool("Rblink", false);

                    }
                }
             else
                {

                    if (rand == 0)
                    {
                        animator.SetBool("blinkL", false);
                        animator.SetBool("blinkR", true);
                    }
                    else
                    {
                       
                        animator.SetBool("blinkL", true);
                    }

                }

               

            }

           
            
        }


       
    }

    public void Expression(string exp)
    {
        animator.SetBool("expressing", true);

        animator.SetBool("happy", false);
        animator.SetBool("sad", false);
        animator.SetBool("suprised", false);
        animator.SetBool("sus", false);
        animator.SetBool("blinkL", false);
        animator.SetBool("fboy", false);
        animator.SetBool("Rblink", false);
        animator.SetBool("blinkR", false);
        animator.SetBool("angry", false);


        switch (exp)
        {
            case "angry":
                animator.SetBool("angry", true);
                break;
            case "suprised":
                animator.SetBool("suprised", true);
                break;
            case "sus":
                animator.SetBool("sus", true);
                break;
            case "fboy":
                animator.SetBool("fboy", true);
                break;
            case "sad":
                animator.SetBool("sad", true);
                break;
            case "happy":
                animator.SetBool("happy", true);
                break;
            default:
                animator.SetBool("expressing", false);
                break;
        }

    }
}
