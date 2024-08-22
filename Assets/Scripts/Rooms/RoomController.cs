using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomController : MonoBehaviour
{
    [SerializeField] private Transform customerPoint;
    [SerializeField] private List<CleanerZoneHandler> cleaningZones;
    //[SerializeField] private Transform[] foods;
    [SerializeField] private Animator[] anims;
    [SerializeField] public SupplyZoneHandler foodZone;
    public List<GameObject> roomVersionList;
    
    public GameObject[] cleaningMaterials;
    public bool isDirty;
    private int _foodIndex = 0;
    public List<GameObject> activeFoodList;
    public List<GameObject> deactiveFoodList;
    public List<GameObject> allFoodList;
    private int _maxFoodNumber = 3;
    public GiftRoomController relevantGiftRoom;
    public Transform CustomerPoint => customerPoint;
    public RoomStatus status;
    public int customerCounter;
    public GameObject fence;
    private static readonly int Throw = Animator.StringToHash("Throw");
    public int MaxFoodNumber
    {
        get => _maxFoodNumber;
        set => _maxFoodNumber = value;
    }

    private void Start()
    {
        customerCounter = 0;
        GetStatus();
        SaveFoods();
        
    }

    private void OnEnable()
    {
        if(!CompareTag("GiftRoom"))
            RoomsManager.Instance.roomList.Add(this);
        
    }
    
    public void RoomClaim()
    {
        ChangeStatus(RoomStatus.Claim);

    }
    public void RoomUnavailable()
    {
        ChangeStatus(RoomStatus.Unavailable);
    }
    
    public void RoomAvailable()
    {
        ChangeStatus(RoomStatus.Available);
        

    }
    private void ChangeStatus(RoomStatus target)
    {
        status = target;
        switch (target)
        {
            case RoomStatus.Available:
                PlayerPrefs.SetString(name + "_RoomStatus","Available");
                CleanRoom();
                break;
            
            case RoomStatus.Unavailable:
                PlayerPrefs.SetString(name + "_RoomStatus","Unavailable");
                switch (isDirty)
                {
                    case true:
                        DirtyRoom();
                        break;
                    case false:
                        CleanRoom();
                        break;
                }
                break;
                
            case RoomStatus.Claim:
                break;
            
            default:
                return;
        }
    }
    public void CustomerTourComplete()
    {
        customerCounter++;
        if(customerCounter > 3)
            customerCounter %= 3;
        switch (customerCounter)
        {
            case < 3:
                if (IsClaimFood())
                {
                    RoomAvailable();
                    break;
                }
                CleanRoom();
                break;
            
            case 3:
                isDirty = true;
                PlayerPrefs.SetInt(name + "Dirty", 1);
                RoomUnavailable();
                break;
        }
        
    }
    
    public bool AreZonesCleaned() //PLayerPrefse kaydet
    {
        var flag = false;
        foreach (var cleanerZone in cleaningZones)
        {
            if (cleanerZone.IsCleaned)
            {
                flag = true;
            }
            else
            {
                flag = false;
                break;
            }
        } return flag;
    } 
    private void DirtyRoom()
    {
        foreach (var cleanerZone in cleaningZones)
        {
            cleanerZone.IsCleaned = false;
        }

        foreach (var c in cleaningMaterials)
        {
            c.SetActive(true);
        }
        foreach (var zone in cleaningZones)
        {
            zone.gameObject.SetActive(true);
        }
    }
    private void CleanRoom()
    {
        foreach (var cleanerZone in cleaningZones)
        {
            cleanerZone.IsCleaned = true;
        }

        foreach (var c in cleaningMaterials)
        {
            c.SetActive(false);
        }
        foreach (var zone in cleaningZones)
        {
            zone.gameObject.SetActive(false);
        }
    }
    
    
    public void PlayFoodAnimation()
    {
        if (_foodIndex >= 3)
        {
            _foodIndex = 0;
        }
        
        anims[_foodIndex++].SetTrigger(Throw);
    }

    public void DecreaseFood()
    {
        if (activeFoodList.Count == 0) return;
        GameObject food;
        food = activeFoodList[0];
        activeFoodList.Remove(food);
        deactiveFoodList.Add(food);
        //_food.SetActive(false);
        if (!IsClaimFood())
            RoomUnavailable();
        //PlayerPrefs.SetInt(food.name + "_Active", 0);
        foodZone.gameObject.SetActive(true);
        
    }
    
    public void RefreshFoods()
    {
        var count = deactiveFoodList.Count;
        for (int i = 0; i < count; i++)
        {
            deactiveFoodList[0].SetActive(true);
            activeFoodList.Add(deactiveFoodList[0]);
            //PlayerPrefs.SetInt(deactiveFoodList[0].name + "_Active", 1);
            deactiveFoodList.Remove(deactiveFoodList[0]);
        }
        foodZone.gameObject.SetActive(false);
        if(!isDirty)
            RoomAvailable();

        
    }
    public bool IsClaimFood()
    {
        return activeFoodList.Count != 0;
    }
    private void GetStatus()
    {
        if (!PlayerPrefs.HasKey(name + "_RoomStatus"))
        {
            CleanRoom();
            RoomAvailable();
            return;
        }
        switch (PlayerPrefs.GetString(name + "_RoomStatus"))
        {
            case "Available":
                RoomAvailable();
                break;
            case "Unavailable":
                isDirty = PlayerPrefs.GetInt(name + "Dirty") == 1;
                RoomUnavailable();
                break;
        }
    }
     private void SaveFoods()
     {
         foreach (var f in allFoodList)
         {
             f.gameObject.SetActive(true);
             activeFoodList.Add(f);
         }
         RoomAvailable();
         if(foodZone)
            foodZone.gameObject.SetActive(false);
    //     var flag = false;
    //     
    //     foreach (var f in allFoodList)
    //     {
    //         if (PlayerPrefs.HasKey(f.name + "_Active"))
    //         {
    //             flag = true;
    //             break;
    //         }
    //     }
    //
    //     if (!flag)
    //     {
    //         foreach (var f in allFoodList)
    //         {
    //             PlayerPrefs.SetInt(f.name + "_Active", 1);  
    //             f.gameObject.SetActive(true);
    //             activeFoodList.Add(f);
    //
    //         }
    //     }
    //     else
    //     {
    //         foreach (var f in allFoodList)
    //         {
    //             if (PlayerPrefs.GetInt(f.name + "_Active") == 1)
    //             {
    //                 f.gameObject.SetActive(true);
    //                 activeFoodList.Add(f);
    //             }
    //             else if(PlayerPrefs.GetInt(f.name + "_Active") == 0)
    //             {
    //                 f.gameObject.SetActive(false);
    //                 deactiveFoodList.Add(f);
    //             }
    //
    //         }
    //     }
     }
    

}

