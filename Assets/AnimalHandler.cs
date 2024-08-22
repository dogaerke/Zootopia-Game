using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AnimalHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private List<Transform> walkingPoints;
    [SerializeField] private float waitingTime;
    [SerializeField] private float walkingTime = 5f;
    [SerializeField] private Transform lookAt;
    

    private Tween _newTweenMove;
    private Tween _newTweenLook;
    
    private static readonly int Walk = Animator.StringToHash("Walk");

    private void OnEnable()
    {
        Invoke(nameof(StartRoutine), 5f);
    }
    
    private void StartRoutine()
    {
        StartCoroutine(nameof(WalkRandomly));
    }
    private IEnumerator WalkRandomly()
    {
        int currentPos = 0;
        var pointsCount = walkingPoints.Count;
        while (true)
        {
            var val = Random.Range(0, 100);
            if (currentPos + 1 > pointsCount)
            {
                currentPos = 0;
            }
            if (val < 80)
            {
                animator.SetTrigger(Walk);
                _newTweenMove?.Kill();
                _newTweenLook?.Kill();
                _newTweenLook = transform.DOLookAt(walkingPoints[currentPos].position, .5f);
                _newTweenMove = transform.DOMove(walkingPoints[currentPos++].position, walkingTime).OnComplete(() =>
                {
                    animator.SetTrigger(Walk);
                    transform.DOLookAt(lookAt.position, 0.3f);
                });
                
                
            }
            
            yield return new WaitForSeconds(waitingTime);
        }

    }
}
