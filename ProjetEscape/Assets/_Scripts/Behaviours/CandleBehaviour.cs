using Game.RenderPipelines;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(CandleSensor))]
public class CandleBehaviour : MonoBehaviour
{
    private CandleSensor candleSensor;
    //public LightSource lightSource;

    private void Start()
    {
        candleSensor = GetComponent<CandleSensor>();
        //lightSource = GetComponentInChildren<LightSource>();
        InitializeStateMachine();
    }

    private void OnDestroy()
    {
        stateMachine.Dispose();
    }

    #region STATE_MACHINE

    #region MACHINE

    public StateMachine stateMachine;

    private void InitializeStateMachine()
    {
        stateMachine = new StateMachine("Spirit Candle", 1f);
        // States
        stateMachine.AddState("Idle");
        stateMachine.AddActionState("Light");
        stateMachine.AddActionState("Dim");
        // Bindings
        stateMachine.BindState("Idle", State_Idle);
        stateMachine.BindState("Start", State_Start);
        stateMachine.BindState("Light", State_Light);
        stateMachine.BindState("Dim", State_Dim);
        // Transitions
        stateMachine.Connect("Start", "Idle", Transition_Always);
        stateMachine.Connect("Idle", "Light", Transition_Light);
        stateMachine.Connect("Idle", "Dim", Transition_Dim);
        stateMachine.Connect("Light", "Idle", Transition_WhenDone);
        stateMachine.Connect("Dim", "Idle", Transition_WhenDone);
        //
        stateMachine.Start();
    }

    #endregion MACHINE

    #region STATES

    private void State_Start(StateMachine machine, StateMachine.State state, float deltaTime)
    {
        candleSensor.SetLitFactor(0);
    }

    private void State_Light(StateMachine machine, StateMachine.State state, float deltaTime)
    {
        if (!(state is StateMachine.ActionState)) throw new System.Exception("State is not of type: ActionState");
        candleSensor.triggered = true;
        machine.updateTime = 0.01f;
        float speed = 1.0f / CandleSensor.fadeTime;
        candleSensor.SetLitFactor(candleSensor.litFactor + speed * machine.updateTime);
        if (candleSensor.litFactor >= 1)
        {
            var action = (StateMachine.ActionState)state;
            candleSensor.litFactor = 1;
            action.done = true;
        }
        
    }

    private void State_Dim(StateMachine machine, StateMachine.State state, float deltaTime)
    {
        if (!(state is StateMachine.ActionState)) throw new System.Exception("State is not of type: ActionState");
        candleSensor.triggered = false;
        machine.updateTime = 0.01f;
        float speed = 1.0f / CandleSensor.fadeTime;
        candleSensor.SetLitFactor(candleSensor.litFactor - speed * machine.updateTime);
        if (candleSensor.litFactor <= 0)
        {
            var action = (StateMachine.ActionState)state;
            candleSensor.litFactor = 0;
            action.done = true;
        }
        
    }

    private void State_Idle(StateMachine machine, StateMachine.State state, float deltaTime)
    {
        machine.updateTime = 1f;
    }

    private void State_End(StateMachine machine, StateMachine.State state, float deltaTime)
    {

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

    private bool Transition_TriggerInRadius(StateMachine.State a, StateMachine.State b)
    {
        Vector3 candlePosition = transform.position;
        foreach (CandleTrigger trigger in CandleTrigger.all)
        {
            if (!trigger) continue;
            Vector3 triggerPosition = trigger.transform.position;
            float distance = Vector3.Distance(candlePosition, triggerPosition);
            if (distance < candleSensor.radius)
            {
                return true;
            }
        }
        return false;
    }

    private bool Transition_NoTriggerInRadius(StateMachine.State a, StateMachine.State b)
    {
        return !Transition_TriggerInRadius(a,b);
    }

    private bool Transition_Light(StateMachine.State a, StateMachine.State b)
    {
        if (candleSensor.triggered) return false;
        else return Transition_TriggerInRadius(a, b);
    }

    private bool Transition_Dim(StateMachine.State a, StateMachine.State b)
    {
        if (!candleSensor.triggered) return false;
        else return Transition_NoTriggerInRadius(a, b);
    }

    #endregion TRANSITIONS

    #endregion STATE_MACHINE
}
