using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GamePhase { Real, Spirit };

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    GamePhase currentGamePhase;
    int currentSealCount;
    bool timerActive = true;
    bool timerFinished = false;
    float totalTimeLeft;
    float totalTime;


    // Start is called before the first frame update
    void Start()
    {
        main = this;

        currentGamePhase = GamePhase.Real;

        currentSealCount = 0;

        totalTime = Encense.burnDuration * 5;
        totalTimeLeft = totalTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            totalTimeLeft -= Time.deltaTime;

            if (totalTimeLeft <= 0)
            {
                timerActive = false;
                noTimeLeft();
            }
        }
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
        if (newPhase == GamePhase.Spirit)
        {
            SpiritVision.main.FadeIn();
            Messenger.Broadcast("EnteredSpiritWorld");
        } else
        {
            SpiritVision.main.FadeOut();
            Messenger.Broadcast("EnteredRealWorld");
        }
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

    //// TIME ////
    //////////////
    
    void LostTime(float amount)
    {
        totalTimeLeft = Mathf.Clamp(totalTimeLeft - amount, 0, totalTime);
    }

    public float getCurrentTimeLeft()
    {
        return totalTimeLeft;
    }

    void noTimeLeft()
    {
        timerFinished = true;
        Messenger.Broadcast("NoTimeLeft");
    }

    //// PLAYER ////
    ////////////////
    
    public void PlayerGotHit(float damage)
    {
        if (timerFinished)
        {
            GameOver();
        } else
        {
            LostTime(damage);

            //Damage sequence.
        }
    }

    void GameOver()
    {
        // Death screen

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
