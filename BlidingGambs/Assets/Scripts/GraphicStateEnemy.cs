using UnityEngine;

public class GraphicStateEnemy : MonoBehaviour, IGraphicState 
{
    [SerializeField] private float _timeToHideFeedback;
    [SerializeField] private float _timeToHideComboFeedback;

    [Header("Color feedbacks")]
    [SerializeField] private Color comboColor = Color.magenta;
    [SerializeField] private Color winnerColor = Color.green;
    [SerializeField] private Color looserColor = Color.red;

    [Header("Sprite feedbacks")]
    [SerializeField] private SpriteRenderer _sprBody;
    [SerializeField] private GameObject _sprBad;
    [SerializeField] private GameObject _sprGoodL;
    [SerializeField] private GameObject _sprGoodR;

    private EntityController entityController;
    private float currentTimeToHideFeedback = 0;
    private int goodFeedback = 0;

    #region Implementation of Graphic State interface
    public void LooseFeedback()
    {
        _sprBad.SetActive(false);
        _sprGoodL.SetActive(false);
        _sprGoodR.SetActive(false);

        _sprBody.color = looserColor;

        goodFeedback = 0;

        currentTimeToHideFeedback = _timeToHideFeedback;
    }

    public void NegativeFeedback(ScriptableCombo combo)
    {
        
    }

    public void PositiveFeedback(int step, AudioClip clip)
    {
        
    }

    public void WinFeedback()
    {
        /*_sprBad.SetActive(false);
        _sprGoodL.SetActive(false);
        _sprGoodR.SetActive(false);

        _sprBody.color = winnerColor;*/

        //TODO: Show enemy as winner after attacking
        Debug.Log("<color=red>Enemy is WINNER!</color>");
    }

    public void PerformCombo(ScriptableCombo actionCombo)
    {
        
    }

    public void Initialize()
    {
        HideFeedback();
    }
    #endregion

    private void Awake()
    {
        entityController = GetComponent<EntityController>();

        /*entityController.OnLoose += LooseFeedback;*/
        entityController.OnWin += WinFeedback;
        entityController.OnAttack += Attack;
        entityController.OnMissAttack += MissAttack;
        entityController.OnPreAttack += PreAttack;
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (currentTimeToHideFeedback > 0)
        {
            currentTimeToHideFeedback -= Time.deltaTime;

            if (currentTimeToHideFeedback <= 0)
            {
                currentTimeToHideFeedback = 0;
                HideFeedback();
            }
        }
    }

    private void HideFeedback()
    {
        /*_sprBad.SetActive(false);
        _sprGoodL.SetActive(false);
        _sprGoodR.SetActive(false);*/

        _sprBody.color = Color.white;
    }

    private void ReceiveDamage(ScriptableAttack attack)
    {
        if (attack.action == ActionType.Type.KICK)
        {
            //TODO: Show damage by Kick animation
        }
        else if (attack.action == ActionType.Type.PUNCH)
        {
            //TODO: show damage by Punch animation
        }
    }

    private void Attack(ScriptableAttack attack)
    {
        //TODO: Show animation based on type of attack
    }

    private void MissAttack(ScriptableAttack attack)
    {
        //TODO: Show animation based on type of attack

        //TODO: Add right value to the sliding after missing attack
    }

    private void PreAttack(ScriptableAttack attack)
    {
        //TODO: Show animation of pre attack based on type of attack
    }

    private void OnDestroy()
    {
        /*entityController.OnLoose -= LooseFeedback;*/
        entityController.OnWin -= WinFeedback;
        entityController.OnAttack -= Attack;
        entityController.OnMissAttack -= MissAttack;
        entityController.OnPreAttack -= PreAttack;
    }
}
