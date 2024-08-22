using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using HelperStatusNamespace;
using ZoneStatusNamespace;

public class StackTriggerHandler : MonoBehaviour
{
    [SerializeField] [Range(0.01f, 0.1f)] private float _speed = 0.01f;
    [SerializeField] private float _pulse = 0.1f;
    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject boxPrefab;

    private Tween _timeTween;
    private Tween _moveTween;
    private Coroutine _newRoutine;
    private List<Transform> _stackList;
    private Transform _boxParent;
    private Helper _helper = null;
    private int _stackIndex = 0;
    private bool _isProcessStarted = false;

    private void Awake()
    {
        switch (boxPrefab.tag)
        {
            case "SupplyBox":
                HelperManager.Instance.RegisterStackSouvenirPoint(this);
                break;
            case "FoodBox":
                HelperManager.Instance.RegisterStackFoodPoint(this);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isProcessStarted) { return; }
        
        if (other.CompareTag("Player"))
        {
            _isProcessStarted = true;
            _stackList = Player.Instance.stackPoints;
            _stackIndex = Player.Instance.stackIndex;
            _boxParent = Player.Instance.boxParent;
            _newRoutine = StartCoroutine(ProcessFill());
        }

        if (other.CompareTag("Helper"))
        {
            _isProcessStarted = true;
            _helper = other.GetComponent<Helper>();
            _stackList = other.GetComponent<HelperBoxPositions>().GetBoxList();
            _stackIndex = _helper.HelperStackIndex;
            _boxParent = _helper.GetBoxParent();
            _newRoutine = StartCoroutine(ProcessFill());
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
        _isProcessStarted = false;
        _stackList = null;
        _boxParent = null;
        _helper = null;
        _stackIndex = 0;
    }

    IEnumerator ProcessFill()
    {
        while (_stackIndex < 5)
        {
            var currentFillAmount = fillImage.fillAmount;
            var destination = currentFillAmount;

            destination = Mathf.Clamp(destination + _speed, 0f, 1f);

            _timeTween?.Kill();
            _timeTween = DOVirtual.Float(currentFillAmount, destination, _pulse, (value) =>
            {
                fillImage.fillAmount = value;
            }); 
            
            if (destination >= 1f) //When filling complete
            {
                ProcessStack();
            }
            
            yield return new WaitForSeconds(_pulse);
        }
    }    
    private void ProcessStack()
    {
        var box = Instantiate(boxPrefab, _helper ? _boxParent : Player.Instance.boxParent);

        var currentPos = box.transform.position;
        _moveTween?.Kill();
        _moveTween = DOVirtual.Float(0f, 1f, _pulse, (value) =>
        {
            var destinationPos = _stackList[_stackIndex].transform.position;
            box.transform.position = Vector3.Lerp(currentPos, destinationPos, value);
            box.transform.DOLocalRotate(Vector3.zero, _pulse);
        }).OnComplete(() =>
        {
            if (_stackIndex + 1 < _stackList.Count)
                _stackIndex++;
            fillImage.fillAmount = 0f;
            _moveTween.Kill();

            if (!_helper)
            {
                Player.Instance.stackIndex = _stackIndex;
                
            }
            else
            {
                _helper.HelperStackIndex = _stackIndex;
            }
            
            
        });
        
    }
}
