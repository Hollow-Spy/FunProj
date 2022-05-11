using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviourPunCallbacks
{
    [SerializeField] Slider volumeslider;
   public List<ResItems> resitems = new List<ResItems>();
    public int ResolutionIndex;
    [SerializeField] Text ResolutionText;


    [System.Serializable]
   public class ResItems
        {
        public int Horizontal, Vertical;

        }

  
    private void Start()
    {

        if (PlayerPrefs.GetInt("Fullscreen") == 0)
        {
            Screen.fullScreen = false;

        }
        else
        {
            Screen.fullScreen = true;
        }


        bool FoundRes = false;
        for (int i = 0; i < resitems.Count; i++)
        {
            if (Screen.width == resitems[ResolutionIndex].Horizontal && Screen.height == resitems[ResolutionIndex].Vertical)
            {
                FoundRes = true;

                ResolutionIndex = i;
                UpdateResLabel();
                ApplyResolution();

            }
        }
        if (!FoundRes)
        {
            ResItems newitem = new ResItems();
            newitem.Horizontal = Screen.width;
            newitem.Vertical = Screen.height;

            resitems.Add(newitem);
            ResolutionIndex = resitems.Count - 1;
            UpdateResLabel();
            ApplyResolution();
        }



        ApplyResolution();

        volumeslider.value = PlayerPrefs.GetFloat("AudioVolume");
    }


    public void ResolutionLeft()
    {
        ResolutionIndex--;
        if(ResolutionIndex < 0)
        {
            ResolutionIndex = resitems.Count-1;
        }
        UpdateResLabel();
    }


    public void ResolutionRight()
    {
        ResolutionIndex++;
        if (ResolutionIndex >= resitems.Count)
        {
            ResolutionIndex = 0;
        }
        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        ResolutionText.text = resitems[ResolutionIndex].Horizontal.ToString() + "x" + resitems[ResolutionIndex].Vertical.ToString();

    }
    public void ApplyResolution()
    {
        Screen.SetResolution(resitems[ResolutionIndex].Horizontal, resitems[ResolutionIndex].Vertical, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", ResolutionIndex);
    }


    public void VolumeChanged()
    {
        PlayerPrefs.SetFloat("AudioVolume", volumeslider.value);

        FindObjectOfType<VolumeChanger>().ChangeVolume();
    }

    public void FullScreenToggle()
    {
        Screen.fullScreen = !Screen.fullScreen;

        if(Screen.fullScreen)
        {
            PlayerPrefs.SetInt("Fullscreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Fullscreen", 0);

        }
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
