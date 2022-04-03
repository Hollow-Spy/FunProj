using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TileSpawner : MonoBehaviourPun
{
    [SerializeField] int GenAmout;
    [SerializeField] int serie;
    [SerializeField] GameObject[] ARooms,BRoombs,CRooms;
    [SerializeField] bool Horizontal;
    [SerializeField] float YAxis;

    void Start()
    {
        for (int i = 1; i < GenAmout + 1; i++)
        {
            if (serie > 2)
            {
                serie = 0;
            }
            if (Horizontal)
            {
                switch (serie)
                {
                    case 0:
                        PhotonNetwork.Instantiate(ARooms[Random.Range(0, ARooms.Length)].name, new Vector2(2.6f * i, YAxis), Quaternion.identity);
                        break;
                    case 1:
                        PhotonNetwork.Instantiate(BRoombs[Random.Range(0, BRoombs.Length)].name, new Vector2(2.6f * i,YAxis ), Quaternion.identity);
                        break;
                    case 2:
                        PhotonNetwork.Instantiate(CRooms[Random.Range(0, CRooms.Length)].name, new Vector2(2.6f * i, YAxis), Quaternion.identity);

                        break;
                }
                serie++;
            }
            else
            {
                switch (serie)
                {
                    case 0:
                        PhotonNetwork.Instantiate(ARooms[Random.Range(0, ARooms.Length)].name, new Vector2(0, 2.6f * i), Quaternion.identity);
                        break;
                    case 1:
                        PhotonNetwork.Instantiate(BRoombs[Random.Range(0, BRoombs.Length)].name, new Vector2(0, 2.6f * i), Quaternion.identity);
                        break;
                    case 2:
                        PhotonNetwork.Instantiate(CRooms[Random.Range(0, CRooms.Length)].name, new Vector2(0, 2.6f * i), Quaternion.identity);

                        break;
                }
                serie++;
            }

           

        }
    }

  
}
