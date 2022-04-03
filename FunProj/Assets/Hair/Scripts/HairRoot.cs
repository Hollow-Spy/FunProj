using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairRoot : MonoBehaviour
{
    public Vector2 HairOffset;
    Transform hairAnchor;
    [SerializeField] Transform[] hairPart;
    [SerializeField] float LerpSpeed;

    private void Awake()
    {
        hairAnchor = GetComponent<Transform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform piece2follow = hairAnchor;
        for(int i=0;i<hairPart.Length;i++)
        {
            if(!hairPart[i].Equals(hairAnchor))
            {
               Vector2 targetpos = (Vector2)piece2follow.position + HairOffset;

                Vector2 lerppos = Vector2.Lerp(hairPart[i].position, targetpos, Time.deltaTime * LerpSpeed);
                hairPart[i].position = lerppos;

                piece2follow = hairPart[i];
            }
       


        }


    }
}
