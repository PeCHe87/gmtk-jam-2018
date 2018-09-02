using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController
{
    [Tooltip("List of possibles attacks to perform")]
    [SerializeField] private List<ScriptableAttack> _attacks;
    [SerializeField] private bool _canDebug = false;
    [Tooltip("First steps that enemy waits for to start its behaviour")]
    [SerializeField] private int _firstRecoveryBeats = 30;
    [Tooltip("Fixed attacks that enemy will use before randomness")]
    [SerializeField] private ScriptableAttack[] _firstFixedAttacks;

    private int _recoveryBeats = 0;
    private int _preparationBeats = 0;
    private int _toleranceBeats = 0;
    private GameController _gameController;
    private BeatManager _beatManager;
    private PlayerController player;
    private char currentBeat = ' ';
    private ScriptableAttack currentAttack = null;
    private bool playerIsDead = false;
    private HealthController healthController;
    private int firstAttacksDone = 0;

    public void Init(GameController gameController)
    {
        _gameController = gameController;

        _beatManager = _gameController.GetBeatManager();
        _beatManager.OnBeat += NewBeat;

        player = _gameController.GetPlayer();
        player.Health.OnDead += PlayerDead;
    }

    public int GetCurrentHealth()
    {
        return healthController.Health;
    }

    public int MaxHealth()
    {
        return healthController.MaxHealth;
    }

    private void Awake()
    {
        healthController = GetComponent<HealthController>();
    }

    private void Start()
    {
        _recoveryBeats = _firstRecoveryBeats;
    }

    private void NewBeat(char beat)
    {
        currentBeat = beat;
    }

    private void PlayerDead()
    {
        playerIsDead = true;

        OnWin();
    }

    private void OnDestroy()
    {
        _beatManager.OnBeat -= NewBeat;
        player.Health.OnDead -= PlayerDead;
    }

    private void Update()
    {
        if (playerIsDead)
            return;

        if (currentBeat != 'H')
            return;

        currentBeat = ' ';

        if (currentAttack != null)
        {
            _preparationBeats--;

            if (_preparationBeats <= 0)
                PerformAttack();
            else
            {
                Debug.Log("Preparing attack <color=orange>" + currentAttack.name + "</color>: " + _preparationBeats + " beats left");
            }
        }
        else
        {
            if (_recoveryBeats > 0)
            {
                Debug.Log("Recovering from attack: " + _recoveryBeats + " beats left   ----    currenBeat: " + currentBeat);

                _recoveryBeats--;

                if (_recoveryBeats == 0)
                    Debug.Log("<color=blue>Enemy starts to prepare to choose an attack</color>");

                return;
            }

            ScriptableAttack attack;

            //Check if the first fixed attacks were performed
            if (firstAttacksDone < _firstFixedAttacks.Length)
            {
                attack = _firstFixedAttacks[firstAttacksDone];
                firstAttacksDone++;
            }
            else
            {
                attack = _attacks[Random.Range(0, _attacks.Count)];
            }

            PrepareAttack(attack);
        };
    }

    private void PrepareAttack(ScriptableAttack attack)
    {
        //Start graphic state (Preparation)
        OnPreAttack(attack);

        currentAttack = attack;
        _preparationBeats = attack.preparationBeats;
        _toleranceBeats = attack.toleranceBeats;

        Debug.Log("Preparing attack <b>" + currentAttack.name + "</b>: " + _preparationBeats + " beats left");
    }

    private void PerformAttack()
    {
        bool playerIsPerformingCombo = player.IsPerformingComboToAvoidAttack(currentAttack);

        if (_toleranceBeats > 0 && playerIsPerformingCombo)
        {
            _toleranceBeats--;

            Debug.Log("Performing attack and player is performing combo: " + currentAttack.name + ": tolerance beats left: " + _toleranceBeats);

            return;
        }

        //Update graphic state (based Action selected)
        if (!playerIsPerformingCombo)
        {
            Debug.Log("Enemy::Performing <color=red>Successfull</color> attack " + currentAttack.name + ": damage dealt " + currentAttack.damage);

            OnAttack(currentAttack);

            player.Damage(currentAttack.damage, currentAttack);
        }
        else
        {
            Debug.Log("Enemy::Performing <color=yellow>Miss</color> attack " + currentAttack.name);

            //SFX swoosh (miss). Save state of miss attack to show an animation after attacking based on the attack made
            OnMissAttack(currentAttack);
        }

        _recoveryBeats = currentAttack.recoveryBeats;
        _toleranceBeats = 0;
        currentAttack = null;
    }
}
