using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class HairOffsetClass
 {

    public HairRoot Root;
    public Vector2 IdleOffset;
    public Vector2 RunOffset;
    public Vector2 FallOffset;
    public Vector2 CrouchOffset;
    public Vector2 NodOffset;
}


public class HairManager : MonoBehaviour
{
    [SerializeField] HairOffsetClass[] HairParts;
    public Animator anim;
    [SerializeField] PlayerController playercontroll;

  

    public void IdleHair(int flip)
    {
       for(int i=0;i<HairParts.Length;i++)
        {
            HairParts[i].Root.HairOffset = new Vector2(HairParts[i].IdleOffset.x * flip, HairParts[i].IdleOffset.y);
        }
    }

    public void RunHair(int flip)
    {
        for (int i = 0; i < HairParts.Length; i++)
        {
            HairParts[i].Root.HairOffset = new Vector2(HairParts[i].RunOffset.x * flip,HairParts[i].RunOffset.y) ;
        }
    }

    public void FallHair(int flip)
    {
        for (int i = 0; i < HairParts.Length; i++)
        {
            HairParts[i].Root.HairOffset = new Vector2(HairParts[i].FallOffset.x * flip, HairParts[i].FallOffset.y);
        }
    }

    public void CrouchHair(int flip)
    {
        for (int i = 0; i < HairParts.Length; i++)
        {
            HairParts[i].Root.HairOffset = new Vector2(HairParts[i].CrouchOffset.x * flip, HairParts[i].CrouchOffset.y);
        }
    }
    public void NodeHair(int flip)
    {
        for (int i = 0; i < HairParts.Length; i++)
        {
            HairParts[i].Root.HairOffset = new Vector2(HairParts[i].NodOffset.x * flip, HairParts[i].NodOffset.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
       if(playercontroll.is_idle)
        {
            IdleHair(playercontroll.flipdir);
        }
       if(playercontroll.is_running)
        {
            RunHair(playercontroll.flipdir);

        }
        if (playercontroll.is_Airborne)
        {
            FallHair(playercontroll.flipdir);

        }
        if (playercontroll.is_crouching)
        {
            CrouchHair(playercontroll.flipdir);

        }
        if (playercontroll.is_noding)
        {
            NodeHair(playercontroll.flipdir);

        }
    }
}
