using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController
{
    [SerializeField] private GameController _gameController;
    [Tooltip("List of possibles attacks to perform")]
    [SerializeField] private List<ScriptableAttack> _attacks;
    [SerializeField] private bool _canDebug = false;

    private BeatManager _beatManager;

    private char currentBeat = ' ';

    private ScriptableAttack currentAttack = null;

    private int recoveryBeats = 2;
    private int preparationBeats = 0;
    private int toleranceBeats = 0;

    private void Awake()
    {
        _beatManager = _gameController.GetBeatManager();
        _beatManager.OnBeat += NewBeat;
    }

    private void NewBeat(char beat)
    {
        currentBeat = beat;
    }

    private void OnDestroy()
    {
        _beatManager.OnBeat -= NewBeat;
    }

    private void Update()
    {
        if (currentBeat != 'H')
            return;

        if (recoveryBeats > 0)
        {
            Debug.Log("Recovering from attack: " + recoveryBeats + " beats left");

            recoveryBeats--;
            return;
        }

        currentBeat = ' ';

        if (currentAttack != null)
        {
            preparationBeats--;

            if (preparationBeats == 0)
                PerformAttack();
            else
                Debug.Log("Preparing attack " + currentAttack.name + ": " + preparationBeats + " beats left");
        }
        else
        {
            ScriptableAttack attack = _attacks[Random.Range(0, _attacks.Count)];

            PrepareAttack(attack);
        };
    }

    private void PrepareAttack(ScriptableAttack attack)
    {
        currentAttack = attack;
        preparationBeats = attack.preparationBeats;
        toleranceBeats = attack.toleranceBeats;

        Debug.Log("Preparing attack " + currentAttack.name + ": " + preparationBeats + " beats left");
    }

    private void PerformAttack()
    {
        Debug.Log("Performing attack " + currentAttack.name + ": damage dealt " + currentAttack.damage);

        PlayerController player = _gameController.GetPlayer();

        if (toleranceBeats > 0 && player.PerformedSucessfulCombo())
        {
            toleranceBeats--;
            return;
        }

        // player.Damage(currentAttack.damage);

        recoveryBeats = currentAttack.recoveryBeats;
        toleranceBeats = 0;
        currentAttack = null;
    }
}
