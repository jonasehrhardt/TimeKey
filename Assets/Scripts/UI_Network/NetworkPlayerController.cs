using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerController : NetworkBehaviour
{
    private MyNetworkManager _networkManager;
    private PasswordKeyGenerator _passwordKeyGenerator;

    private ClientUIController _clientUIController;

    public void ProvePasswordKey(string passwordKey)
    {
        if (isLocalPlayer && !isServer && isClient)
        {
            CmdProvePasswordKey(passwordKey);
        }
    }

    public void StartClientGame()
    {
        if (isLocalPlayer && isServer)
        {
            RpcStartClientGame();
        }
    }

    public void StopClientGame()
    {
        if (isLocalPlayer && isServer)
        {
            RpcStopClientGame();
        }
    }

    [Command]
    public void CmdProvePasswordKey(string passwordKey)
    {
        if (isServer && _passwordKeyGenerator.IsPasswordKeyCorrect(passwordKey))
        {
            TargetPasswordKeyIsCorrect(connectionToClient);

            _networkManager.ClientIsReady(connectionToClient);
        }
    }

    [TargetRpc]
    public void TargetPasswordKeyIsCorrect(NetworkConnection conn)
    {
        if (_clientUIController == null)
        {
            GameObject clientUI = GameObject.FindGameObjectWithTag("ClientUI");
            if (clientUI != null)
                _clientUIController = clientUI.GetComponent<ClientUIController>();
        }

        if (_clientUIController != null)
        {
            _clientUIController.HideClientConnectionUI();
            _clientUIController.HideClientWaitUI();
        }
    }

    [ClientRpc]
    public void RpcStartClientGame()
    {
        if (_clientUIController != null)
        {
            _clientUIController.HideClientWaitUI();
            _clientUIController.ShowClientGameUI();
        }

    }

    [ClientRpc]
    public void RpcStopClientGame()
    {
        // TODO hier checken. Ui wird nciht angezeigt!!

        if (_clientUIController != null)
        {
            _clientUIController.HideClientGameUI();
            _clientUIController.ShowClientWaitUI();
        }
    }

    private void Awake()
    {
        var networkManagerObject = GameObject.Find("Network Manager");
        if (networkManagerObject != null)
            _networkManager = networkManagerObject.GetComponent<MyNetworkManager>();
        UnityEngine.Assertions.Assert.IsNotNull(_networkManager, "NetworkManager is null");

        var passwordGeneratorObject = GameObject.Find("PasswordKeyGenerator");
        if (passwordGeneratorObject != null)
            _passwordKeyGenerator = passwordGeneratorObject.GetComponent<PasswordKeyGenerator>();
        UnityEngine.Assertions.Assert.IsNotNull(_passwordKeyGenerator, "PasswordKeyGenerator is null");
    }
}
