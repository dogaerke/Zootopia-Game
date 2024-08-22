using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarrierHelperAI : MonoBehaviour
{
    [SerializeField] private Helper npc;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;


    [SerializeField] private State _currentState;
    
    void Start()
    {
        _currentState = new IdleRestockOnly(npc, agent, anim);
    }
    
    void Update()
    {
        _currentState = _currentState.Process();
    }
}