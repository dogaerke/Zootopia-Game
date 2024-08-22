using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class State
{
    public enum STATE
    {
        IDLE, STACK, RESTOCK, CLEAN, FLUSH
    }
    
    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    }

    public STATE Name;
    protected EVENT stage;
    protected Helper npc;
    protected NavMeshAgent agent;
    protected Animator anim;
    protected StackTriggerHandler stackPoint;
    protected SupplyZoneHandler restockPoint;
    protected CleanerZoneHandler cleanPoint;
    protected readonly Transform ClearHelperIdlePoint;
    protected readonly Transform CarrierHelperIdlePoint;
    protected State nextState;
    protected int duityLevel;
    
    protected static readonly int Speed = Animator.StringToHash("Speed");
    protected static readonly int Carry = Animator.StringToHash("Carry");

    public State(Helper npc, NavMeshAgent agent, Animator anim)//Idle State
    {
        this.npc = npc;
        this.agent = agent;
        this.anim = anim;
        this.stage = EVENT.ENTER;
        ClearHelperIdlePoint = npc.idlePoint;
        CarrierHelperIdlePoint = npc.idlePoint;
        duityLevel = npc.duityArea;
    }

    public State(Helper npc, NavMeshAgent agent, Animator anim, CleanerZoneHandler cleanPoint)//Clean State
    {
        this.npc = npc;
        this.agent = agent;
        this.anim = anim;
        this.stage = EVENT.ENTER;
        this.cleanPoint = cleanPoint;
        ClearHelperIdlePoint = npc.idlePoint;
    }
    
    public State(Helper npc, NavMeshAgent agent, Animator anim, StackTriggerHandler stackPoint, SupplyZoneHandler restockPoint)//Stack State
    {
        this.npc = npc;
        this.agent = agent;
        this.anim = anim;
        this.stage = EVENT.ENTER;
        this.stackPoint = stackPoint;
        this.restockPoint = restockPoint;
        CarrierHelperIdlePoint = npc.idlePoint;
    }
    
    public State(Helper npc, NavMeshAgent agent, Animator anim, SupplyZoneHandler restockPoint)//Restock State
    {
        this.npc = npc;
        this.agent = agent;
        this.anim = anim;
        this.stage = EVENT.ENTER;
        this.restockPoint = restockPoint;  
        CarrierHelperIdlePoint = npc.idlePoint;
    }
    
    public State(Helper npc, NavMeshAgent agent, Animator anim, Flusher flusher)//Restock State
    {
        this.npc = npc;
        this.agent = agent;
        this.anim = anim;
        this.stage = EVENT.ENTER;
        CarrierHelperIdlePoint = npc.idlePoint;

    }

    protected virtual void Enter()
    {
        //anim.SetFloat(Speed, 1);
        stage = EVENT.UPDATE;
    }

    protected virtual void Update()
    {
        //anim.SetFloat(Speed, agent.speed);
        stage = EVENT.UPDATE; 
    }

    protected virtual void Exit(){ stage = EVENT.EXIT; }

    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage != EVENT.EXIT) return this;

        Exit();
        return nextState;
    }
}