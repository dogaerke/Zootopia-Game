using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftRoomTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private StackTriggerHandler stackZoneSupply;
    
    private void OnDisable()
    {
        if(stackZoneSupply != null)
            stackZoneSupply.gameObject.SetActive(true);
    }
    
}
