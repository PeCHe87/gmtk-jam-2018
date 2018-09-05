using System;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    #region Events
    public Action<int, AudioClip, int> OnGoodComboStep;
    public Action<ScriptableCombo> OnBadComboStep;
    public Action<ScriptableAttack> OnLose;
    public Action OnWin;
    public Action<ScriptableCombo, int> OnComboComplete;
    public Action<ScriptableAttack> OnReceiveDamage;
    public Action<ScriptableAttack> OnMissAttack;
    public Action<ScriptableAttack> OnAttack;
    public Action<ScriptableAttack> OnPreAttack;
    public Action OnIdle;
    #endregion
}
