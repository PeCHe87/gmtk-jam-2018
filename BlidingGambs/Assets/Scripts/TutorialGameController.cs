using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGameController : GameController {

    // Use this for initialization
    protected void Awake()
    {
        base.Awake();
        StartCoroutine(StartTutorial());
    }

    protected IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(0);

        GameStarted();
        _beatManager.StartGame();

        Debug.Log("Game Started");
    }
}
