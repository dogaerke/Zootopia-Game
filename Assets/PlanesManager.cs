using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanesManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> planes;
    private bool _flag;

    private void Start()
    {
        UpdatePlane();
    }

    private void UpdatePlane()
    {
        foreach (var p in planes)
        {
            if (PlayerPrefs.HasKey(p.name + "Active"))
            {
                _flag = true;
                break;
            }
        }

        if (!_flag)
        {
            planes[0].SetActive(true);
            PlayerPrefs.SetString(planes[0].name + "Active", planes[0].name);
            return;
        }
        foreach (var p in planes)
        {
            p.SetActive(PlayerPrefs.HasKey(p.name + "Active"));
        }
    }
}
