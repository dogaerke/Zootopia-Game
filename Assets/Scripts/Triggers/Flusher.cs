using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Flusher : MonoBehaviour
{
    [SerializeField] private float _waitThisSeconds = 1f;
    [SerializeField] private Transform trashCan;
    
    private bool _isProcessDone;
    private Tween _moveTween;
    private Coroutine _newRoutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Player.Instance.stackIndex == 0)
                return;
            
            _newRoutine = StartCoroutine(ProcessFlush());
        }
        else if (other.CompareTag("Helper"))
        {
            var helper = other.GetComponent<Helper>();
            _newRoutine = StartCoroutine(ProcessFlush(helper));
        }
    }

    private IEnumerator ProcessFlush()//For Player
    {
        while (Player.Instance.stackIndex > 0)
        {
            var topBoxIndex = Player.Instance.boxParent.GetChild(Player.Instance.stackIndex - 1 );
            Player.Instance.stackIndex--;

            _moveTween?.Kill();
            topBoxIndex.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), _waitThisSeconds);
            _moveTween = DOVirtual.Float(0f, 1f, _waitThisSeconds, (value) =>
            {
                var currentPos = topBoxIndex.transform.position;
                topBoxIndex.transform.position = Vector3.Lerp(currentPos, trashCan.position, value);
                
            }).OnComplete(() =>
            {
                Destroy(topBoxIndex.gameObject);
            });
            
            yield return new WaitForSeconds(_waitThisSeconds);
        }
    }

    private IEnumerator ProcessFlush(Helper helper)//For Helpers
    {
        while (helper.HelperStackIndex > 0)
        {
            var topBoxIndex = helper.GetBoxParent().GetChild(helper.HelperStackIndex - 1);//Player.Instance.boxParent.GetChild(Player.Instance.stackIndex - 1 );
            var destinationPos = transform.position;

            _moveTween?.Kill();
            topBoxIndex.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), _waitThisSeconds);
            _moveTween = DOVirtual.Float(0f, 1f, _waitThisSeconds, (value) =>
            {
                var currentPos = topBoxIndex.transform.position;
                topBoxIndex.transform.position = Vector3.Lerp(currentPos, destinationPos, value);
                
            }).OnComplete(() =>
            {
                helper.HelperStackIndex--;
                Destroy(topBoxIndex.gameObject);
            });
            
            yield return new WaitForSeconds(_waitThisSeconds);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(_newRoutine != null)
            StopCoroutine(_newRoutine);
    }
}
