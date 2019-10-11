using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PatrolState", menuName = "ProjetEscape/States/Patrol", order = 2)]
public class PatrolState : AbstractFSMClass
{
    PatrolPoints[] _patrolPoints;
    int _patrolPointIndex;


    public override void OnEnable()
    {
        base.OnEnable();
        _patrolPointIndex = -1;
        FSMStateType = FSMStateType.PATROL;
    }

    public override bool EnterState()
    {
        EnteredState = false;
        if (base.EnterState())
        {
            //Grab and store Patrol Points
            _patrolPoints = _npc.PatrolPoint;

            if (_patrolPoints == null || _patrolPoints.Length == 0)
            {
                Debug.Log("Patrol state failed");

            }
            else
            {
                if (_patrolPointIndex < 0)
                {
                    _patrolPointIndex = UnityEngine.Random.Range(0, _patrolPoints.Length);
                }
                else
                {
                    _patrolPointIndex = (_patrolPointIndex++) % _patrolPoints.Length;
                }

                SetDestination(_patrolPoints[_patrolPointIndex]);
                EnteredState = true;
            }
        }
        return EnteredState;
    }

    public override void UpdateState()
    {
        //TODO : Check successfull state entry
        if (EnteredState)
        {
            if (Vector3.Distance(_navMeshAgent.transform.position, _patrolPoints[_patrolPointIndex].transform.position) <= 1f)
            {
                _fsm.EnterState(FSMStateType.IDLE);
            }
        }
    }

    private void SetDestination(PatrolPoints destination)
    {
        if (_navMeshAgent != null && destination != null)
        {
            _navMeshAgent.SetDestination(destination.transform.position);
        }

    }
}
