using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerGameUIController : MonoBehaviour
{
    [SerializeField]
    private Text _pointsText;
    [SerializeField]
    private Text _timeText;
    [SerializeField]
    private Text _buttonsPressedText;

    private string prePointsText = "Points\n";
    private string preTimeText = "Time\n";
    private string preButtonsPressedText = "Buttons\n";

    public void ChangeGamePointsText(int points)
    {
        if (_pointsText != null)
            _pointsText.text = prePointsText + points;
    }

    public void ChangeGameTimeText(float time)
    {
        if (_timeText != null)
            _timeText.text = prePointsText + time.ToString("0.0");
    }

    public void ChangeButtonsPressedText(string firstButton, string secondButton)
    {
        if (_buttonsPressedText != null)
        {
            if (!string.IsNullOrEmpty(firstButton))
                _buttonsPressedText.text = preButtonsPressedText + firstButton + (!string.IsNullOrEmpty(secondButton) ? "+" + secondButton : string.Empty);
            else if(!string.IsNullOrEmpty(secondButton))
            {
                _buttonsPressedText.text = preButtonsPressedText + secondButton;
            }
        }
    }

    internal void ShowWaitToPressAButtonText()
    {
        if (_buttonsPressedText != null)
        {
            _buttonsPressedText.text = "Wait!\n";
        }
    }

    internal void ShowNoInputText()
    {
        if (_buttonsPressedText != null)
        {
            _buttonsPressedText.text = "No input!\n";
        }
    }

    internal void ShowPressNowText()
    {
        if (_buttonsPressedText != null)
        {
            _buttonsPressedText.text = "Press a button!\n";
        }
    }
}
