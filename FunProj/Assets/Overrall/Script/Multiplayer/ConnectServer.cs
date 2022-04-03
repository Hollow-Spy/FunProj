using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using UnityEngine.UI;
public class ConnectServer : MonoBehaviourPunCallbacks
{
      [SerializeField] InputField text;
    [SerializeField] Button button;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetString("Nickname").Length > 0)
        {
        
            text.text = PlayerPrefs.GetString("Nickname");
        }
       
    }
     
    public void JoinClick()
    {
        if (text.text.Length < 1)
        {
            return;
        }
       

        PhotonNetwork.NickName = text.text;
        button.GetComponentInChildren<Text>().text = "...";
        PhotonNetwork.AutomaticallySyncScene = true;
        button.interactable = false;
        PlayerPrefs.SetString("Nickname", text.text);

        PhotonNetwork.ConnectUsingSettings();
    
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
      
       SceneManager.LoadScene("Menu");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

        CreateNJoinRooms.roomListt = roomList;


    }
}
