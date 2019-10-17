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
        /// <summary>
        /// Last known player position
        /// </summary>
        Vector3 lastPCPos;

        /// <summary>
        /// Randomly selected roaming destination
        /// </summary>
        Vector3 roamDest;

        /// <summary>
        /// Roaming distance constraints
        /// </summary>
        int min_x, max_x, min_z, max_z;

        float elapsedTime = 0.0f;

        [SerializeField]
        float roamDuration = 6.0f;

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
