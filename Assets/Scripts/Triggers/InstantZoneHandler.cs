using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class InstantZoneHandler : MonoBehaviour
{
    [SerializeField] private float _completeTime;
    [SerializeField] private float _completeSpeed;
    [SerializeField] private Image fillImage;
    
    private Tween _timeTween;

    public float CompleteSpeed { get => _completeSpeed; }

    public void SetCompleteTime(float val)
    {
        _completeTime = val;
    }

    private void Awake()
    {
        FindObjectOfType<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ProcessFill();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            ProcessExit();
        }
    }

    void ProcessFill()
    {
        _timeTween?.Kill();

        _completeSpeed = (1f - _completeTime) * _completeSpeed;
        var currentFillAmount = fillImage.fillAmount;

        _timeTween = DOVirtual.Float(currentFillAmount, 1f, CompleteSpeed, (value) =>
        {
            fillImage.fillAmount = value;
            SetCompleteTime(value);
        }).OnComplete(() =>
            {
                SetCompleteTime(0f);
                fillImage.fillAmount = 0f;

                CustomersManager.Instance.WhenTriggerDone();

            });
    }    

    private void ProcessExit()
    {
        _timeTween?.Kill();
    }


}
