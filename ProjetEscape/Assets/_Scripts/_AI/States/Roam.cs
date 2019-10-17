using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._Scripts._AI.States
{
    [CreateAssetMenu(fileName = "RoamState", menuName = "ProjetEscape/States/Roam", order = 4)]
    public class Roam : AbstractFSMClass
    {

        public override void OnEnable()
        {
            base.OnEnable();
            FSMStateType = FSMStateType.ROAM;
        }

        public override bool EnterState()
        {
            EnteredState = false;
            if (base.EnterState())
            {
                /*player = GameObject.FindGameObjectWithTag("Player").transform;
                Vector3 targetVector = player.transform.position;
                _navMeshAgent.SetDestination(targetVector);
                Debug.Log("ENTERRED ATTACK STATE");*/
                EnteredState = true;
            }

            return EnteredState;
        }

        public override void UpdateState()
        {
            if (EnteredState)
            {
                Debug.Log("UPDATING ROAM STATE");
            }
        }

        public override bool ExitState()
        {
            base.ExitState();
            Debug.Log("EXITED ROAM STATE");
            return true;
        }

    }
}
