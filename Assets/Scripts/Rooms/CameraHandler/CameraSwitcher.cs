using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public static class CameraSwitcher
{
    public static List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();
    public static CinemachineVirtualCamera ActiveCamera = null;

    public static bool IsActiveCamera(CinemachineVirtualCamera camera)
    {
        return camera == ActiveCamera;
    }

    public static void SwitchCamera(CinemachineVirtualCamera camera)
    {
        camera.Priority = 10;
        ActiveCamera = camera;

        foreach (var cam in cameras)
        {
            if (cam != camera && cam.Priority != 0)
            {
                cam.Priority = 0;
            }
        }
    }

    public static void Register(CinemachineVirtualCamera virtualCamera)
    {
        cameras.Add(virtualCamera);
    }

    public static void Unregister(CinemachineVirtualCamera virtualCamera)
    {
        cameras.Remove(virtualCamera);
    }
    
    
}
