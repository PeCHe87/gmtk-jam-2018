using System;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    #region Events
    public Action<int, AudioClip> OnGoodComboStep;
    public Action OnBadComboStep;
    public Action OnLoose;
    public Action OnWin;
    public Action<ScriptableCombo> OnComboComplete;
    #endregion
}
