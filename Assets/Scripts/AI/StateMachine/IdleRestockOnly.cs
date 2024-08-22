using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleRestockOnly : State
{
    public IdleRestockOnly(Helper npc, NavMeshAgent agent, Animator anim)
        : base(npc, agent, anim)
    {
        Name = STATE.IDLE;
    }

    protected override void Enter()
    {
        npc.SetDestination(CarrierHelperIdlePoint);
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

        if (HelperManager.Instance.NeedFoodZones().Count > 0) //Food Restocking
        {
            Debug.Log("needFOodZones");
            nextState = new Stack(npc, agent, anim, HelperManager.Instance.StackFoodPoints[npc.duityArea],
                HelperManager.Instance.NeedFoodZones()[0]);
            stage = EVENT.EXIT;
            return;
        }

        if (HelperManager.Instance.NeedSouvenirZones().Count > 0) //Souvenir Restocking
        {
            nextState = new Stack(npc, agent, anim, HelperManager.Instance.StackSouvenirPoints[npc.duityArea],
                HelperManager.Instance.NeedSouvenirZones()[0]);
            stage = EVENT.EXIT;
        }

        if (npc.HelperStackIndex > 0)
        {
            Debug.Log(npc.name);
            nextState = new Flush(npc, agent, anim);
            stage = EVENT.EXIT;
        }
    }

    protected override void Exit()
    {
        base.Exit();
    }
}

    

