using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeChanger : MonoBehaviour
{
  
    private void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("AudioVolume");
    }

    public void ChangeVolume()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("AudioVolume");

    }

}
