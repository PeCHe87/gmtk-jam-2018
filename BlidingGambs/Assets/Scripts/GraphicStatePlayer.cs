using UnityEngine;

public class GraphicStatePlayer : MonoBehaviour, IGraphicState 
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

    public void NegativeFeedback()
    {
        _sprBad.SetActive(true);
        _sprGoodL.SetActive(false);
        _sprGoodR.SetActive(false);

        _sprBody.color = Color.white;

        currentTimeToHideFeedback = _timeToHideFeedback;
    }

    public void PositiveFeedback(int step)
    {
        _sprBad.SetActive(false);
        _sprGoodL.SetActive(goodFeedback % 2 == 0);
        _sprGoodR.SetActive(goodFeedback % 2 != 0);

        goodFeedback++;

        _sprBody.color = Color.white;

        currentTimeToHideFeedback = _timeToHideFeedback;
    }

    public void WinFeedback()
    {
        _sprBad.SetActive(false);
        _sprGoodL.SetActive(false);
        _sprGoodR.SetActive(false);

        _sprBody.color = winnerColor;
    }

    public void PerformCombo(ActionType.Type actionCombo)
    {
        _sprBad.SetActive(false);
        _sprGoodL.SetActive(false);
        _sprGoodR.SetActive(false);

        _sprBody.color = comboColor;

        currentTimeToHideFeedback = _timeToHideComboFeedback;

        switch (actionCombo)
        {
            case ActionType.Type.DODGE_DOWN:
                DodgeDown();
                break;

            case ActionType.Type.DODGE_UP:
                DodgeUp();
                break;
        }
    }

    public void Initialize()
    {
        HideFeedback();
    }
    #endregion

    private void Awake()
    {
        entityController = GetComponent<EntityController>();

        entityController.OnGoodComboStep += PositiveFeedback;
        entityController.OnBadComboStep += NegativeFeedback;
        entityController.OnLoose += LooseFeedback;
        entityController.OnWin += WinFeedback;
        entityController.OnComboComplete += PerformCombo;
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
        Debug.Log("Action: <b><color=magenta>DOWN</color></b>");
    }

    private void DodgeUp()
    {
        Debug.Log("Action: <b><color=magenta>UP</color></b>");
    }

    private void OnDestroy()
    {
        entityController.OnGoodComboStep -= PositiveFeedback;
        entityController.OnBadComboStep -= NegativeFeedback;
        entityController.OnLoose -= LooseFeedback;
        entityController.OnWin -= WinFeedback;
        entityController.OnComboComplete -= PerformCombo;
    }
}
