using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPlayerController : PlayerController {

    [SerializeField] private Image _dodgeUpCheck;
    [SerializeField] private Image _dodgeDownCheck;
    [SerializeField] private Button _buttonContinue;

    private void FixedUpdate()
    {

        if (!_dodgeUpCheck.IsActive() && performingCombo != null && performingCombo.keyAction == ActionType.Type.DODGE_UP)
            _dodgeUpCheck.enabled = true;

        if (!_dodgeDownCheck.IsActive() && performingCombo != null && performingCombo.keyAction == ActionType.Type.DODGE_DOWN)
            _dodgeDownCheck.enabled = true;

        if (_dodgeUpCheck.IsActive() && _dodgeDownCheck.IsActive())
        {
            _buttonContinue.interactable = true;
        }
    }
}
