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

    private string prePointsText = "Points\n";
    private string preTimeText = "Time\n";

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
}
