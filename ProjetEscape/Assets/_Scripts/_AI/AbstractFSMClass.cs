using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ExecutionState
{
    NONE,
    ACTIVE,
    COMPLETED,
    TERMINATED,
};

public enum FSMStateType
{
    NONE,
    PATROL,
    ATTACK,
};

public abstract class AbstractFSMClass : ScriptableObject
{

    protected FiniteStateMachine _fsm;

    public ExecutionState ExecutionState { get; protected set; }
    public bool EnteredState { get; protected set; }
    public FSMStateType FSMStateType { get; protected set; }

    /// <summary>
    /// Default setup : None active
    /// </summary>
    public virtual void OnEnabled()
    {
        ExecutionState = ExecutionState.NONE;
    }

    /// <summary>
    /// Check if state is entered without issue
    /// </summary>
    /// <returns></returns>
    public virtual bool EnterState()
    {
        //WIP

        return true;
    }


    /// <summary>
    /// Tells FSM to update current state
    /// </summary>
    public abstract void UpdateState();

    /// <summary>
    /// Check if state is exited without issue
    /// </summary>
    /// <returns></returns>
    public virtual bool ExitState()
    {
        ExecutionState = ExecutionState.COMPLETED;
        return true;
    }
}
