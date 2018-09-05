using UnityEngine;

public class GraphicStatePlayer : MonoBehaviour, IGraphicState 
{
    [SerializeField] private Animator _anim;

    [SerializeField] private float _timeToHideFeedback;
    //[SerializeField] private float _timeToHideComboFeedback;

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
        Debug.Log("Player Action: <b><color=cyan>IDLE</color></b>");

        /*_sprBad.SetActive(false);
        _sprGoodL.SetActive(false);
        _sprGoodR.SetActive(false);

        _sprBody.color = Color.white;*/

        _anim.SetTrigger("Idle");
    }

    public void LoseFeedback(ScriptableAttack attack)
    {
        Debug.Log("Player Action: <b><color=red>DEAD</color></b>");

        if (attack.action == ActionType.Type.KICK)
        {
            Vector3 scale = _anim.transform.localScale;
            scale.x *= -1;
            _anim.transform.localScale = scale;
        }

        _anim.ResetTrigger("Idle");
        _anim.SetBool("IsDead", true);
    }

    public void NegativeFeedback(ScriptableCombo combo)
    {
        _anim.ResetTrigger("Idle");
        _anim.SetTrigger("ComboMiss");
    }

    public void PositiveFeedback(int step, AudioClip clip, int comboType)
    {
        Debug.Log("Player::PositiveFeedback -- step: " + step);

        _anim.ResetTrigger("Idle");
        _anim.SetInteger("ComboStep", step);
        _anim.SetInteger("ComboType", comboType);
        _anim.SetTrigger("ComboGood");
    }

    public void WinFeedback()
    {
        Debug.Log("Player Action: <b><color=green>WIN</color></b>");

        _anim.ResetTrigger("Idle");
        _anim.SetTrigger("Win");
    }

    public void PerformCombo(ScriptableCombo actionCombo, int step)
    {
        _sprBad.SetActive(false);
        _sprGoodL.SetActive(false);
        _sprGoodR.SetActive(false);

        _sprBody.color = comboColor;

        currentTimeToHideFeedback = ((PlayerController)entityController).TimePerformingCombo;   //_timeToHideComboFeedback;

        /*switch (actionCombo.keyAction)
        {
            case ActionType.Type.DODGE_DOWN:
                DodgeDown();
                break;

            case ActionType.Type.DODGE_UP:
                DodgeUp();
                break;
        }*/

        Debug.Log("Player::ComboComplete -- step: " + step);

        _anim.ResetTrigger("Idle");
        _anim.SetInteger("ComboStep", step);
        _anim.SetTrigger("ComboGood");
        _anim.SetInteger("ComboType", (actionCombo.keyAction == ActionType.Type.DODGE_DOWN)?0:1);
    }

    public void Initialize()
    {
        ((PlayerController)entityController).Health.OnDead += Dead;

        HideFeedback();
    }
    #endregion

    private void Awake()
    {
        entityController = GetComponent<EntityController>();

        entityController.OnGoodComboStep += PositiveFeedback;
        entityController.OnBadComboStep += NegativeFeedback;
        entityController.OnLose += LoseFeedback;
        //entityController.OnWin += WinFeedback;
        entityController.OnComboComplete += PerformCombo;
        entityController.OnReceiveDamage += ReceiveDamage;
        entityController.OnIdle += Idle;
        AnimationEvents.OnEnemyDead += WinFeedback;
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
        _sprBad.SetActive(false);
        _sprGoodL.SetActive(false);
        _sprGoodR.SetActive(false);

        _sprBody.color = Color.white;
    }

    private void DodgeDown()
    {
        Debug.Log("Player Action: <b><color=magenta>DOWN</color></b>");
    }

    private void DodgeUp()
    {
        Debug.Log("Player Action: <b><color=magenta>UP</color></b>");
    }

    

    private void ReceiveDamage(ScriptableAttack attack)
    {
        Debug.Log("Action: player <b><color=orange>RECEIVES Damage</color></b> by " + attack.action);

        if (attack.action == ActionType.Type.KICK)
        {
            //Show damage by Kick animation
            _anim.SetTrigger("DamageKick");
        }
        else if (attack.action == ActionType.Type.PUNCH)
        {
            //Show damage by Punch animation
            _anim.SetTrigger("DamagePunch");
        }
    }

    private void Dead()
    {
        Debug.Log("<b><color=red>DEAD</color></b>");
    }

    private void OnDestroy()
    {
        entityController.OnGoodComboStep -= PositiveFeedback;
        entityController.OnBadComboStep -= NegativeFeedback;
        entityController.OnLose -= LoseFeedback;
        //entityController.OnWin -= WinFeedback;
        entityController.OnComboComplete -= PerformCombo;
        entityController.OnReceiveDamage -= ReceiveDamage;
        ((PlayerController)entityController).Health.OnDead -= Dead;
        AnimationEvents.OnEnemyDead -= WinFeedback;

        entityController.OnIdle -= Idle;
    }
}
