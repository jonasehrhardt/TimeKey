using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerWaitUIController : MonoBehaviour
{
    [SerializeField]
    private Text _connectionsText;

    [SerializeField]
    private Text _passwordKeyText;

    [SerializeField]
    private Text _highscoreText;

    public void SetPasswordKeyText(string passwordKey)
    {
        _passwordKeyText.text = "Current Key\n <color=#ff0000ff>" + passwordKey + "</color>";
    }

    public void ChangeWaitForConnectionsTextTo(int waitForConnectionsCount)
    {
        _connectionsText.text = "Wait for <color=#ff0000ff>" + waitForConnectionsCount + "</color> \nconnections";
    }

    public void SetHighscore(int highscore)
    {
        _highscoreText.text = "Highscore\n<color=#ff0000ff>" + highscore + "</color>";
    }

    private void Start()
    {

    }
}
