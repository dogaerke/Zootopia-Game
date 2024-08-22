using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneOpener : MonoBehaviour
{
    [SerializeField] private List<Transform> openers;
    [SerializeField] private List<Transform> closers;

    private void OnEnable()
    {
        PlayerPrefs.SetString(name + "Active", name);
    }

    public void DoOpenAndClose()
    {
        foreach (var o in openers)
        {
            o.gameObject.SetActive(true);
            PlayerPrefs.SetString(o.name + "Active", o.name);
        }
        
        foreach (var c in closers)
        {
            c.gameObject.SetActive(false);
            PlayerPrefs.DeleteKey(c.name + "Active");

        }
        
        transform.gameObject.SetActive(false);
        PlayerPrefs.DeleteKey(name + "Active");
    }
    /*public void Open()
    {
        foreach (var o in openers)
        {
            o.gameObject.SetActive(true);
        }
    }

    public void Close()
    {
        foreach (var c in closers)
        {
            c.gameObject.SetActive(false);
        }
        
    }*/
}
