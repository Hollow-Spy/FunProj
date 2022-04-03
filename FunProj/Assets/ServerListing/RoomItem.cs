using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Text roomName;
    CreateNJoinRooms manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<CreateNJoinRooms>();
    }

    public void SetRoomName(string _roomName)
    {
        roomName.text = _roomName;
    }

    public void JoinRoomDirect()
    {
        manager.JoinRoomDirect(roomName.text);
    }

  
}
