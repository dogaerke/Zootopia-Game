using System.Collections;
using System;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = System.Random;


public class CustomerNpc : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private MoneyHandler moneyPrefab;
    [SerializeField] private float speed = 300f;
    [SerializeField] private Animator animator;
    [SerializeField] private List<Material> clothes;

    
    public event Action<CustomerNpc> OnReach;
    public Transform giftPoint;
    public Transform stopPoint;
    public Transform target;
    private RoomController _myRoom;
    private Quaternion _idleRotation;
    private bool _canMove;
    private Vector3 _velocity;
    private GiftRoomController giftRoom;
    private int num;
    private static readonly int Carry = Animator.StringToHash("Carry");
    private static readonly int Speed = Animator.StringToHash("Speed");

    private void Awake()
    {
        _idleRotation = transform.rotation;
        navMeshAgent.updatePosition = false;
        GetClothesMaterial();
    }
    
    private void Update()
    {
        MoveAndRotate();
    }
    
    private void MoveAndRotate()
    {
        if (!target)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _idleRotation, Time.deltaTime * 5f);
            return;
        }

        Vector3 relativePos = target.position - transform.position;
        var targetRot = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * 25f);
        var myPos = transform.position; //Navmesh shaking fixed
        myPos.y = 0f;                           //
        var targetPos = target.position;    //
        targetPos.y = 0f;                           //Navmesh shaking fixed
        if (Vector3.Distance(myPos, targetPos) >= .2f)
        {
            animator.SetFloat(Speed, 1f);
            return;
        }
        
        animator.SetFloat(Speed, 0f);
        _idleRotation = target.rotation;
        target = null;
        _canMove = false;
        navMeshAgent.isStopped = true;
        OnReach?.Invoke(this);
    }

    private void FixedUpdate()
    {
        if (!target)
            return;
        if (_canMove)
        {
            transform.position = Vector3.SmoothDamp(transform.position, navMeshAgent.nextPosition, ref _velocity, .1f,
                speed, Time.fixedDeltaTime);
        }
    }

    public void SetDestination(Transform target)
    {
        this.target = target;
        navMeshAgent.SetDestination(target.position);
        _canMove = true;
        navMeshAgent.isStopped = false;
    }

    public bool CheckCustomerIsReady()
    {
        return navMeshAgent != null;
    }

    public void ClaimRoom(RoomController target)
    {
        var manager = CustomersManager.Instance;
        _myRoom = target;
        SetDestination(_myRoom.CustomerPoint);
        manager.customerInLineList.Remove(this);
        manager.customerInRoomList.Add(this);
        manager.ShiftCustomers();
        _myRoom.RoomClaim();
        
        giftRoom = _myRoom.relevantGiftRoom;
        OnReach += ReachForAnimalRoom;
        
    }

    private void ReachForAnimalRoom(CustomerNpc obj)
    {
        bool control = false;
        var manager = CustomersManager.Instance;
        OnReach -= ReachForAnimalRoom;
        StartCoroutine(WaitForSeconds(10f, () =>
        {
            num = GetRondomNum();
            _myRoom.PlayFoodAnimation();
            _myRoom.DecreaseFood();
            //decrease food
            if (num < 50)
            {
                if(giftRoom.IsAvailableGiftRoom())
                    FireToGiftRoom(out control);

            }
            if (num < 50 && control)
            {
                OnReach += ReachForGiftRoom;
                
            }
            else
            {
                manager.customerInRoomList.Remove(this);
                _myRoom.CustomerTourComplete();
                ReachForExitRoom();
            }

        }));
    }

    private void ReachForGiftRoom(CustomerNpc obj)
    {
        OnReach -= ReachForGiftRoom;

        StartCoroutine(WaitForSeconds(10f, () =>
        {
            giftRoom.GetFromGiftList();
            var newGift = Instantiate(giftRoom.gift, giftPoint);
            newGift.transform.localScale = Vector3.one * 3;
            animator.SetBool(Carry, true);
            GetMoneyInGiftRoom();
            
            CustomersManager.Instance.customerInGiftRoomList.Remove(this);
            giftRoom.SouvenirRoomComplete();
            giftRoom.GetGiftPoints().Add(stopPoint);
            ReachForExitRoom();

        }));
        
    }
    private void ReachForExitRoom()
    {
        var manager = CustomersManager.Instance;
        SetDestination(manager.exitPoint);
        OnReach += DestroyCustomer;
    }

    private void FireToGiftRoom(out bool control)
    {
        var manager = CustomersManager.Instance;
        var giftPoints = giftRoom.GetGiftPoints();
        if (giftRoom.status == RoomStatus.Available)
        {
            stopPoint = giftRoom.GetGiftPoints()[0];
            giftPoints.Remove(stopPoint);
            _myRoom.CustomerTourComplete();
            SetDestination(stopPoint);
            giftRoom.GiftRoomClaim();
    
            manager.customerInRoomList.Remove(this);
            manager.customerInGiftRoomList.Add(this);
            control = true;

        }
        else
        {
            control = false;
        }

    }
    
    

    private void DestroyCustomer(CustomerNpc npc)
    {
        Destroy(gameObject);
        OnReach -= DestroyCustomer;
    }

    private int GetRondomNum()
    {
        Random random = new Random();
        int randomValue = random.Next(0, 100);
        return randomValue;
    }

    private void GetMoneyInGiftRoom()
    {
        var moneyObj = Instantiate(moneyPrefab, transform.position + Vector3.up, Quaternion.identity);
        var rb = moneyObj.AddComponent<Rigidbody>();
        moneyObj.BoxCollider().isTrigger = false;
        rb.useGravity = true;

    }
    
    private IEnumerator WaitForSeconds(float t, Action onWaitEnd)
    {
        yield return new WaitForSeconds(t);
        onWaitEnd?.Invoke();
    }

    private void GetClothesMaterial()
    {
        var random = new Random(); 
        var index = random.Next(clothes.Count);
        //GetComponent<Renderer>().material = clothes[index];
        foreach(var rend in GetComponentsInChildren<Renderer>(true))
        {
            rend.material = clothes[index];
        }

    }
    
}
