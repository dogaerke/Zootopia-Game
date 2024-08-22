using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class RoomsManager : MonoBehaviour
{
    public List<RoomController> allRoomList;
    public List<RoomController> roomList;
    public List<GiftRoomController> giftRoomList;
    public static RoomsManager Instance { get; private set; }
    private bool _flag;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);

        else
            Instance = this;

        CallRooms();
    }
    private void UpdateRoomVersion(RoomController room)
    {
        foreach (var v in room.roomVersionList)
        {
            if (PlayerPrefs.HasKey(v.name + "Active"))
            {
                _flag = true;
                break;
            }
        }

        if (!_flag)
        {
            room.roomVersionList[0].SetActive(true);
            PlayerPrefs.SetString(room.roomVersionList[0].name + "Active", room.roomVersionList[0].name);
            return;
        }
        foreach (var v in room.roomVersionList)
        {
            if (PlayerPrefs.HasKey(v.name + "Active"))
            {
                v.SetActive(true);
            }
            else
            {
                v.SetActive(false);
            }
        }
        
        
    }
    public bool TryGetEmptyRoom(out RoomController roomController)
    {
        roomController = null;
        var haveEmpty = false;
        foreach (var room in roomList)
        {
            if (room.status != RoomStatus.Available)
                continue;

            roomController = room;
            haveEmpty = true;
            break;
        }

        return haveEmpty;
    }

    private void CallRooms()
    {
        foreach (var room in allRoomList)
        {
            if (PlayerPrefs.HasKey(room.name + "Active"))
            {
                room.gameObject.SetActive(true);
                room.fence.gameObject.SetActive(false);
                UpdateRoomVersion(room);
                
            }

            if (room.CompareTag("GiftRoom") && PlayerPrefs.HasKey(room.name + "_GiftRoomStatus"))
            {
                room.gameObject.SetActive(true);
                room.fence.gameObject.SetActive(false);
            }
        }
    }
}
