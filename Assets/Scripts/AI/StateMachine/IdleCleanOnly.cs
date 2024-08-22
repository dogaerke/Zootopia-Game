using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleCleanOnly : State
{
    public IdleCleanOnly(Helper npc, NavMeshAgent agent, Animator anim)
        : base(npc, agent, anim)
    {
        Name = STATE.IDLE;
    }

    protected override void Enter()
    {
        npc.SetDestination(ClearHelperIdlePoint);
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
        
        if(HelperManager.Instance.NeedCleanZonesDict.ContainsKey(duityLevel))
            if (HelperManager.Instance.NeedCleanZonesDict[duityLevel].Count > 0)
            {
                var zone = HelperManager.Instance.NeedCleanZonesDict[duityLevel][0];
                nextState = new Clean(npc, agent, anim, zone);
                stage = EVENT.EXIT;
            }
    }

    protected override void Exit()
    {
        
        base.Exit();
    }
}
