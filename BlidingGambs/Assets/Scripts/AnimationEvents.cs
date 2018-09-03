using System;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public static Action OnPlayerFall;
    public static Action OnStopMainMusic;
    public static Action OnEnemyFall;
    public static Action OnPlayerWin;
    public static Action OnPlayPuke;
    public static Action OnGameOverAfterFalling;

    public void CrashPlayerPreDefeat()
    {
        //Play SFX of player crashing floor
        OnPlayerFall();
    }

    public void EndPlayerPreDefeat()
    {
        //Stop main music and play one shot a SFX to define (Turn table) 
        OnStopMainMusic();
    }

    public void CrashEnemyPreDefeat()
    {
        //Play SFX of enemy crashing
        OnEnemyFall();
    }

    public void EndEnemyPreDefeat()
    {
        //Player has to play its winner animation
        OnPlayerWin();
    }

    public void PreDefeatEnemyPuking()
    {
        //Play SFX of puke
        OnPlayPuke();
    }

    public void PlayerPreDefeatEnd()
    {
        //Game over
        OnGameOverAfterFalling();
    }
}
