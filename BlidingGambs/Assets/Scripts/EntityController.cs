using System;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    #region Events
    public Action<int, AudioClip> OnGoodComboStep;
    public Action<ScriptableCombo> OnBadComboStep;
    public Action OnLose;
    public Action OnWin;
    public Action<ScriptableCombo> OnComboComplete;
    public Action<ScriptableAttack> OnReceiveDamage;
    public Action<ScriptableAttack> OnMissAttack;
    public Action<ScriptableAttack> OnAttack;
    public Action<ScriptableAttack> OnPreAttack;
    public Action OnIdle;
    #endregion
}
