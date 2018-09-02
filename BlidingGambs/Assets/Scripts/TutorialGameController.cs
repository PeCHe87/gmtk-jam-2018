using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGameController : GameController {

    // Use this for initialization
    void Awake()
    {
        base.Awake();
        StartCoroutine(StartTutorial());
    }

    IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(2);

        GameStarted();
        _beatManager.StartGame();

        Debug.Log("Game Started");
    }
}
