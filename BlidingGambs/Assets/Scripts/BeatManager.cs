using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//https://www.reddit.com/r/Unity3D/comments/4eruce/sampleprecise_audio_syncing_in_unity_driving_me/

public class BeatManager : MonoBehaviour
{
    public System.Action<char> OnBeat;

    [SerializeField] private bool _gameStarted = false;
    [SerializeField] private Text _txtHigh, _txtLow, _txtHighInput, _txtLowInput;
    [SerializeField] private GameObject _btnStart, _btnPause;
    [SerializeField] private bool _showLow = true;
    [SerializeField] private float _timeToHideBeat = 0.5f;
    [SerializeField] private float _delayBeat = 250;

    private bool isHigh = true;
    private AudioSource audioSource;
    private float currentTime = 0;

    public float DelayBeat { get { return _delayBeat; } }

    [Tooltip("Time between beats (BPM)")]
    public double bpm = 140.0F;
    public float gain = 0.5F;
    public int signatureHi = 4;
    public int signatureLo = 4;
    private double nextTick = 0.0F;
    private float amp = 0.0F;
    private float phase = 0.0F;
    private double sampleRate = 0.0F;
    private int accent;
    private bool running = false;
    private bool showTone = false;

    public void StartGame()
    {
        _gameStarted = true;
        _txtHigh.text = string.Empty;
        _txtLow.text = string.Empty;

        _btnPause.SetActive(true);
        _btnStart.SetActive(false);

        audioSource.Play();
    }

    public void PauseGame()
    {
        _gameStarted = false;
        _txtHigh.text = string.Empty;
        _txtLow.text = string.Empty;

        _btnPause.SetActive(false);
        _btnStart.SetActive(true);

        audioSource.Pause();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        _txtHighInput.color = Color.black;
        _txtLowInput.color = Color.black;

        PauseGame();


        accent = signatureHi;
        double startTick = AudioSettings.dspTime;
        sampleRate = AudioSettings.outputSampleRate;
        nextTick = startTick * sampleRate;
        running = true;
    }

    private void Update()
    {
        _txtHigh.text = (accent % 2 == 1) ? "HI" : string.Empty;
        _txtLow.text = (accent % 2 == 0) ? "LOW" : string.Empty;

        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            _txtHighInput.color = Color.black;
            _txtLowInput.color = Color.black;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowFeedback(accent);
        }
    }

    private void ShowFeedback(int tick)
    {
        currentTime = _timeToHideBeat;
        _txtHighInput.color = (tick == 1)?Color.green:((tick == 2)?Color.red:Color.black);
        _txtLowInput.color = (tick == 3) ? Color.green : ((tick == 4) ? Color.red : Color.black);
    }

    private IEnumerator HideTone()
    {
        yield return new WaitForSeconds(_timeToHideBeat);

        _txtHigh.text = string.Empty;
        _txtLow.text = string.Empty;

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
