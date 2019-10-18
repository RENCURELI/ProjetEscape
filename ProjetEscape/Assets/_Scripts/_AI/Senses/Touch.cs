using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._Scripts._AI.Senses
{
    public class Touch : Sense
    {
        //Base for sending messages upon spotting or losing the player
        public UnityEvent spotted;
        public UnityEvent lost;

        void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                print("Enemy Touch Detected");
                spotted.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                print("Enemy Touch Lost");
                lost.Invoke();
            }
        }
    }
}
