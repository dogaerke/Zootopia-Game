using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomCameraTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            if (!CameraSwitcher.IsActiveCamera(cam)) 
                CameraSwitcher.SwitchCamera(cam);
        
    }
}
