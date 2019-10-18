using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritMask : MonoBehaviour
{
    bool canUseMask; //Global on/off for the mask.
    bool maskReadyToUse = true; //Used by the cooldown
    public float cdBeforeMaskOff = 0.5f;

    float animTimeRealToSpirit = 3;
    float animTimeSpiritToReal = 2;

    GameManager gameManagerRef;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerRef = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        canUseMask = false; //Doesn't have mask initially.
    }

    // Update is called once per frame
    void Update()
    {
        if (canUseMask)
        {
            if (maskReadyToUse)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
                {
                    switch (gameManagerRef.GetCurrentGamePhase())
                    {
                        case GamePhase.Real:
                            // Transition Reel -> Esprit

                            gameManagerRef.SetCurrentGamePhase(GamePhase.Spirit);

                            maskReadyToUse = false;
                            Invoke("MaskIsReadyAgain", animTimeRealToSpirit + cdBeforeMaskOff);
                            break;

                        case GamePhase.Spirit:
                            // Transition Esprit -> Reel

                            gameManagerRef.SetCurrentGamePhase(GamePhase.Real);

                            maskReadyToUse = false;
                            Invoke("MaskIsReadyAgain", animTimeSpiritToReal);
                            break;
                    }
                }
            }
        }
    }

    void MaskIsReadyAgain()
    {
        maskReadyToUse = true;
    }

    public void CollectedSpiritMask()
    {
        canUseMask = true;
        Messenger.Broadcast("PlayerCollectedSpiritMask");
    }
}
