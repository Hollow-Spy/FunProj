using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviourPunCallbacks
{
    [SerializeField] Slider volumeslider;
    

    private void Start()
    {
        volumeslider.value = PlayerPrefs.GetFloat("AudioVolume");
    }

    public void VolumeChanged()
    {
        PlayerPrefs.SetFloat("AudioVolume", volumeslider.value);

        FindObjectOfType<VolumeChanger>().ChangeVolume();
    }

    public void FullScreenToggle()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

 public void PlayerDisconnectPhoton()
    {
       
        
            PhotonNetwork.LeaveRoom();

        

    }


    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Menu");
    }


}
