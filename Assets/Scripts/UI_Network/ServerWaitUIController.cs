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
        _passwordKeyText.text = "Current Key\n" + passwordKey;
    }

    public void ChangeWaitForConnectionsTextTo(int waitForConnectionsCount)
    {
        _connectionsText.text = "Wait for " + waitForConnectionsCount + "\nconnections";
    }

    private void Start()
    {

    }
}
