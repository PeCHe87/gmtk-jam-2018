using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSourceSFX;
    [SerializeField] private AudioSource _audioSourceMain;
    [SerializeField] private AudioClip _clipPuke;
    [SerializeField] private AudioClip _clipEnemyCrash;
    [SerializeField] private AudioClip _clipWinScratching;
    [SerializeField] private AudioClip _clipPlayerCrash;
    [SerializeField] private AudioClip _clipMainMusic;
    [SerializeField] private AudioClip _clipPublicOvation;
    [SerializeField] private AudioClip _clipPlayerSpinning;
    [SerializeField] private AudioClip _clipFightTimeout;

    private BeatManager beatManager;

    private void Awake()
    {
        AnimationEvents.OnPlayerFall += PlayerFall;
        AnimationEvents.OnEnemyFall += EnemyFall;
        AnimationEvents.OnPlayerWin += PlayerWin;
        AnimationEvents.OnPlayPuke += EnemyPuke;
        AnimationEvents.OnStopMainMusic += StopMainMusic;
        AnimationEvents.OnPlaySpinningSound += PlayPlayerSpinning;
        GameController.OnGameTimeComplete += FightTimeout;

        beatManager = GetComponent<BeatManager>();

        beatManager.SetMainSong(_clipMainMusic);
    }

    private void Start()
    {
        //beatManager = GetComponent<BeatManager>();

        //beatManager.SetMainSong(_clipMainMusic);
    }

    private void PlayerFall()
    {
        //Play player fall
        _audioSourceSFX.PlayOneShot(_clipPlayerCrash);
    }

    private void EnemyFall()
    {
        //Play enemy fall
        _audioSourceSFX.volume = 1.0f;
        _audioSourceSFX.PlayOneShot(_clipEnemyCrash);
    }

    private void PlayerWin()
    {
        Debug.Log("🔉 - Ovation!");
        //Play public ovation
        _audioSourceSFX.PlayOneShot(_clipPublicOvation);
    }

    private void EnemyPuke()
    {
        //Play enemy puke
        _audioSourceSFX.PlayOneShot(_clipPuke);
    }

    private void StopMainMusic()
    {
        //Stop main music
        _audioSourceMain.Stop();

        //Play scracth
        _audioSourceSFX.PlayOneShot(_clipWinScratching);
    }

    private void PlayPlayerSpinning()
    {
        _audioSourceSFX.PlayOneShot(_clipPlayerSpinning);
    }

    private void FightTimeout()
    {
        _audioSourceSFX.volume = 0.1f;
        _audioSourceSFX.PlayOneShot(_clipFightTimeout);
    }

    private void OnDestroy()
    {
        AnimationEvents.OnPlayerFall -= PlayerFall;
        AnimationEvents.OnEnemyFall -= EnemyFall;
        AnimationEvents.OnPlayerWin -= PlayerWin;
        AnimationEvents.OnPlayPuke -= EnemyPuke;
        AnimationEvents.OnStopMainMusic -= StopMainMusic;
        AnimationEvents.OnPlaySpinningSound -= PlayPlayerSpinning;
        GameController.OnGameTimeComplete -= FightTimeout;
    }
}
