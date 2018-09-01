using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//https://www.reddit.com/r/Unity3D/comments/4eruce/sampleprecise_audio_syncing_in_unity_driving_me/

public class AudioToneManager : MonoBehaviour
{
    [Tooltip("Time between beats (BPM)")]
    [SerializeField] private float _timeBetweenBeats = 60;
    [SerializeField] private bool _gameStarted = false;
    [SerializeField] private Text _txtHigh, _txtLow, _txtHighInput, _txtLowInput;
    [SerializeField] private GameObject _btnStart, _btnPause;
    [SerializeField] private bool _showLow = true;
    [SerializeField] private float _timeToHideBeat = 0.5f;

    private bool isHigh = true;
    private AudioSource audioSource;
    private float currentTime = 0;

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

        //StartCoroutine(CheckTones());

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

        //currentTime = 0;

        _txtHighInput.color = Color.black;
        _txtLowInput.color = Color.black;

        PauseGame();


        accent = signatureHi;
        double startTick = AudioSettings.dspTime;
        sampleRate = AudioSettings.outputSampleRate;
        nextTick = startTick * sampleRate;
        running = true;
    }

    /*private void FixedUpdate()
    {
        if (_gameStarted)
        {
            Debug.Log("<b>Current time: </b>" + audioSource.time);

            currentTime += Time.fixedDeltaTime;

            if (currentTime >= _timeBetweenBeats/60)
            {
                ShowTone();

                currentTime = 0;
            }
        }
    }*/

    private void Update()
    {
        _txtHigh.text = (accent == 1) ? "HI" : string.Empty;
        _txtLow.text = (accent == 3) ? "LOW" : string.Empty;

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

    private IEnumerator CheckTones()
    {
        while (_gameStarted)
        {
            ShowTone();

            yield return new WaitForSeconds(60 / _timeBetweenBeats);
        }
    }

    private void ShowTone()
    {
        /*if (isHigh)
            Debug.Log("<color=green>Current time: </color>" + audioSource.time);
        else
            Debug.Log("<color=orange>Current time: </color>" + audioSource.time);*/

        if (isHigh)
        {
            _txtHigh.text ="1";
            _txtLow.text = string.Empty;
        }
        else
        {
            if (_showLow)
            {
                _txtHigh.text = string.Empty;
                _txtLow.text = "2";
            }
            else
            {
                _txtHigh.text = string.Empty;
                _txtLow.text = string.Empty;
            }
        }

        isHigh = !isHigh;
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
               
               Debug.Log("Tick: " + accent + "/" + signatureHi);

            }
            phase += amp * 0.3F;
            amp *= 0.993F;
            n++;
        }
    }
}
