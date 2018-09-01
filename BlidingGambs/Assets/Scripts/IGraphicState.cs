
public interface IGraphicState
{
    void PositiveFeedback(int step);
    void NegativeFeedback();
    void LooseFeedback();
    void WinFeedback();
    void PerformCombo(ActionType.Type actionCombo);
    void Initialize();
}
