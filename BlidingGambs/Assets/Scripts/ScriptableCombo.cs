using UnityEngine;

[CreateAssetMenu(fileName = "Combo", menuName = "Tools/Combo", order = 1)]
public class ScriptableCombo : ScriptableObject
{
    public string comboName;
    [Tooltip("Beats marked 'High' or 'Low' if they need input or 'high'/'low' in another case (no input)")]
    public string beats;
    [Tooltip("The key of the action to perform when combo is successfully completed")]
    public ActionType.Type keyAction;
    [Tooltip("Audio to play when performs the action")]
    public AudioClip[] clipsFeedback;
    [Tooltip("Amount of beats that player is freezing in that state until user recovers the control")]
    public int beatsFreezingAfterCombo = 4;
    [Tooltip("The attack that is blocked when combo is successfuly performed")]
    public ScriptableAttack enemyAttackBlocked;
    [Tooltip("Audio to play when performs the action in a wrong way")]
    public AudioClip clipBadStep;
}
