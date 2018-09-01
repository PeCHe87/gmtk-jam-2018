using UnityEngine;

[CreateAssetMenu(fileName = "Combo", menuName = "Tools/Combo", order = 1)]
public class ScriptableCombo : ScriptableObject
{
    public string comboName;
    [Tooltip("Beats marked 'High' or 'Low' if they need input or 'high'/'low' in another case (no input)")]
    public string beats;
    [Tooltip("The key of the action to perform when combo is successfully completed")]
    public ActionType.Type keyAction;
}
