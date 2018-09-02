using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private Text _txtTimer;
    [SerializeField] private Image _fillPlayer;
    [SerializeField] private Image _fillEnemy;
    [SerializeField] private GameObject _panelGameOver;
    [SerializeField] private GameObject _panelGameWin;

    private PlayerController player;
    private EnemyController enemy;
    private bool gameStarted = false;

    private void Start()
    {
        _panelGameOver.SetActive(false);
        _panelGameWin.SetActive(false);

        UpdateTime();

        player = _gameController.GetPlayer();
        player.Health.OnDamage += PlayerDamage;
        player.Health.OnDead += PlayerDead;

        InitPlayerHealth();

        enemy = _gameController.GetEnemy();

        InitEnemyHealth();

        BeatManager.OnGameStarted += GameStarted;
        BeatManager.OnGamePaused += GamePaused;

        GameController.OnGameTimeComplete += ShowGameWin;
    }

    private void Update()
    {
        if (gameStarted)
            UpdateTime();
    }

    private void UpdateTime()
    {
        float currentTime = _gameController.GetCurrentTime();
        float segs = currentTime % 60;
        int mins = Mathf.FloorToInt(currentTime / 60);

        _txtTimer.text = string.Format("{0:00}:{1:00}", mins, segs);
    }

    private void PlayerDamage(int damage)
    {
        float perc = (float)player.GetCurrentHealth() / (float)player.MaxHealth();
        _fillPlayer.fillAmount = perc;
    }

    private void InitPlayerHealth()
    {
        Debug.Log("Player current Health: " + player.GetCurrentHealth() + ", max health: " + player.MaxHealth());
        float perc = (float)player.GetCurrentHealth() / (float)player.MaxHealth();
        _fillPlayer.fillAmount = perc;
    }

    private void InitEnemyHealth()
    {
        float perc = (float)enemy.GetCurrentHealth() / (float)enemy.MaxHealth();
        _fillEnemy.fillAmount = perc;
    }

    private void GameStarted()
    {
        gameStarted = true;
    }

    private void GamePaused()
    {
        gameStarted = false;
    }

    private void PlayerDead()
    {
        gameStarted = false;

        PlayerDamage(player.MaxHealth());

        Debug.Log("<b>UI</b>: Player is Dead");

        ShowGameOver();
    }

    private void ShowGameOver()
    {
        _panelGameOver.SetActive(true);
    }

    private void ShowGameWin()
    {
        _panelGameWin.SetActive(true);
        gameStarted = false;
    }

    private void OnDestroy()
    {
        player.Health.OnDamage -= PlayerDamage;
        player.Health.OnDead -= PlayerDead;

        BeatManager.OnGameStarted -= GameStarted;
        BeatManager.OnGamePaused -= GamePaused;

        GameController.OnGameTimeComplete -= ShowGameWin;
    }
}
