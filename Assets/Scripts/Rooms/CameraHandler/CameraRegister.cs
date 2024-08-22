using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraRegister : MonoBehaviour
{
    private void OnEnable()
    {
        CameraSwitcher.Register(GetComponent<CinemachineVirtualCamera>());
    }

    private void OnDisable()
    {
        CameraSwitcher.Unregister(GetComponent<CinemachineVirtualCamera>());
    }
}
