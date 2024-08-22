using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CleanerHelperAI : MonoBehaviour
{
    [SerializeField] private Helper npc;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;

    private State _currentState;
    // Start is called before the first frame update
    void Start()
    {
        _currentState = new IdleCleanOnly(npc, agent, anim);
    }

    // Update is called once per frame
    void Update()
    {
        _currentState = _currentState.Process();
    }
}
