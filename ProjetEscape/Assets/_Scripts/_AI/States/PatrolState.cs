using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PatrolState", menuName = "ProjetEscape/States/Patrol", order = 2)]
public class PatrolState : AbstractFSMClass
{
    PatrolPoints[] _patrolPoints;
    int _patrolPointIndex;

    ConnectedWayPoints _currentWaypoint;
    ConnectedWayPoints _previousWaypoint;

    int visitedWaypoint;


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

            if (_currentWaypoint == null)
            {
                //Debug.Log("Patrol state failed");
                GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("waypoint");

                if(allWaypoints.Length > 0)
                {
                    while(_currentWaypoint == null)
                    {
                        int random = UnityEngine.Random.Range(0, allWaypoints.Length);
                        ConnectedWayPoints startingWaypoint = allWaypoints[random].GetComponent<ConnectedWayPoints>();

                        if(startingWaypoint != null)
                        {
                            _currentWaypoint = startingWaypoint;
                            //EnteredState = true;
                        }
                    }
                }
            }

            if (visitedWaypoint > 0)
                {
                    ConnectedWayPoints nextWaypoint = _currentWaypoint.NextWayPoint(_previousWaypoint);
                    _previousWaypoint = _currentWaypoint;
                    _currentWaypoint = nextWaypoint;
                }

                Vector3 targetVector = _currentWaypoint.transform.position;
                _navMeshAgent.SetDestination(targetVector);

                //SetDestination(_patrolPoints[_patrolPointIndex]);
                Debug.Log("ENTERRED PATROL STATE");
                EnteredState = true;
        }
        return EnteredState;
    }

    public override void UpdateState()
    {
        //TODO : Check successfull state entry
        if (EnteredState)
        {
            Debug.Log("UPDATING PATROL STATE");
            if (Vector3.Distance(_navMeshAgent.transform.position, _currentWaypoint.transform.position) <= 1.5f)
            {
                visitedWaypoint++;
                _fsm.EnterState(FSMStateType.IDLE);
            }
        }
    }

    public override bool ExitState()
    {
        base.ExitState();
        Debug.Log("EXITED PATROL STATE");
        return true;
    }

    private void SetDestination(PatrolPoints destination)
    {
        if (_navMeshAgent != null && destination != null)
        {
            _navMeshAgent.SetDestination(destination.transform.position);
        }

    }
}
