using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Linq;
using System.Text;
using DefaultNamespace;
using UnityEngine.Serialization;

public class ReceptionTrigger : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] [Range(0.01f, 0.1f)] private float speed = 0.01f;
    [SerializeField] private float pulse = 0.1f;
    [SerializeField] private MoneyHandler money;

    private bool hasReceptionist;
    private Tween _timeTween;
    private Coroutine _newRoutine;
    
    // private RoomStatus _roomStatus;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Receptionist"))
        {
            _newRoutine = StartCoroutine(ProcessFill());
            hasReceptionist = true;
        }
        if (other.CompareTag("Player") && !hasReceptionist)
        {
            _newRoutine = StartCoroutine(ProcessFill());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !hasReceptionist)
        {
            ProcessExit();
        }
    }

    private IEnumerator ProcessFill()
    {
        bool haveEmptyRoom;
        int inLineCount;

        do
        {
            var currentFillAmount = fillImage.fillAmount;
            var destination = currentFillAmount;

            haveEmptyRoom = RoomsManager.Instance.TryGetEmptyRoom(out var room);
            inLineCount = CustomersManager.Instance.customerInLineList.Count;

            if (haveEmptyRoom && inLineCount != 0 && room.status == RoomStatus.Available)
            {
                destination = Mathf.Clamp(destination + speed, 0f, 1f);

                _timeTween?.Kill();
                _timeTween = DOVirtual.Float(currentFillAmount, destination, pulse,
                    (value) => { fillImage.fillAmount = value; });

                if (destination >= 1f) //When filling complete
                {
                    CustomersManager.Instance.WhenTriggerDone();
                    GetMoneyFromCustomer.Instance.ProcessCustomerGiveMoney();
                    fillImage.fillAmount = 0f;
                    _timeTween.Kill();
                }
            }

            yield return new WaitForSeconds(pulse);
        } while (CustomersManager.Instance.customerInLineList.Count + CustomersManager.Instance.customerWalkingList.Count 
                + CustomersManager.Instance.customerWaitList.Count < CustomersManager.Instance.maxCustomerNum + 1);
    }

    private void ProcessExit()
    {
        if (_newRoutine != null)
            StopCoroutine(_newRoutine);
        fillImage.fillAmount = 0;
        _timeTween?.Kill();
    }
}