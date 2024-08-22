using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class GetMoneyFromCustomer : MonoBehaviour
{
    [SerializeField] private MoneyHandler moneyPrefab;
    [SerializeField] private List<Transform> moneyPlacesList;
    [SerializeField] private List<MoneyHandler> moneyList;

    //private int _stackIndex = 0;
    private Tween _moveTween;
    private Transform _playerTransform;

    public static GetMoneyFromCustomer Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);

        else
            Instance = this;
    }

    private void Start()
    {
        _playerTransform = Player.Instance.transform;
    }

    public void ProcessCustomerGiveMoney()
    {
        if (moneyPlacesList.Count > moneyList.Count)
        {
            var moneyObj = Instantiate(moneyPrefab, moneyPlacesList[moneyList.Count].position, Quaternion.identity);
            moneyObj.BoxCollider().enabled = false;
            moneyList.Add(moneyObj);
        }
        else 
        {
            var moneyObj = Instantiate(moneyPrefab, moneyPlacesList[moneyPlacesList.Count].position, Quaternion.identity);
            moneyObj.BoxCollider().enabled = false;
            moneyList.Add(moneyObj);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerTakesMoney(other);
        }
    }

    private void PlayerTakesMoney(Collider other)
    {
        foreach (var money in moneyList.ToList())
        {
            money.MoneyGoesToPlayer(other);
            moneyList.Remove(money);
        }
    }
}
