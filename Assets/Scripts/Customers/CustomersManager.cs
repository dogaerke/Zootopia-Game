
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class CustomersManager : MonoBehaviour
{
    [SerializeField] private CustomerNpc customerPrefab;
    [SerializeField] private List<Transform> inLinePointList;
    [SerializeField] private List<Transform> waitingPointList;
    [SerializeField] public int maxCustomerNum = 9; 
    [SerializeField] public Transform exitPoint;

    [SerializeField]private List<GiftRoomController> _giftRoomsController; 
    

    public List<CustomerNpc> customerWalkingList = new List<CustomerNpc>();
    public List<CustomerNpc> customerWaitList = new List<CustomerNpc>();
    public List<CustomerNpc> customerInLineList = new List<CustomerNpc>();
    public List<CustomerNpc> customerInRoomList = new List<CustomerNpc>();
    public List<CustomerNpc> customerInGiftRoomList = new List<CustomerNpc>();
    
    private List <Transform> _roomPointList;
    public static CustomersManager Instance { get; private set; }

    public List<GiftRoomController> GiftRoomsController => _giftRoomsController;

    public bool haveEmptyRoom;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);

        else
            Instance = this;
    }
    private void Start()
    {
        StartCoroutine(nameof(TimeofSpawn));
        haveEmptyRoom = true;
        
    }
    
    private IEnumerator TimeofSpawn()
    {
        while (true)
        {
            if(customerInLineList.Count + customerWaitList.Count + customerWalkingList.Count < maxCustomerNum)
                CreateCustomer();
            yield return new WaitForSeconds(5f);

        }
    }
    private void CreateCustomer()
    {
        var instance = Instantiate(customerPrefab, this.transform);

        if (instance.CheckCustomerIsReady() && customerInLineList.Count + customerWalkingList.Count < inLinePointList.Count)
        {
            instance.SetDestination(inLinePointList[customerInLineList.Count + customerWalkingList.Count]);
            customerWalkingList.Add(instance);
            instance.OnReach += AddInLine;
        }
        else if (instance.CheckCustomerIsReady() && customerInLineList.Count + customerWalkingList.Count >= inLinePointList.Count)
        {
            instance.SetDestination(waitingPointList[customerWaitList.Count]);
            customerWalkingList.Add(instance);
            instance.OnReach += FireOnInstantiate;
        }

    }
    private void AddInLine(CustomerNpc obj)
    {
        if (customerInLineList.Count < inLinePointList.Count)
        {
            customerInLineList.Add(obj);
        }
        if (customerWalkingList.Count > 0)
        {
            customerWalkingList.Remove(obj);
        }
        obj.OnReach -= AddInLine;
    }
    private void FireOnInstantiate(CustomerNpc obj)
    {
        customerWaitList.Add(obj);
        if (customerWalkingList.Count > 0)
            customerWalkingList.Remove(obj);
        obj.OnReach -= FireOnInstantiate;
    }
    public void WhenTriggerDone()
    {
        FireOnWelcome(customerInLineList[0]);
    }
    private void FireOnWelcome(CustomerNpc obj) //To leave from reception
    {
        RoomController room;
        haveEmptyRoom = RoomsManager.Instance.TryGetEmptyRoom(out room);
        if (haveEmptyRoom && customerInLineList.Count != 0 && room.status == RoomStatus.Available)
        {
            obj.ClaimRoom(room);
        }

    }
    public void ShiftCustomers()
    {
        var jvalue = 0;
        
        foreach (var c in customerInLineList)
        {
            c.SetDestination(inLinePointList[jvalue++].transform);
        }
        jvalue = 0;
        if (customerWaitList.Count > 0)
        {
            customerWaitList[0].SetDestination(inLinePointList[customerInLineList.Count].transform);
            customerInLineList.Add(customerWaitList[0]);
            customerWaitList.Remove(customerWaitList[0]);
            foreach (var c in customerWaitList)
            {
                c.SetDestination(waitingPointList[jvalue++]);
            }
        }

        if (customerWalkingList.Count <= 0) return;
        foreach (var customer in customerWalkingList)
        {
            if (customerInLineList.Count + customerWalkingList.Count < inLinePointList.Count)
            {
                customer.SetDestination(inLinePointList[customerInLineList.Count].transform);
                if (customerInLineList.Count != 4) continue;
                customerInLineList.Add(customer);
                customerWaitList.Remove(customer);
                jvalue = 0;
                foreach (var c in customerWaitList)
                {
                    c.SetDestination(waitingPointList[jvalue++]);
                }
            }
            else if (customerInLineList.Count == inLinePointList.Count)
                customer.SetDestination(waitingPointList[customerWaitList.Count].transform);
        }
    }

    public void RegisterGiftRoomPoints(GiftRoomController room)
    {
        GiftRoomsController.Add(room);
    }
}
