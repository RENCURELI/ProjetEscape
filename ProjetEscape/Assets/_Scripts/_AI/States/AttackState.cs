using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts._AI.States
{
    [CreateAssetMenu(fileName = "AttackState", menuName = "ProjetEscape/States/Attack", order = 3)]
    public class AttackState : AbstractFSMClass
    {
        public override void OnEnable()
        {
            base.OnEnable();
            FSMStateType = FSMStateType.ATTACK;
        }

        public override bool EnterState()
        {
            EnteredState = false;
            if (base.EnterState())
            {
                //TO IMPLEMENT
            }

            return EnteredState;
        }

        //TO DO
        public override void UpdateState()
        {
            if (EnteredState)
            {

            }
        }

        public override bool ExitState()
        {
            base.ExitState();

            return true;
        }
    }
}
