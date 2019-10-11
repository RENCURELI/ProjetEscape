using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FiniteStateMachine : MonoBehaviour
{

    [SerializeField]
    AbstractFSMClass _startingState;
    AbstractFSMClass _currentState;

    [SerializeField]
    List<AbstractFSMClass> _validStates;

    Dictionary<FSMStateType, AbstractFSMClass> _fsmStates;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
