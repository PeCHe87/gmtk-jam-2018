using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private ScriptableCombo _combo;
    [SerializeField] private bool comboStarted = false;     //TODO: combo is started when combo builder has something
    [SerializeField] private bool _canDebug = false;
    [SerializeField] private float _delayBeat = 250;

    private int currentComboStep = 0;
    private AudioToneManager beatManager;
    private bool skipBeat = false;
    private int currentBeat = 0;
    private StringBuilder currentCombo;
    private bool delayActive = false;

    private void Awake()
    {
        currentCombo = new StringBuilder();

        beatManager = GetComponent<AudioToneManager>();
        beatManager.OnBeat += NewBeat;

        InputController.OnActionKeyPressed += Action;
    }

    private void Update()
    {
        if (delayActive)
        {
            delayActive = false;
            StartCoroutine(CheckNewBeat());
        }
    }

    private void Action()
    {       
        if (CheckComboStep())
        {
            comboStarted = true;

            skipBeat = true;

            currentComboStep++;

            //TODO: feedback

            //Check if combo was completed
            CheckComboCompleted(); 
        }
        else
        {
             ResetCurrentCombo();
        }
    }

    private bool CheckComboStep(bool silence = false)
    {
        //Current beat 
        string beat = (currentBeat == 1) ? "H" : "L";

        //Skip the cheking if it is a beat to skip
        if (currentCombo.Length > 0)
        {
            string last = (currentCombo[currentCombo.Length - 1]).ToString().ToUpper();
            if (beat.Equals(last))
                return true;
        }

        if (silence)
            beat = beat.ToLower();

        //Append it to combo builder
        currentCombo.Append(beat);

        Debug.Log("Check current combo: " + currentCombo.ToString());

        //Check if current combo builder is prefix of combo
        int match = string.Compare(_combo.beats, 0, currentCombo.ToString(), 0, currentCombo.Length);

        if (match == 0)
            Debug.Log("<b><color=green>GOOD!!</color></b> - " + ((silence)?"Silence":"Action") + ", beat: " + FormatBeat(currentBeat) + ", current combo: " + currentCombo.ToString());

        return match == 0;
    }

    private void CheckComboCompleted()
    {
        //Check if combo was completed
        if (currentCombo.ToString().Equals(_combo.beats))
        {
            Debug.Log("<b><i><color=magenta>COMBOOOOOOOO!!!!</color></i></b> Action: " + _combo.keyAction);

            //TODO: feedback for combo completed

            //Reset current step
            SuccessfullCombo();
        }
    }

    private void NewBeat(int beat)
    {
        if (beat == 1 || beat == 3)
        {
            delayActive = true;
            currentBeat = beat;
        }
    }

    private IEnumerator CheckNewBeat()
    {
        yield return new WaitForSeconds(_delayBeat/1000);

        if (_canDebug && comboStarted)
            Debug.Log("beat: " + FormatBeat(currentBeat));

        if (skipBeat)
        {
            skipBeat = false;
            yield return null;
        }

        if (comboStarted)
        {
            if (CheckComboStep(true))
            {
                currentComboStep++;

                //TODO: feedback

                //Check if combo was completed
                CheckComboCompleted();
            }
            else
            {
                ResetCurrentCombo(true);
            }
        }
    }

    private string FormatBeat(int beat)
    {
        return (beat == 1)?"H":"L";
    }

    private void ResetCurrentCombo(bool silence = false)
    {
        Debug.Log("<b><color=red>BAD!!</color></b> current beat: " + FormatBeat(currentBeat) + "   --   " + ((silence)?"silence" : "action"));

        //Reset action started
        comboStarted = false;

        currentCombo.Remove(0, currentCombo.Length);
    }

    private void SuccessfullCombo()
    {
        //Reset action started
        comboStarted = false;

        currentCombo.Remove(0, currentCombo.Length);
    }

    private void OnDestroy()
    {
        InputController.OnActionKeyPressed -= Action;
        beatManager.OnBeat -= NewBeat;
    }
}
