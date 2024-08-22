using UnityEngine;
using UnityEngine.AI;

public class Clean : State
{
    public Clean(Helper npc, NavMeshAgent agent, Animator anim, CleanerZoneHandler zone)
        : base(npc, agent, anim, zone)
    {
        Name = STATE.CLEAN;
    }

    protected override void Enter()
    {
        npc.SetDestination(cleanPoint.transform);
        base.Enter();
    }

    protected override void Update()
    {
        //anim.SetFloat(Speed, agent.remainingDistance > .2f ? 0f : 1f);
        if (agent.remainingDistance > 0.2f)
        {
            anim.SetFloat(Speed, 1f);
        }
        else
        {
            anim.SetFloat(Speed, 0f);
        }
        
        if (cleanPoint.IsCleaned)
        {
            nextState = new IdleCleanOnly(npc, agent, anim);
            stage = EVENT.EXIT;
        }
    }

    protected override void Exit()
    {
        //Clean anim reset
        base.Exit();
    }
}
