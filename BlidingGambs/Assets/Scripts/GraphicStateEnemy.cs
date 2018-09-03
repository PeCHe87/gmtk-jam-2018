using UnityEngine;

public class GraphicStateEnemy : MonoBehaviour, IGraphicState 
{
    [SerializeField] private Animator _anim;

    [SerializeField] private float _timeToHideFeedback;

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
    public void Idle()
    {
        //TODO: Show enemy idle animation
        Debug.Log("<color=orange>Enem</color>::Idle");
    }

    public void LoseFeedback()
    {
        //_sprBody.color = looserColor;

        goodFeedback = 0;

        //currentTimeToHideFeedback = _timeToHideFeedback;

        //Enemy lose feedback
        Debug.Log("<color=red>Enemy LOSE!</color>");

        _anim.SetTrigger("Dead");
    }

    public void NegativeFeedback(ScriptableCombo combo)
    {
        
    }

    public void PositiveFeedback(int step, AudioClip clip, int comboStep)
    {
        
    }

    public void WinFeedback()
    {
        //Show enemy as winner after attacking
        Debug.Log("<color=red>Enemy is WINNER!</color>");

        _anim.SetBool("Win", true);
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

        entityController.OnWin += WinFeedback;
        entityController.OnAttack += Attack;
        entityController.OnMissAttack += MissAttack;
        entityController.OnPreAttack += PreAttack;
        entityController.OnIdle += Idle;
        entityController.OnLose += LoseFeedback;
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

    private void Attack(ScriptableAttack attack)
    {
        int type = (attack.action == ActionType.Type.PUNCH) ? 0 : 1;

        //Show animation of attack based on type of attack
        _anim.SetInteger("AttackType", type);
        _anim.SetBool("AttackMiss", false);
        _anim.SetTrigger("Attack");
    }

    private void MissAttack(ScriptableAttack attack)
    {
        int type = (attack.action == ActionType.Type.PUNCH) ? 0 : 1;

        //Show animation of attack based on type of attack
        _anim.SetInteger("AttackType", type);
        _anim.SetBool("AttackMiss", true);
        _anim.SetTrigger("Attack");
    }

    private void PreAttack(ScriptableAttack attack)
    {
        int type = (attack.action == ActionType.Type.PUNCH) ? 0 : 1;

        //Show animation of pre attack based on type of attack
        _anim.SetInteger("AttackType", type);
        _anim.SetTrigger("AttackPre");
    }

    private void OnDestroy()
    {
        /*entityController.OnLoose -= LooseFeedback;*/
        entityController.OnWin -= WinFeedback;
        entityController.OnAttack -= Attack;
        entityController.OnMissAttack -= MissAttack;
        entityController.OnPreAttack -= PreAttack;
        entityController.OnIdle -= Idle;
        entityController.OnLose -= LoseFeedback;
    }
}
