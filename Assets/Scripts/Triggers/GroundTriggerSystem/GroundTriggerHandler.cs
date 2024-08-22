using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using TMPro;

public class GroundTriggerHandler : MonoBehaviour
{
    [SerializeField] public string id;
    
    [SerializeField] private MoneyHandler money;
    [SerializeField] private Slider slider;
    [SerializeField] private float speed;
    [SerializeField] private UnityEvent OnTriggerComplete;
    
    [SerializeField] private Image iconObj;
    [SerializeField] private Image upgradeIcon;
    
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text costText;
    
    [SerializeField] public TMP_Text remainderText;
    

    private float _pulse = 0.1f;
    private float _counter = 0;
    private bool _isProcessDone;
    private bool _quitting;
    private int _intRemainder;

    private Coroutine _newRoutine;
    private Tween _timeTween;
    private Tween _moveTween;
    private Transform _playerTransform;

    private float _moneyNeedForThisTrigger;
    public string Id => id;

    public Image UpgradeIcon => upgradeIcon;

    public void Initialize(float needMoneyVal, string idVal, Sprite newIconObj, SpriteState newUpgradeIcon, string nameTxt)
    {
        
        iconObj.sprite = newIconObj;
        UpgradeIcon.enabled = newUpgradeIcon != SpriteState.NEW;
        nameText.text = nameTxt;
        id = idVal;
        _moneyNeedForThisTrigger = needMoneyVal;
        
        _playerTransform = Player.Instance.transform;
        slider.maxValue = needMoneyVal;
        CurrencyManager.Instance.OnChange += CurrencyManager.Instance.SetMoneyText;
        slider.value = PlayerPrefsHandler.GetRoomUpdaterMoneyAmount(id);
        
        if (PlayerPrefs.HasKey(gameObject.name + "_counter"))
            _counter = PlayerPrefs.GetFloat(gameObject.name + "_counter");
        else
            _counter = 0;
        
        _intRemainder = Mathf.RoundToInt(needMoneyVal - _counter);
        remainderText.SetText(_intRemainder.ToString());
        
    }

    private void OnDisable()
    {
        if (_quitting) 
            return;
        
        if(PlayerPrefs.HasKey(gameObject.name + "_counter"))
            PlayerPrefs.DeleteKey(gameObject.name + "_counter");
        
    }

    private void OnApplicationQuit()
    {
        _quitting = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        slider.maxValue = _moneyNeedForThisTrigger;
        if (_newRoutine != null)
        {
            StopCoroutine(_newRoutine);
        }

        if (_isProcessDone)
        {
            return;
        }

        _isProcessDone = false;
        _newRoutine = StartCoroutine(TakeMoney());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ProcessExit();
        }
    }

    private void ProcessExit()
    {
        _timeTween?.Kill();
        _isProcessDone = false;
        if (_newRoutine != null)
        {
            StopCoroutine(_newRoutine);
        }

        PlayerPrefsHandler.SetRoomUpdateMoneyAmount(name, slider.value);
    }

    IEnumerator TakeMoney()
    {
        while (slider.value < _moneyNeedForThisTrigger)
        {
            if (CurrencyManager.Instance.moneyAmount < 0.01f) yield break;
            if (!Player.Instance.IsPlayerStop() || _isProcessDone)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            ProcessTake();

            yield return new WaitForSeconds(_pulse);
        }
    }

    public void SetID(string newID)
    {
        id = newID;
    }

    void ProcessTake()
    {
        #region FillImage

        speed = _moneyNeedForThisTrigger * 0.6f;

        var currentFillAmount = slider.value;
        var sliderMaxVal = slider.maxValue;
        var destination = slider.value + _pulse * speed;
        var removeAmount = _pulse * speed;

        if (CurrencyManager.Instance.moneyAmount < removeAmount)
        {
            removeAmount = CurrencyManager.Instance.moneyAmount;

            destination = slider.value + removeAmount;
        }

        if (sliderMaxVal - slider.value < removeAmount)
        {
            removeAmount = sliderMaxVal - slider.value;
            destination = slider.value + removeAmount;
        }

        if (_moneyNeedForThisTrigger - _counter > 0.01f)
        {
            _counter += removeAmount;
        
            CurrencyManager.Instance.DecreaseMoney(removeAmount);
            PlayerPrefs.SetFloat(gameObject.name + "_counter", _counter);
        
            _intRemainder = Mathf.RoundToInt(_moneyNeedForThisTrigger - _counter);
            remainderText.SetText(_intRemainder.ToString());

        }
        
        if (_moneyNeedForThisTrigger - _counter < 0.01f) // When Fill Image done
        {
            slider.value = 0f;
            _isProcessDone = true;
            CurrencyManager.Instance.moneyAmount = Mathf.RoundToInt(CurrencyManager.Instance.moneyAmount);
            CurrencyManager.Instance.OnChange += CurrencyManager.Instance.SetMoneyText;
            //CurrencyManager.Instance.SetMoney();

            OnTriggerComplete?.Invoke();

            return;
        }
        
        _timeTween?.Kill();
        _timeTween = DOVirtual.Float(currentFillAmount, destination, _pulse, (value) => { slider.value = value; })
            .OnComplete(() => { _timeTween?.Kill(); });

        #endregion

        #region GettingMoneyAnim

        var zonePos = this.transform.position;
        var moneyClone = Instantiate(money, _playerTransform);
        moneyClone.BoxCollider().enabled = false;

        _moveTween = DOVirtual.Float(0f, 1f, _pulse, (value) =>
        {
            var pTPosition = _playerTransform.position;
            pTPosition.y += 1f;
            moneyClone.transform.position = Vector3.Lerp(pTPosition, zonePos, value);
        }).OnComplete((() =>
        {
            Destroy(moneyClone.gameObject);
            _moveTween?.Kill();
        }));

        #endregion
    }
}

