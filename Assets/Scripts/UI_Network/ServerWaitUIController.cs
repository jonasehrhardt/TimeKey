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

    public void SetPasswordKeyText(string passwordKey)
    {
        _passwordKeyText.text = "Current Key\n <color=#ff0000ff>" + passwordKey + "</color>";
    }

    public void ChangeWaitForConnectionsTextTo(int waitForConnectionsCount)
    {
        _connectionsText.text = "Wait for <color=#ff0000ff>" + waitForConnectionsCount + "</color> \nconnections";
    }

    private void Start()
    {

    }
}
