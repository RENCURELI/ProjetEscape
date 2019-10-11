using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent), typeof(FiniteStateMachine))]
public class NPC : MonoBehaviour
{

    NavMeshAgent _navMeshAgent;
    FiniteStateMachine _finiteStateMachine;

    [SerializeField]
    PatrolPoints[] _patrolPoints;

    public void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _finiteStateMachine = GetComponent<FiniteStateMachine>();

    }

    public void Start()
    {

    }

    public void Update()
    {

    }

    public PatrolPoints[] PatrolPoint
    {
        get
        {
            return _patrolPoints;
        }
    }
}
