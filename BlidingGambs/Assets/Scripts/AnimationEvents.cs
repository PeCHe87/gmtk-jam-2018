using System;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public static Action OnPlayerFall;
    public static Action OnStopMainMusic;
    public static Action OnEnemyFall;
    public static Action OnEnemyDead;
    public static Action OnPlayerWin;
    public static Action OnPlayPuke;
    public static Action OnGameOverAfterFalling;
    public static Action OnPlaySpinningSound;

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
        //Play SFX of enemy crashing and player starts its win animation
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
        //Stop main music and play one shot a SFX to define (Turn table) 
        OnStopMainMusic();

        //Game over
        OnGameOverAfterFalling();
    }

    public void PlayerPreDefeatStart()
    {
        //Play spinning sound
        OnPlaySpinningSound();
    }

    public void EnemyPreDefeatPlayerWins()
    {
        //Player has to starts its dancing animation of win
        OnEnemyDead();
    }
}
