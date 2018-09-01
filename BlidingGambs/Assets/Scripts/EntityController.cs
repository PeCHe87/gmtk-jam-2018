using System;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    public Action<int> OnGoodComboStep;
    public Action OnBadComboStep;
    public Action OnLoose;
    public Action OnWin;
    public Action<ActionType.Type> OnComboComplete;
}
