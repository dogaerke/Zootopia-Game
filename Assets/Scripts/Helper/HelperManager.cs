using System;
using System.Collections;
using System.Collections.Generic;
using HelperStatusNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class HelperManager : MonoBehaviour
{
    [SerializeField] private List<StackTriggerHandler> stackFoodPoints;
    [SerializeField] private List<StackTriggerHandler> stackSouvenirPoints;
    [SerializeField] private List<SupplyZoneHandler> needFoodZones;
    [SerializeField] private List<SupplyZoneHandler> needSouvenirZones;

    [SerializeField] private Dictionary<int, List<CleanerZoneHandler>> needCleanZonesDict
        = new Dictionary<int, List<CleanerZoneHandler>>();
    
    [SerializeField] private List<Helper> helperNpcList;
    public ReceptionistPrefs receptionist;
    
    
    private Transform _cleanerHelperIdlePoint;
    private Transform _carrierHelperIdlePoint;
    public List<SupplyZoneHandler> NeedFoodZones() => needFoodZones;
    public List<SupplyZoneHandler> NeedSouvenirZones() => needSouvenirZones; 
    
    public static HelperManager Instance { get; private set; }
    public Transform CleanerHelperIdlePoint => _cleanerHelperIdlePoint;

    public Transform CarrierHelperIdlePoint => _carrierHelperIdlePoint;

    public Dictionary<int, List<CleanerZoneHandler>> NeedCleanZonesDict => needCleanZonesDict;

    public List<StackTriggerHandler> StackFoodPoints => stackFoodPoints;

    public List<StackTriggerHandler> StackSouvenirPoints => stackSouvenirPoints;

    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    private void Start()
    {
        InıtilizeHelper();
    }

    private void InıtilizeHelper()
    {
        foreach (Transform child in transform)
        {
            if (PlayerPrefs.HasKey(child.name + "Active"))
            {
                child.gameObject.SetActive(true);
            }
        }
    }


    #region Registers
    
    public void RegisterNpc(Helper helper)
    {
        helperNpcList.Add(helper);
    }

    public void RegisterStackFoodPoint(StackTriggerHandler zone)
    {
        stackFoodPoints.Add(zone);
    }
    
    public void RegisterStackSouvenirPoint(StackTriggerHandler zone)
    {
        stackSouvenirPoints.Add(zone);
    }

    public void RegisterFoodZone(SupplyZoneHandler zone)
    {
        needFoodZones.Add(zone);
    }

    public void RegisterSouvenirZone(SupplyZoneHandler zone)
    {
        needSouvenirZones.Add(zone);
    }

    public void RegisterCleanZoneDict(int key, CleanerZoneHandler zone)
    {
        if (!needCleanZonesDict.ContainsKey(key))
        {
            var newList = new List<CleanerZoneHandler>();
            newList.Add(zone);
            needCleanZonesDict.Add(key,newList);
            return;
        }

        var newCleanList = needCleanZonesDict[key];
        newCleanList.Add(zone);

    }

    public void RegisterCleanerHelperIdlePoint(Transform point)
    {
        _cleanerHelperIdlePoint = point;
    }

    public void RegisterCarrierHelperIdlePoint(Transform point)
    {
        _carrierHelperIdlePoint = point;
    }
    
    #endregion

    #region Removers

    public void RemoveFoodZone(SupplyZoneHandler zone)
    {
        if(!needFoodZones.Contains(zone)) return;
        var smt = needFoodZones.Remove(zone);
    }

    public void RemoveSouvenirZone(SupplyZoneHandler zone)
    {
        if(!needFoodZones.Contains(zone)) return;
        needSouvenirZones.Remove(zone);
    }

    public void RemoveCleanZone(int key, CleanerZoneHandler zone)
    {
        needCleanZonesDict[key].Remove(zone);
    }

    #endregion
}
