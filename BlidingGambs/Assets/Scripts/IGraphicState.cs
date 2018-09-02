
using UnityEngine;

public interface IGraphicState
{
    void Idle();
    void PositiveFeedback(int step, AudioClip clip);
    void NegativeFeedback(ScriptableCombo combo);
    void LooseFeedback();
    void WinFeedback();
    //void PerformCombo(ActionType.Type actionCombo);
    void Initialize();
}
