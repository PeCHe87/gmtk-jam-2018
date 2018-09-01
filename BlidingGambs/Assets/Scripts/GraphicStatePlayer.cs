using UnityEngine;

public class GraphicStatePlayer : MonoBehaviour, IGraphicState 
{
    private EntityController entityController;

    #region Implementation of Graphic State interface
    public void LooseFeedback()
    {
        throw new System.NotImplementedException();
    }

    public void NegativeFeedback()
    {
        throw new System.NotImplementedException();
    }

    public void PositiveFeedback(int step)
    {
        throw new System.NotImplementedException();
    }

    public void WinFeedback()
    {
        throw new System.NotImplementedException();
    }
    #endregion

    private void Awake()
    {
        entityController = GetComponent<EntityController>();

        entityController.OnGoodComboStep += PositiveFeedback;
        entityController.OnBadComboStep += NegativeFeedback;
        entityController.OnLoose += LooseFeedback;
        entityController.OnWin += WinFeedback;
    }

    private void OnDestroy()
    {
        entityController.OnGoodComboStep -= PositiveFeedback;
        entityController.OnBadComboStep -= NegativeFeedback;
        entityController.OnLoose -= LooseFeedback;
        entityController.OnWin -= WinFeedback;
    }
}
