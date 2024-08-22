using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Restock : State
{
    public Restock(Helper npc, NavMeshAgent agent, Animator anim, SupplyZoneHandler restockPoint)
        : base(npc, agent, anim, restockPoint)
    {
        Name = STATE.RESTOCK;
    }

    protected override void Enter()
    {
        npc.SetDestination(restockPoint.transform);
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
        
        if (!restockPoint.gameObject.activeInHierarchy || npc.HelperStackIndex == 0)
        {
            nextState = new IdleRestockOnly(npc, agent, anim);
            stage = EVENT.EXIT;
        }
    }

    protected override void Exit()
    {
        anim.SetBool(Carry, false);
        base.Exit();
    }
}
