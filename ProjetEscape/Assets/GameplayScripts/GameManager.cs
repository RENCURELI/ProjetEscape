using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase { Real, Spirit };

public class GameManager : MonoBehaviour
{
    GamePhase currentGamePhase;
    int currentSealCount;

    // Start is called before the first frame update
    void Start()
    {
        currentGamePhase = GamePhase.Real;
        currentSealCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //// GAMEPHASES ////
    ////////////////////

    public GamePhase GetCurrentGamePhase()
    {
        return currentGamePhase;
    }

    public void SetCurrentGamePhase(GamePhase newPhase)
    {
        currentGamePhase = newPhase;
        Debug.Log("Player is now in the " + currentGamePhase + " world.");

    }

    //// SEALS ////
    ///////////////

    public void CollectedSeal(string sealName)
    {
        currentSealCount++;

        if (currentSealCount >= 3)
        {
            Messenger.Broadcast("AllSealsCollected");
        } else
        {
            switch (sealName)
            {
                case "X":
                    Messenger.Broadcast("SealXCollected");
                    break;
                case "Y":
                    Messenger.Broadcast("SealYCollected");
                    break;
                case "Z":
                    Messenger.Broadcast("SealZCollected");
                    break;
            }
        }
    }

    public int GetCurrentSealCount()
    {
        return currentSealCount;
    }
}
