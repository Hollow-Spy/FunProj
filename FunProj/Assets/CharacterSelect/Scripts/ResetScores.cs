using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScores : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("OldScore", 0);
        PlayerPrefs.SetInt("NewScore", 0);
    }

   
}
