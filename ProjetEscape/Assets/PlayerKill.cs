using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKill : MonoBehaviour
{
    public GameManager gm;
    void OnTriggerEnter(Collider other)
    {
        if (gm.GetCurrentGamePhase().Equals(GamePhase.Spirit))
        { Player player = other.gameObject.GetComponent<Player>();
            if(player != null)
            {
                gm.PlayerGotHit(20f);
            }
        }
    }
}
