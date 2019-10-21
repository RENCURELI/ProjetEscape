using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encense : MonoBehaviour
{
    [HideInInspector]
    public int currentRendererId;
    public Renderer[] stickRenderers;
    public Renderer mainRenderer;

    [HideInInspector]
    public float burnTimer = 0;
    public const float burnDuration = 60;
    public float BurnFactor
    {
        get
        {
            return burnTimer / burnDuration;
        }
    }

    private MaterialPropertyBlock mpb;
    private int burntFactorPropId = Shader.PropertyToID("_BurntFactor");
    
    void Start()
    {
        currentRendererId = 0;
        burnTimer = burnDuration;
        mpb = new MaterialPropertyBlock();
        InitializeStateMachine();
    }

    private void OnDestroy()
    {
        stateMachine.Dispose();
    }

    public void NextStick()
    {
        currentRendererId++;
    }

    public void UpdateBurn()
    {
        mpb.SetFloat(burntFactorPropId, BurnFactor);
        stickRenderers[currentRendererId].SetPropertyBlock(mpb);
    }

    #region STATE_MACHINE

    #region MACHINE

    public StateMachine stateMachine;

    private void InitializeStateMachine()
    {
        stateMachine = new StateMachine("Encense", 1f);
        // States
        stateMachine.AddState("Idle");
        stateMachine.AddActionState("Burn");
        stateMachine.AddState("End");
        // Bindings
        stateMachine.BindState("Start", State_Start);
        stateMachine.BindState("Idle", State_Idle);
        stateMachine.BindState("Burn", State_Burn);
        // Transitions
        stateMachine.Connect("Start", "Idle", Transition_Always);
        stateMachine.Connect("Idle", "Burn", Transition_Always);
        stateMachine.Connect("Burn", "End", Transition_WhenDone);
        //
        stateMachine.Start();
    }

    #endregion MACHINE

    #region STATES

    private void State_Start(StateMachine machine, StateMachine.State state, float deltaTime)
    {
        burnTimer = burnDuration;
        currentRendererId = 0;
    }

    private void State_Idle(StateMachine machine, StateMachine.State state, float deltaTime)
    {

    }

    private void State_Burn(StateMachine machine, StateMachine.State state, float deltaTime)
    {
        if (!(state is StateMachine.ActionState)) throw new System.Exception("State is not of type Action.");
        burnTimer -= deltaTime;
        UpdateBurn();
        if (burnTimer <= 0)
        {
            burnTimer = burnDuration;
            NextStick();
            if (currentRendererId >= stickRenderers.Length)
            {
                var action = state as StateMachine.ActionState;
                action.done = true;
            }
        }
    }

    #endregion STATES

    #region TRANSITIONS

    private bool Transition_Always(StateMachine.State a, StateMachine.State b)
    {
        return true;
    }

    private bool Transition_WhenDone(StateMachine.State a, StateMachine.State b)
    {
        if (a is StateMachine.ActionState)
        {
            var actionState = (StateMachine.ActionState)a;
            if (actionState.done) return true;
        }
        else
        {
            throw new System.Exception("State is not of type: Action");
        }
        return false;
    }

    #endregion TRANSITIONS

    #endregion STATE_MACHINE
}
