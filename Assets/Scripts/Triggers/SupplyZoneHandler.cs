using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using ZoneStatusNamespace;
using UnityEngine.UI;

public class SupplyZoneHandler : MonoBehaviour
{
    [SerializeField] [Range(0.01f, 0.1f)] private float speed;
    [SerializeField] private Image fillImage;
    //[SerializeField] private ZoneStatus zoneStatus;
    [SerializeField] private GameObject zoneElements;
    public GiftRoomController souvenirRoom;
    public RoomController myRoom;

    private Tween _timeTween;
    private Tween _moveTween;
    private Coroutine _newRoutine;
    private Transform _boxParent;
    private Transform _supplyBox;
    private int _stackIndex = 0;
    private int _stackCounter = 0;
    private float _completedTime;
    private float _pulse = 0.1f;
    private string _objectTag;

    private void Awake()
    {
        switch (tag)
        {
            case "SupplyZone":
                _objectTag = "SupplyBox";
                break;
            case "FoodZone":
                _objectTag = "FoodBox";
                break;
        }
        
    }

    private void OnEnable()
    {
        switch (tag)
        {
            case "SupplyZone":
                HelperManager.Instance.RegisterSouvenirZone(this);
                break;
            case "FoodZone":
                HelperManager.Instance.RegisterFoodZone(this);
                break;
        }
    }

    private void Start()
    {
        if(CompareTag("SupplyZone")&& souvenirRoom.activeGiftList.Count == souvenirRoom.MaxGiftNumber)
            gameObject.SetActive(false);
        
        if (CompareTag("FoodZone") && myRoom.activeFoodList.Count == myRoom.MaxFoodNumber)
        {
            gameObject.SetActive(false);
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _boxParent = Player.Instance.boxParent;
            _stackIndex = Player.Instance.stackIndex;
            _newRoutine = StartCoroutine(ProcessFill(null));
        }
        
        else if (other.CompareTag("Helper"))
        {
            var helper = other.GetComponent<Helper>();
            _boxParent = helper.GetBoxParent();
            _stackIndex = helper.HelperStackIndex;
            _newRoutine = StartCoroutine(ProcessFill(helper));
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Helper"))
        {
            ProcessExit();
        }
    }
    
    private void ProcessExit()
    {
        if(_newRoutine != null)
            StopCoroutine(_newRoutine);
        
        fillImage.fillAmount = 0f;
        
        _timeTween?.Kill();
        
        _boxParent = null;
        _stackIndex = 0;
    }

    private IEnumerator ProcessFill(Helper helper)
    {
        while (gameObject.activeInHierarchy)
        {
            if (_stackIndex == 0)
            {
                yield break;
            }

            var isExist = false;
            _stackCounter = 0;
            foreach (Transform box in _boxParent)
            {
                _stackCounter++;
                if (box.CompareTag(_objectTag))
                {
                    isExist = true;
                    _supplyBox = box;
                    break;
                }
            }
            if (!isExist)
            {
                yield break;
            }
            var currentFillAmount = fillImage.fillAmount;
            var destination = currentFillAmount;

            destination = Mathf.Clamp(destination + speed, 0f, 1f);

            _timeTween?.Kill();
            _timeTween = DOVirtual.Float(currentFillAmount, destination, _pulse, (value) =>
            {
                fillImage.fillAmount = value;
            }); 
            
            if (destination >= 1f)
            {
                if (CompareTag("SupplyZone") && souvenirRoom.activeGiftList.Count < souvenirRoom.MaxGiftNumber)
                {
                    souvenirRoom.RefreshGifts();
                }
                if (CompareTag("FoodZone") && myRoom.activeFoodList.Count < myRoom.MaxFoodNumber)
                {
                    myRoom.RefreshFoods();
                }
                if (helper)
                    ProcessResupply(helper);
                else
                    ProcessResupply();
            }
            
            yield return new WaitForSeconds(_pulse);
        }
    }

    private void ProcessResupply()
    {
        _moveTween?.Kill();
        var destinationPos = transform.position;
        
        _moveTween = DOVirtual.Float(0f, 1f, _pulse, (value) =>
        {
            var currentPos = _supplyBox.transform.position;
            _supplyBox.transform.position = Vector3.Lerp(currentPos, destinationPos, value);
        }).OnComplete(() =>
        {
            if ( 0 < _stackIndex) { _stackIndex--; }
            
            _moveTween.Kill();
            
            Destroy(_supplyBox.gameObject);
            
            Player.Instance.ShiftBoxes(_stackCounter);
            Player.Instance.stackIndex = _stackIndex;
            
            fillImage.fillAmount = 0f;
            if (CompareTag("SupplyZone") && souvenirRoom.activeGiftList.Count == souvenirRoom.MaxGiftNumber)
            {
                OnDisableElements();
                return;
            }
            
            if(CompareTag("FoodZone") && myRoom.activeFoodList.Count == myRoom.MaxFoodNumber)
            {
                OnDisableElements();
                return;
            }
            

        });
    }
    
    private void ProcessResupply(Helper helper)
    {
        _moveTween?.Kill();
        var destinationPos = transform.position;
        
        _moveTween = DOVirtual.Float(0f, 1f, _pulse, (value) =>
        {
            var currentPos = _supplyBox.transform.position;
            _supplyBox.transform.position = Vector3.Lerp(currentPos, destinationPos, value);
        }).OnComplete(() =>
        {
            if ( 0 < _stackIndex) { _stackIndex--; }
            
            _moveTween.Kill();
            
            helper.HelperStackIndex = _stackIndex;
            fillImage.fillAmount = 0f;
            gameObject.SetActive(false);
            
            Destroy(_supplyBox.gameObject);
            OnDisableElements();

        });
    }
    
    private void OnDisableElements()
    {
        switch (tag)
        {
            case "SupplyZone":
                HelperManager.Instance.RemoveSouvenirZone(this);
                break;
            case "FoodZone":
                HelperManager.Instance.RemoveFoodZone(this);
                break;
        }
    }
}
