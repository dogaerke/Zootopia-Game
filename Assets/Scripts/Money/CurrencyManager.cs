using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Events;


public class CurrencyManager : MonoBehaviour
{
    [SerializeField] public float moneyAmount;
    [SerializeField] public TMP_Text moneyText;

    public event Action<float>OnChange;

    public static CurrencyManager Instance { get; private set; }
    
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
        moneyAmount = PlayerPrefsHandler.GetPlayerMoney();
        SetMoneyText(moneyAmount);

    }
    // Start is called before the first frame update
    
    public void IncreaseMoney(float moneyValue)
    {
        var playerMoneyKey = "PlayerMoney";
        
        moneyAmount += moneyValue;
        //moneyAmount = Mathf.RoundToInt(moneyAmount);
        PlayerPrefs.SetFloat(playerMoneyKey, moneyAmount);
        OnChange?.Invoke(moneyValue);
    }
    
    public void DecreaseMoney(float moneyValue)
    {
        var playerMoneyKey = "PlayerMoney";
        if (Math.Abs(moneyAmount - moneyValue) < 0.01f)
            moneyAmount = 0;
        else
            moneyAmount -= moneyValue;

        PlayerPrefs.SetFloat(playerMoneyKey, moneyAmount);
        OnChange?.Invoke(moneyValue);
    }
    
    public void SetMoneyText(float m)
    {
        var money = Mathf.RoundToInt(moneyAmount);
        moneyText.SetText(money.ToString());
    }
    // public void SetMoney()
    // {
    //     var money = Mathf.RoundToInt(moneyAmount);
    //     moneyText.SetText(money.ToString());
    // }
}
