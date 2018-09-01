using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerController : EntityController
{
    [Tooltip("List of possibles combos to perform")]
    [SerializeField] private List<ScriptableCombo> _combos;
    [SerializeField] private bool comboStarted = false;     //TODO: combo is started when combo builder has something
    [SerializeField] private bool _canDebug = false;
    [SerializeField] private BeatManager _beatManager;
    [SerializeField] private float _timeDelayBeforeComboComplete = 0.5f;

    private int currentComboStep = 0;
    private bool skipBeat = false;
    private char currentBeat = ' ';
    private StringBuilder currentCombo;
    private bool delayActive = false;
    private float delayBeat;

    public void Damage(int damage)  //TODO: add param to indicate the action performed by enemy
    {
        //Update HealthController

        //Show damage graphic animation feedback

        //SFX of cancel combo if player is building the combo

        //SFX of hit damage of type of attack

        //Cancel combo in progress if player was building a combo
    }

    private void Awake()
    {
        currentCombo = new StringBuilder();

        _beatManager.OnBeat += NewBeat;

        delayBeat = _beatManager.DelayBeat;

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
        //Skip the checking if it is a beat to skip
        if (currentCombo.Length > 0)
        {
            string last = (currentCombo[currentCombo.Length - 1]).ToString().ToUpper();

            string current = currentBeat.ToString().ToUpper();

            if (current.Equals(last))
                return true;
        }

        if (silence)
        {
            string lower = currentBeat.ToString().ToLower();
            currentBeat = lower[0];
        }

        //Append it to combo builder
        currentCombo.Append(currentBeat);

        for (int i = 0; i < _combos.Count; i++)
        {
            ScriptableCombo combo = _combos[i];

            //Check if current combo builder is prefix of combo
            int match = string.Compare(combo.beats, 0, currentCombo.ToString(), 0, currentCombo.Length);

            if (match == 0)
            {
                Debug.Log("<b><color=green>GOOD!!</color></b> - " + ((silence) ? "Silence" : "Action") + ", beat: " + currentBeat + ", current combo: " + currentCombo.ToString());
                OnGoodComboStep(currentCombo.Length);
            }

            if (match == 0)
                return true;
        }

        OnBadComboStep();

        return false;
    }

    private void CheckComboCompleted()
    {
        //Check if one of the combos was completed
        for (int i = 0; i < _combos.Count; i++)
        {
            ScriptableCombo combo = _combos[i];

            if (currentCombo.ToString().Equals(combo.beats))
            {
                Debug.Log("<b><i><color=magenta>COMBOOOOOOOO!!!!</color></i></b> Action: " + combo.keyAction);

                //Reset current step and feedback fo combo completed
                SuccessfullCombo(combo.keyAction);
            }
        }
    }

    private void NewBeat(char beat)
    {
        delayActive = true;
        currentBeat = beat;
    }

    private IEnumerator CheckNewBeat()
    {
        yield return new WaitForSeconds(delayBeat/1000);

        if (_canDebug && comboStarted)
            Debug.Log("beat: " + currentBeat);

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

    private void ResetCurrentCombo(bool silence = false)
    {
        Debug.Log("<b><color=red>BAD!!</color></b> current beat: " + currentBeat + "   --   " + ((silence)?"silence" : "action") + ", current combo: " + currentCombo.ToString());

        //Reset action started
        comboStarted = false;

        currentCombo.Remove(0, currentCombo.Length);
    }

    private void SuccessfullCombo(ActionType.Type actionCombo)
    {
        //Reset action started
        comboStarted = false;

        currentCombo.Remove(0, currentCombo.Length);

        StartCoroutine(ShowComboCompleteFeedback(actionCombo));
    }

    private IEnumerator ShowComboCompleteFeedback(ActionType.Type actionCombo)
    {
        yield return new WaitForSeconds(_timeDelayBeforeComboComplete);

        OnComboComplete(actionCombo);
    }

    private void OnDestroy()
    {
        InputController.OnActionKeyPressed -= Action;
        _beatManager.OnBeat -= NewBeat;
    }
}
