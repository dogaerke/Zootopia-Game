using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HelperStatusNamespace;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Helper : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform boxParent;
    [SerializeField] private List<State> helperStates;
    [SerializeField] private int helperStackIndex;
    [SerializeField] private Flusher flusher;

    private int _helperStackIndex;
    private StackTriggerHandler _stackZone;
    private Quaternion _idleRotation;
    private Coroutine _newRoutine;
    
    public Transform destinationPoint;
    public Transform idlePoint;
    public int duityArea;

    public int HelperStackIndex
    {
        get => helperStackIndex;
        set => helperStackIndex = value;
    }

    public Flusher GetFlusher => flusher;

    public int maxCarryAmount = 2; // playerprefs'den cekilecek.
    
    protected void Awake()
    {
        _idleRotation = transform.rotation;
    }
    
    protected void OnEnable()
    {
        HelperManager.Instance.RegisterNpc(this);
    }
    
    public Transform GetBoxParent()
    {
        return boxParent;
    }

    public void SetDestination(Transform target)
    {
        destinationPoint = target;
        navMeshAgent.SetDestination(target.position);
        navMeshAgent.isStopped = false;
    }
}
