using UnityEngine;
using UnityEngine.AI;

public class Stack : State
{
    public Stack(Helper npc, NavMeshAgent agent, Animator anim, StackTriggerHandler stackPoint, SupplyZoneHandler restockPoint)
        : base(npc, agent, anim, stackPoint, restockPoint)
    {
        Name = STATE.STACK;
    }

    protected override void Enter()
    {
        npc.SetDestination(stackPoint.transform);
        base.Enter();
    }

    protected override void Update()
    {
        if (agent.remainingDistance > 0.2f)
        {
            anim.SetFloat(Speed, 1f);
        }
        else
        {
            anim.SetFloat(Speed, 0f);
        }
        
        if (npc.HelperStackIndex > 0)
        {
            nextState = new Restock(npc, agent, anim, restockPoint);
            stage = EVENT.EXIT;
        }
        
    }

    protected override void Exit()
    {
        anim.SetBool(Carry, true);
        base.Exit();
    }
}
