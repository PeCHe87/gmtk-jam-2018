
using UnityEngine;

public interface IGraphicState
{
    void Idle();
    void PositiveFeedback(int step, AudioClip clip, int comboAction);
    void NegativeFeedback(ScriptableCombo combo);
    void LoseFeedback();
    void WinFeedback();
    //void PerformCombo(ActionType.Type actionCombo);
    void Initialize();
}
