using UnityEngine;
using UnityEngine.AI;

public class Flush : State
{
    public Flush(Helper npc, NavMeshAgent agent, Animator anim)
        : base(npc, agent, anim)
    {
        Name = STATE.FLUSH;
    }

    protected override void Enter()
    {
        npc.SetDestination(npc.GetFlusher.transform);
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

        if (npc.HelperStackIndex == 0)
        {
            nextState = new IdleCleanOnly(npc, agent, anim);
            stage = EVENT.EXIT;
        }
    }

    protected override void Exit()
    {
        anim.SetBool(Carry, false);
        base.Exit();
    }
}
