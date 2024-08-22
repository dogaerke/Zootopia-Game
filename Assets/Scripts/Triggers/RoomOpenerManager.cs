using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomOpenerManager : MonoBehaviour
{
    public List<GroundTriggerHandler> roomOpeners;
    private bool _flagRoomOpener;
    private bool _flagRooms = true;
    private void Start()
    {
        foreach (var r in RoomsManager.Instance.allRoomList)
        {
            if (!r.gameObject.activeInHierarchy)
            {
                _flagRooms = false;
                break;
            }
        }

        if (_flagRooms)
        {
            foreach (var opener in roomOpeners)
            {
                PlayerPrefs.DeleteKey(opener.id + "Active");
                opener.gameObject.SetActive(false);
                return;
            }
            
        }
        foreach (var opener in roomOpeners)
        {
            opener.gameObject.SetActive(PlayerPrefs.HasKey(opener.id + "Active"));
            
        }
        foreach (var opener in roomOpeners)
        {
            if (PlayerPrefs.HasKey(opener.id + "Active"))
            {
                _flagRoomOpener = true;
                break;
            }
        }

        if (!_flagRoomOpener)
        {
            PlayerPrefs.SetString(roomOpeners[0].id + "Active", roomOpeners[0].id);  
            roomOpeners[0].gameObject.SetActive(true);
        
        }
    }
    
}