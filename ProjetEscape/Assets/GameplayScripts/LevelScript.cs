using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    GameManager gameManagerRef;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerRef = GetComponent<GameManager>();

        Messenger.AddListener("PlayerEnteredTemple", PlayerEnteredForTheFirstTime);
        Messenger.AddListener("PlayerCollectedSpiritMask", PlayerCollectedMask);

        Messenger.AddListener("SealXCollected", PlayerCollectedSealX);
        Messenger.AddListener("SealYCollected", PlayerCollectedSealY);
        Messenger.AddListener("SealZCollected", PlayerCollectedSealZ);
        Messenger.AddListener("AllSealsCollected", PlayerCollectedAllSeals);

        Messenger.AddListener("PlayerEscapedTemple", End);

        Messenger.AddListener("NoTimeLeft", TimerReachedZero);

    }

    
    //// PLAYER ENTERED TEMPLE ////
    ///////////////////////////////

    void PlayerEnteredForTheFirstTime()
    {
        Messenger.Broadcast("MainDoors_Open");

    }

    //// PLAYER COLLECTED MASK ////
    ///////////////////////////////

    void PlayerCollectedMask()
    {
        Messenger.Broadcast("MainDoors_Close");
        Messenger.Broadcast("EnableEscapeTrigger");
    }

    //// SEALS COLLECTED ////
    /////////////////////////

    void PlayerCollectedSealX()
    {

    }

    void PlayerCollectedSealY()
    {

    }

    void PlayerCollectedSealZ()
    {

    }

    //// PLAYER ACHIEVED FINAL OBJECTIVE /////
    //////////////////////////////////////////

    void PlayerCollectedAllSeals()
    {
        Messenger.Broadcast("MainDoors_Open");
    }

    //// TIMER REACHED ZERO /////
    /////////////////////////////
    
    void TimerReachedZero()
    {

    }

    //// PLAYER ESCAPED TEMPLE /////
    ////////////////////////////////
    
    void End()
    {
        Debug.Log("You won");
    }
}
