using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnerScript : MonoBehaviourPun
{
   
    public float minTime, maxTime;
    public float minDecrement, maxDecrement;
    public float minCap, maxCap;
    public float DecrementTime;

    [SerializeField] GameObject[] ObjectsToSpawn;
    [SerializeField] bool[] Spawned;
    void Start()
    {
        StartCoroutine("SpawnerNumerator");
        StartCoroutine("DecrementNumerator");
        Spawned = new bool[ObjectsToSpawn.Length];

    }
    IEnumerator SpawnerNumerator()
    {
        while(true)
        {
           
            float randTime = Random.Range(minTime, maxTime);
            
            yield return new WaitForSeconds(randTime);

            int randSpawn=0;
            bool found=false;
            for (int i = 0; i<100;i++)
            {
                 randSpawn = Random.Range(0, ObjectsToSpawn.Length);
                if (!Spawned[randSpawn])
                {
                    Spawned[randSpawn] = true;
                    found = true;
                    i = 101;
                }

            }
            if(!found)
            {
                for(int a=0;a<Spawned.Length;a++)
                {
                    Spawned[a] = false;
                }

            }


            PhotonNetwork.Instantiate(ObjectsToSpawn[randSpawn].name, ObjectsToSpawn[randSpawn].transform.position, ObjectsToSpawn[randSpawn].transform.rotation);
        }
      


    }
    IEnumerator DecrementNumerator()
    {
        while (true)
        {
            yield return new WaitForSeconds(DecrementTime);
           if( (minTime - minDecrement) > minCap)
            {
                minTime -= minDecrement;
            }

           if((maxTime - maxDecrement) > maxCap)
            {
                maxTime -= maxDecrement;
            }
            
        }
    }
    // Update is called once per frame

}
