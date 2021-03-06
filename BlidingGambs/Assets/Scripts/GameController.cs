﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static System.Action OnGameTimeComplete;

    [SerializeField] private PlayerController player;
    [SerializeField] private EnemyController enemy;
    [SerializeField] private int _gameTotalTime = 60;

    [Header("Floor")]
    [SerializeField] private SpriteRenderer _sprLights;
    [SerializeField] private SpriteRenderer _sprFloor;
    [SerializeField] private Sprite _sprFloorA;
    [SerializeField] private Sprite _sprFloorB;
    [SerializeField] private float _timeToRestartGame = 15;


    protected BeatManager _beatManager;
    private float currentTime = 0;
    private bool gameStarted = false;
    private bool delayActive = false;
    private char currentBeat = ' ';
    private int indexBeat = -1;
    private bool changeFloor = false;
    private bool showLights = false;

    protected void Awake() 
    {
        _beatManager = GetComponent<BeatManager>();
        _beatManager.OnBeat += NewBeat;

        BeatManager.OnGameStarted += GameStarted;
        BeatManager.OnGamePaused += GamePaused;

        if (enemy)
            enemy.OnWin += GameOver;

        if (player)
            player.OnWin += GameOver;

        currentTime = _gameTotalTime;
    }

    private void Start()
    {
        if (enemy != null)
            enemy.Init(this);
    }

    private void Update()
    {
        if (gameStarted)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                currentTime = 0;

                gameStarted = false;

                OnGameTimeComplete();
            }
        }

        _sprLights.enabled = showLights;

        if (changeFloor)
            ChangeFloor();
    }

    private void ChangeFloor()
    {
        Sprite currentFloor = _sprFloor.sprite;

        if (currentFloor == _sprFloorA)
            _sprFloor.sprite = _sprFloorB;
        else
            _sprFloor.sprite = _sprFloorA;

        changeFloor = false;
    }

    private void NewBeat(char beat)
    {
        //if (!gameStarted)
        //    return;

        indexBeat++;

        if (indexBeat % 2 == 0)
            changeFloor = true;

        showLights = (indexBeat % 2 == 0);
    }

    protected void GameStarted()
    {
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(2);

        gameStarted = true;
    }

    private void GamePaused()
    {
        gameStarted = false;
    }

    private void GameOver()
    {
        gameStarted = false;

        StartCoroutine(RestartGame());
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(_timeToRestartGame);

        SceneManager.LoadScene("Menu");
    }

    private void OnDestroy()
    {
        BeatManager.OnGameStarted -= GameStarted;
        BeatManager.OnGamePaused -= GamePaused;

        if (enemy)
            enemy.OnWin -= GameOver;

        if (player)
            player.OnWin -= GameOver;
    }

    public PlayerController GetPlayer()
    {
        return player;
    }

    public EnemyController GetEnemy()
    {
        return enemy;
    }

    public BeatManager GetBeatManager()
    {
        return _beatManager;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
}
