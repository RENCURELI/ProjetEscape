﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "IdleState", menuName = "ProjetEscape/States/IDLE", order = 1)]
public class IdleState : AbstractFSMClass
{
    [SerializeField]
    const float _idleDuration = 3.0f;

    float _totalDuration;

    public override void OnEnable()
    {
        base.OnEnable();
        FSMStateType = FSMStateType.IDLE;
    }

    public override bool EnterState()
    {
        EnteredState = base.EnterState();
        Debug.Log("ENTERRED IDLE STATE");

        if (EnteredState)
        {
           _totalDuration = 0f;
        }
        EnteredState = true;
        return EnteredState;
    }

    public override void UpdateState()
    {

        if (EnteredState == true)
        {
            _totalDuration += Time.deltaTime;
            Debug.Log("UPDATING IDLE STATE");
            if (_totalDuration >= _idleDuration)
            {
                Debug.Log("EXITING IDLE STATE");
                _fsm.EnterState(FSMStateType.PATROL);
            }
        }
        
    }

    public override bool ExitState()
    {
        base.ExitState();
        Debug.Log("EXITED IDLE STATE");
        return true;
    }
}
