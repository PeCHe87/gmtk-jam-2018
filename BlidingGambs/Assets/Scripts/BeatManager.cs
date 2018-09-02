using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//https://www.reddit.com/r/Unity3D/comments/4eruce/sampleprecise_audio_syncing_in_unity_driving_me/

public class BeatManager : MonoBehaviour
{
    public System.Action<char> OnBeat;
    public static System.Action OnGameStarted;
    public static System.Action OnGamePaused;

    [SerializeField] private bool _gameStarted = false;
    [SerializeField] private float _delayBeat = 250;

    private bool isHigh = true;
    private AudioSource audioSource;
    private float currentTime = 0;

    public float DelayBeat { get { return _delayBeat; } }

    [Tooltip("Time between beats (BPM)")]
    public double bpm = 120.0F;
    public float gain = 0.5F;
    public int signatureHi = 4;
    public int signatureLo = 4;
    private double nextTick = 0.0F;
    private float amp = 0.0F;
    private float phase = 0.0F;
    private double sampleRate = 0.0F;
    private int accent;
    private bool showTone = false;

    public void StartGame()
    {
        _gameStarted = true;

        audioSource.Play();

        if (OnGameStarted != null)
            OnGameStarted();
    }

    public void PauseGame()
    {
        _gameStarted = false;

        audioSource.Pause();

        if (OnGamePaused != null)
            OnGamePaused();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        PauseGame();

        accent = signatureHi;
        double startTick = AudioSettings.dspTime;
        sampleRate = AudioSettings.outputSampleRate;
        nextTick = startTick * sampleRate;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        if (!_gameStarted)
            return;

        double samplesPerTick = sampleRate * 60.0F / bpm * 4.0F / signatureLo;
        double sample = AudioSettings.dspTime * sampleRate;
        int dataLen = data.Length / channels;
        int n = 0;

        while (n < dataLen)
        {
            float x = gain * amp * Mathf.Sin(phase);
            int i = 0;

            while (i < channels)
            {
                data[n * channels + i] += x;
                i++;
            }

            while (sample + n >= nextTick)
            {
                nextTick += samplesPerTick;
                amp = 1.0F;

                if (++accent > signatureHi)
                {
                    accent = 1;
                    amp *= 2.0F;
                }

                if (accent == 1)
                    showTone = true;

                OnBeat((accent % 2 == 0)?'L':'H');

            }

            phase += amp * 0.3F;
            amp *= 0.993F;
            n++;
        }
    }
}
