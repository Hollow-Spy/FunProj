using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;



public class CreateNJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] InputField Field;
    public RoomItem roomitemPrefab;
    List<RoomItem> roomitemList = new List<RoomItem>();
    public Transform contentobject;

    float timebetweenupdates = 1.5f;
    float nextupdatetime;

   public static List<RoomInfo> roomListt;

    public List<CharacterItem> characterItemList = new List<CharacterItem>();
    public CharacterItem CharacItemPrefab;
    public Transform CharacItemParent;

    public GameObject StartButton;


    [SerializeField] bool DisableStart;


    [SerializeField] GameObject PlayerListing;

    public void CreateRoom()
    {
        if (Field.text.Length > 0)
        {
            PhotonNetwork.CreateRoom(Field.text, new RoomOptions() { MaxPlayers = 4, BroadcastPropsChangeToAll = true} );
        }
      
    }

    [PunRPC]
    void SetPlayerAmout()
    {
      if(PlayerListing)
        {
            PlayerPrefs.SetInt("PlayerNumber", PhotonNetwork.CurrentRoom.PlayerCount);
        }
      
    
    }
   
 
    public void LoadRandomLeve()
    {
        GetComponent<PhotonView>().RPC("SetPlayerAmout", RpcTarget.All);

        int random = Random.Range(0,3);
        switch(random)
        {
            case 0:
                PhotonNetwork.LoadLevel("Firescape2");
                break;
            case 1:
                PhotonNetwork.LoadLevel("TrainGame1");
                break;
            case 2:
                PhotonNetwork.LoadLevel("EvilSam3");

                break;
        }
    }


    public void LoadLeve(string name)
    {

        

        GetComponent<PhotonView>().RPC("SetPlayerAmout", RpcTarget.All);


       
        PhotonNetwork.LoadLevel(name);
    }

    public void JoinRoom()
    {
        if (Field.text.Length > 0)
        {
            PhotonNetwork.JoinRoom(Field.text);
        }
    }
    public void JoinRoomDirect(string code)
    {
      
            PhotonNetwork.JoinRoom(code);
        
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("CharacterSelect");
    }
  
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        
        if(Time.time >= nextupdatetime)
        {
            UpdateList(roomList);
            nextupdatetime = Time.time + timebetweenupdates;
        }



     
    }

    private void Update()
    {
        if(StartButton!=null)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StartButton.SetActive(true);
            }
            else
            {

                StartButton.SetActive(false);

            }
        }
      
    }
    private void Start()
    {
        if(!DisableStart)
        {
            UpdateList(roomListt);
            UpdateCharacterList();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateCharacterList();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateCharacterList();
    }

    public void LeavingRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Menu");
    }

    public override void OnConnectedToMaster()
    {

        PhotonNetwork.JoinLobby();
    }




    void UpdateList(List<RoomInfo> list)
    {
        foreach(RoomItem item in roomitemList)
        {
            Destroy(item.gameObject);
        }
        roomitemList.Clear();


        foreach(RoomInfo room in list)
        {
            if (room.RemovedFromList)
            {
                return;
            }

            RoomItem newroom = Instantiate(roomitemPrefab, contentobject);
            newroom.SetRoomName(room.Name);
            roomitemList.Add(newroom);
        }


    }


    void UpdateCharacterList()
    {
        foreach(CharacterItem item in characterItemList)
        {
            Destroy(item.gameObject);
        }
        characterItemList.Clear();

        if(PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach(KeyValuePair<int,Player> player in PhotonNetwork.CurrentRoom.Players)
        {
          CharacterItem newCharacterItem =  Instantiate(CharacItemPrefab, CharacItemParent);
            newCharacterItem.SetPlayerInfo(player.Value);

            if(player.Value == PhotonNetwork.LocalPlayer)
            {
                newCharacterItem.ApplyLocalChanges();
            }

            characterItemList.Add(newCharacterItem); 
        }


    }

}
