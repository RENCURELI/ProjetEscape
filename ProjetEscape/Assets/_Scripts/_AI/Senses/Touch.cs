﻿using System;
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

        private bool detected = false;

        public GameManager gm;

        void OnTriggerEnter(Collider other)
        {
            if (gm.GetCurrentGamePhase().Equals(GamePhase.Spirit))
            {
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    print("Enemy Touch Detected");
                    detected = true;
                    spotted.Invoke();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Player player = other.GetComponent<Player>();
            if (player != null && detected == true)
            {
                print("Enemy Touch Lost");
                detected = false;
                lost.Invoke();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if(player != null)
            {
                gm.PlayerGotHit(20f);
            }
        }
    }
}
