using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerOpenerManager : MonoBehaviour
{
    public List<GroundTriggerHandler> cleanerOpeners;
    private void Start()
    {
        foreach (var opener in cleanerOpeners)
        {
            opener.gameObject.SetActive(PlayerPrefs.HasKey(opener.id + "Active"));
            
        }
    }
}
