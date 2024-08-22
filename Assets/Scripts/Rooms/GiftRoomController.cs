using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine.Serialization;


public class GiftRoomController : MonoBehaviour
{
    public List<GameObject> activeGiftList;
    public List<GameObject> deactiveGiftList;
    public List<GameObject> allGiftList;
    public GameObject gift;
    public RoomStatus status;
    private int maxGiftNumber = 3;
    private int maxCustomerNumber = 3;
    [SerializeField] private List<Transform> customerPoints;
    [SerializeField] public SupplyZoneHandler supplyZone;

    public int MaxGiftNumber
    {
        get => maxGiftNumber;
        set => maxGiftNumber = value;
    }

    public List<Transform> GetGiftPoints()
    {
        return customerPoints;
    }

    private void OnEnable()
    {
        RoomsManager.Instance.giftRoomList.Add(this);
        
        if (!PlayerPrefs.HasKey(name + "_GiftRoomStatus"))
        {
            _GiftRoomAvailable();    
        }
    }

    private void Awake()
    {
        CallGifts();
        GetStatus();

    }
    
    private void Start()
    {
        CustomersManager.Instance.RegisterGiftRoomPoints(this);
        
    }
    
    private void _GiftRoomClaim()
    {
        ChangeStatus(RoomStatus.Claim);
    }
    
    private void _GiftRoomUnavailable()
    {
        ChangeStatus(RoomStatus.Unavailable);
        PlayerPrefs.SetString(name + "_GiftRoomStatus","Unavailable");

    }
    private void _GiftRoomAvailable()
    {
        ChangeStatus(RoomStatus.Available);
        PlayerPrefs.SetString(name + "_GiftRoomStatus","Available");

    }
    
    private void ChangeStatus(RoomStatus target)
    {
        status = target;
        switch (target)
        {
            case RoomStatus.Available:
                break;
            case RoomStatus.Claim:
                break;
            case RoomStatus.Unavailable:
                break;
            default:
                return;
        }
    }

    public bool IsClaimGift()
    {
        return activeGiftList.Count != 0;
    }
    
    public void GetFromGiftList()
    {
        if (activeGiftList.Count == 0) return;
        GameObject _gift;
        _gift = activeGiftList[0];
        activeGiftList.Remove(_gift);
        deactiveGiftList.Add(_gift);
        _gift.SetActive(false);
        if (GetGiftPoints().Count == 0 && !IsClaimGift())
            _GiftRoomUnavailable();
        PlayerPrefs.SetInt(_gift.name + "_Active", 0);
        supplyZone.gameObject.SetActive(true);
    }
    public void RefreshGifts()
    {
        var count = deactiveGiftList.Count;
        for (int i = 0; i < count; i++)
        {
            deactiveGiftList[0].SetActive(true);
            activeGiftList.Add(deactiveGiftList[0]);
            PlayerPrefs.SetInt(deactiveGiftList[0].name + "_Active", 1);
            deactiveGiftList.Remove(deactiveGiftList[0]);
        }
        _GiftRoomAvailable();
        supplyZone.gameObject.SetActive(false);
        
    }

    public void SouvenirRoomComplete()
    {
        if (GetGiftPoints().Count > 0 && IsClaimGift() && activeGiftList.Count >= maxCustomerNumber - customerPoints.Count)
        {
            _GiftRoomAvailable();
        }
        else
        {
            _GiftRoomUnavailable();

        }
        
    }

    public void GiftRoomClaim()
    {
        if (activeGiftList.Count == maxCustomerNumber - customerPoints.Count)
        {
            _GiftRoomClaim();
        }
    }

    private void CallGifts()
    {
        var flag = false;
        
        foreach (var g in allGiftList)
        {
            if (PlayerPrefs.HasKey(g.name + "_Active"))
            {
                flag = true;
                break;
            }
        }

        if (!flag)
        {
            foreach (var g in allGiftList)
            {
                PlayerPrefs.SetInt(g.name + "_Active", 1);  
                g.gameObject.SetActive(true);
                activeGiftList.Add(g);

            }
            supplyZone.gameObject.SetActive(false);
        }
        else
        {
            foreach (var g in allGiftList)
            {
                if (PlayerPrefs.GetInt(g.name + "_Active") == 1)
                {
                    g.gameObject.SetActive(true);
                    activeGiftList.Add(g);
                }
                else if(PlayerPrefs.GetInt(g.name + "_Active") == 0)
                {
                    g.gameObject.SetActive(false);
                    deactiveGiftList.Add(g);
                }
                supplyZone.gameObject.SetActive(true);

            }
        }
    }

    private void GetStatus()
    {
        if (!PlayerPrefs.HasKey(name + "_GiftRoomStatus")) return;
        switch (PlayerPrefs.GetString(name + "_GiftRoomStatus"))
        {
            case "Available":
                _GiftRoomAvailable();
                break;
            case "Unavailable":
                _GiftRoomUnavailable();
                break;
        }
    }

    public bool IsAvailableGiftRoom()
    {
        return status == RoomStatus.Available && RoomsManager.Instance.giftRoomList.Contains(this);
    }
}
