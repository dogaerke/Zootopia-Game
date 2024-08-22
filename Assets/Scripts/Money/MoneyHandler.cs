using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class MoneyHandler : MonoBehaviour
{
    [SerializeField] float collectTime = 0.5f;
    [SerializeField] float moneyValue = 10f;
    
    private BoxCollider _boxCollider;
    private Tween _moveTween;
    private Sequence _moneyTakingSequence;
    private Sequence _moneyScalingSequence;
    
    public BoxCollider BoxCollider() { return _boxCollider; }

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        CurrencyManager.Instance.OnChange += CurrencyManager.Instance.SetMoneyText;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Collector"))
        {
            MoneyGoesToPlayer(other);
        }
    }

    public void MoneyGoesToPlayer(Collider other)
    {
        CurrencyManager.Instance.IncreaseMoney(moneyValue);
        _boxCollider.enabled = false;
        var beginPos = transform.position;
        
        _moveTween = DOVirtual.Float(0, 1, collectTime, value =>
        {
            var targetPos = other.transform.position;
            transform.position = Vector3.Lerp(beginPos, targetPos, value);
        }).OnComplete(() => { Destroy(gameObject); });
    }
}
