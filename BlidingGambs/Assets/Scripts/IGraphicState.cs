
using UnityEngine;

public interface IGraphicState
{
    void PositiveFeedback(int step, AudioClip clip);
    void NegativeFeedback();
    void LooseFeedback();
    void WinFeedback();
    //void PerformCombo(ActionType.Type actionCombo);
    void Initialize();
}
