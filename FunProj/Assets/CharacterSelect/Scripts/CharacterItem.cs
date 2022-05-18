using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CharacterItem : MonoBehaviourPunCallbacks
{
    public Text playerName;
    public GameObject PlayerIcons;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    public Image playerAvatar;
    public Sprite[] avatars;

    Player player;

    private void Start()
    {
       playerProperties["OldScore"] = 0;
        playerProperties["NewScore"] = 0;
        playerProperties["WheelUsed"] = false;
        playerProperties["OneVOne"] = false;
        playerProperties["Player1"] = -1;
        playerProperties["Player2"] = -1;


        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void LeftArrowClick()
    {
        if((int)playerProperties["playerAvatar"] == 0 )
        {
            playerProperties["playerAvatar"] = avatars.Length - 1;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] - 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);

    }

    public void RightArrowClick()
    {
        if ((int)playerProperties["playerAvatar"] == avatars.Length-1)
        {
            playerProperties["playerAvatar"] = 0;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] + 1;
        }

      

        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void ApplyLocalChanges()
    {
        PlayerIcons.SetActive(true);

    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(player == targetPlayer)
        {
            UpdateCharacterItem(targetPlayer);
        }
    }
    void UpdateCharacterItem(Player player)
    {
      

        if (player.CustomProperties.ContainsKey("playerAvatar"))
        {
            playerAvatar.sprite = avatars[(int)player.CustomProperties["playerAvatar"]];
            playerProperties["playerAvatar"] = (int)player.CustomProperties["playerAvatar"];
        }
        else
        {
            playerProperties["playerAvatar"] = 0;
        }
    }

    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
        player = _player;
        UpdateCharacterItem(player);
    }
}
