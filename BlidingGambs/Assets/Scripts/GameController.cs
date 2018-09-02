using UnityEngine;

public class GameController : MonoBehaviour
{
    public static System.Action OnGameTimeComplete;

    [SerializeField] private PlayerController player;
    [SerializeField] private EnemyController enemy;
    [SerializeField] private int _gameTotalTime = 60;

    protected BeatManager _beatManager;
    private float currentTime = 0;
    private bool gameStarted = false;

    protected void Awake() 
    {
        _beatManager = GetComponent<BeatManager>();

        BeatManager.OnGameStarted -= GameStarted;
        BeatManager.OnGamePaused -= GamePaused;
    }

    private void Start()
    {
        if (enemy != null)
            enemy.Init(this);

        currentTime = _gameTotalTime;
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
    }

    protected void GameStarted()
    {
        gameStarted = true;
    }

    private void GamePaused()
    {
        gameStarted = false;
    }

    private void OnDestroy()
    {
       BeatManager.OnGameStarted -= GameStarted;
       BeatManager.OnGamePaused -= GamePaused;
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
