using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormFollow : MonoBehaviour
{

    [SerializeField] float updateTime;
    float updatingtimer;
   [SerializeField] GameObject[] tails;
    [SerializeField] float slerpspeed;
    [SerializeField] float rotspeed;
    [SerializeField] Vector2[] LastPositions;
    [SerializeField] Quaternion[] lastrot;

    

    void Awake()
    {
        
        for (int i = 0; i < LastPositions.Length; i++)
        {
            LastPositions[i] = tails[i].transform.position;
            lastrot[i] = tails[i].transform.rotation;
        }
       

    }
   

    // Update is called once per frame
    void Update()
    {
        if(updatingtimer <= 0)
        {
            updatingtimer = updateTime;


            for (int i = 0; i < LastPositions.Length; i++)
            {
                LastPositions[i] = tails[i].transform.position;
                lastrot[i] = tails[i].transform.rotation;
            }
            

          

        }
        else
        {
            updatingtimer -= Time.deltaTime;


            for (int i = 0; i < tails.Length - 1; i++)
            {

                tails[i + 1].transform.position = Vector2.Lerp(tails[i + 1].transform.position, LastPositions[i], slerpspeed * Time.deltaTime);
                tails[i + 1].transform.rotation = Quaternion.Lerp(tails[i + 1].transform.rotation, lastrot[i], rotspeed  * Time.deltaTime); 

            }
        }
        
    }
}
