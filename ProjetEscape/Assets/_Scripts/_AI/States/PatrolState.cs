﻿using System.Collections;
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

    /// <summary>
    /// Get the direction towards the next waypoint
    /// </summary>
    Vector3 nextDestDir;

    /// <summary>
    /// The Y axis rotation 
    /// </summary>
    float angle;

    Quaternion rotation;
    Quaternion prevRotation;

    private bool rotated = false;

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
            prevRotation = _navMeshAgent.transform.rotation;

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
                            nextDestDir = _currentWaypoint.transform.position - _navMeshAgent.transform.position;
                            angle = Vector3.Angle(nextDestDir, _navMeshAgent.transform.forward);
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
                nextDestDir = _currentWaypoint.transform.position - _navMeshAgent.transform.position;
                angle = Vector3.Angle(nextDestDir, _navMeshAgent.transform.forward);
            }

                /*Vector3 targetVector = _currentWaypoint.transform.position;
            if (angle > 0 && angle < 180)
            {
                rotation = Quaternion.LookRotation(new Vector3(nextDestDir.x, nextDestDir.y - 30));
                //_navMeshAgent.transform.rotation = rotation;
                _navMeshAgent.transform.Rotate(rotation.x, rotation.y, rotation.z, Space.Self);
                if(_navMeshAgent.transform.rotation.y == prevRotation.y - 30)
                    _navMeshAgent.SetDestination(targetVector);
            }
            else if (angle < 0 && angle >= 180)
            {
                rotation = Quaternion.LookRotation(new Vector3(nextDestDir.x, nextDestDir.y + 30));
                _navMeshAgent.transform.Rotate(rotation.x, rotation.y, rotation.z, Space.Self);
                if(_navMeshAgent.transform.rotation.y == prevRotation.y + 30)
                    _navMeshAgent.SetDestination(targetVector);
            }
            else
                _navMeshAgent.SetDestination(targetVector);*/

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
            Vector3 targetVector = _currentWaypoint.transform.position;
            if (angle > 0 && angle < 180 && rotated == false)
            {
                rotation = Quaternion.LookRotation(new Vector3(nextDestDir.x, nextDestDir.y - 30));
                //_navMeshAgent.transform.rotation = rotation;
                _navMeshAgent.transform.Rotate(rotation.x, rotation.y, rotation.z, Space.World);
                Debug.Log("Previous rotation = " + prevRotation.eulerAngles.y);
                Debug.Log("Current Rotation = " + _navMeshAgent.transform.rotation.eulerAngles.y);
                if (_navMeshAgent.transform.rotation.y <= (prevRotation.eulerAngles.y - 30))
                {
                    rotated = true;
                    _navMeshAgent.SetDestination(targetVector);
                }
            }
            else if (angle < 0 && angle >= -180 && rotated == false)
            {
                rotation = Quaternion.LookRotation(new Vector3(nextDestDir.x, nextDestDir.y + 30));
                _navMeshAgent.transform.Rotate(rotation.x, rotation.y, rotation.z, Space.World);
                Debug.Log("Previous rotation = " + prevRotation.eulerAngles.y);
                Debug.Log("Current Rotation = " + _navMeshAgent.transform.rotation.eulerAngles.y);
                if (_navMeshAgent.transform.rotation.y >= (prevRotation.eulerAngles.y + 30))
                {
                    rotated = true;
                    _navMeshAgent.SetDestination(targetVector);
                }   
            }
            else
            {
                rotated = true;
                _navMeshAgent.SetDestination(targetVector);
            }

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

    public override void DetectedPlayer()
    {
        _fsm.EnterState(FSMStateType.ATTACK);
    }
}