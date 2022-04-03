using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
  [SerializeField]  Text text,text2;
    [SerializeField] Animator animator;
    [SerializeField] GameObject TimeCanvas;
    void Start()
    {
        StartCoroutine("TimerNumerator");
    }

    IEnumerator TimerNumerator()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            int num = int.Parse(text.text);
            num--;
            if(num >= 0)
            {
                text.text = num.ToString();
                text2.text = text.text;
                animator.SetTrigger("pop");
            }
            else
            {
                TimeCanvas.SetActive(true);
            }
          
        }
    }

  
}
