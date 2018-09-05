using System.Collections;
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
    [SerializeField] private Text _txtPreviousFight;
    [SerializeField] private float _timeToShowNumber1, _timeToShowNumber2, _timeToShowNumber3, _timeToShowFight, _timeToHide;
    [SerializeField] private Color _color1, _color2, _color3, _colorFight;

    private PlayerController player;
    private EnemyController enemy;
    private bool gameStarted = false;

    private void Awake()
    {
        BeatManager.OnGameStarted += GameStarted;
        BeatManager.OnGamePaused += GamePaused;
    }

    private void Start()
    {
        _panelGameOver.SetActive(false);
        _panelGameWin.SetActive(false);

        UpdateTime();

        player = _gameController.GetPlayer();
        player.Health.OnDamage += PlayerDamage;
        player.Health.OnDead += PlayerDead;
        AnimationEvents.OnPlayerFall += GameOver;

        InitPlayerHealth();

        enemy = _gameController.GetEnemy();

        InitEnemyHealth();

        //BeatManager.OnGameStarted += GameStarted;
        //BeatManager.OnGamePaused += GamePaused;

        AnimationEvents.OnEnemyDead += ShowGameWin;
    }

    private void Update()
    {
        if (gameStarted)
            UpdateTime();
    }

    private void UpdateTime()
    {
        float currentTime = _gameController.GetCurrentTime();
        float segs = Mathf.FloorToInt(currentTime) % 60;
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

        StartCoroutine(ShowNumbers());
    }

    private IEnumerator ShowNumbers()
    {
        yield return new WaitForSeconds(_timeToShowNumber1);

        _txtPreviousFight.color = _color1;
        _txtPreviousFight.text = "1";

        yield return new WaitForSeconds(_timeToShowNumber2);

        _txtPreviousFight.color = _color2;
        _txtPreviousFight.text = "2";

        yield return new WaitForSeconds(_timeToShowNumber3);

        _txtPreviousFight.color = _color3;
        _txtPreviousFight.text = "3";

        yield return new WaitForSeconds(_timeToShowFight);

        _txtPreviousFight.color = _colorFight;
        _txtPreviousFight.text = "Dance!";

        yield return new WaitForSeconds(_timeToHide);

        _txtPreviousFight.text = string.Empty;
    }

    private void GamePaused()
    {
        gameStarted = false;
    }

    private void PlayerDead()
    {
        gameStarted = false;

        PlayerDamage(player.MaxHealth());
    }

    private void GameOver()
    {
        Debug.Log("<b>UI</b>: Player is Dead - GameOver");

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
        AnimationEvents.OnPlayerFall -= GameOver;

        BeatManager.OnGameStarted -= GameStarted;
        BeatManager.OnGamePaused -= GamePaused;

        player.Health.OnDead -= PlayerDead;

        AnimationEvents.OnEnemyDead -= ShowGameWin;
    }
}
