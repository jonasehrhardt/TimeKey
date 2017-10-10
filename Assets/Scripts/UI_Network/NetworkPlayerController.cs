using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerController : NetworkBehaviour
{
    private MyNetworkManager _networkManager;
    private PasswordKeyGenerator _passwordKeyGenerator;

    private ClientUIController _clientUIController;

    private int _playerNumber;

    public void ProvePasswordKey(string passwordKey)
    {
        if (isLocalPlayer && !isServer && isClient)
        {
            CmdProvePasswordKey(passwordKey);
        }
    }

    public void StartClientGame()
    {
        if (isServer)
        {
            Debug.Log("send StartClientGame");
            TargetStartClientGame(connectionToClient);
        }
    }

    public void StopClientGame()
    {
        if (isServer)
        {
            RpcStopClientGame();
        }
    }

    public void SetPlayerInput(InputManager.SingleInputType type)
    {
        if (isLocalPlayer && !isServer && isClient)
        {
            CmdSetPlayerInput(_playerNumber, type);
        }
    }

    public void SetSkipTutorial()
    {
        Debug.Log("skipppp");
        if (isLocalPlayer && !isServer && isClient)
        {
            Debug.Log("skip");

            CmdSetSkipTutorial(_playerNumber);
        }
    }

    public void SlowMoFieldBegins()
    {
        if (isServer && (_networkManager.ReadyPlayerCount == 2))
        {
            TargetSlowMoFieldBegins(connectionToClient);
        }
    }

    public void SlowMoFieldOver()
    {
        if (isServer)
        {
            TargetSlowMoFieldOver(connectionToClient);
        }
    }

    [Command]
    public void CmdProvePasswordKey(string passwordKey)
    {
        if (isServer && _passwordKeyGenerator.IsPasswordKeyCorrect(passwordKey))
        {
            TargetPasswordKeyIsCorrect(connectionToClient, _networkManager.ReadyPlayerCount);

            _networkManager.ClientIsReady(connectionToClient);
        }
    }

    [TargetRpc]
    public void TargetPasswordKeyIsCorrect(NetworkConnection conn, int playerNumber)
    {
        _playerNumber = playerNumber;

        if (_clientUIController == null)
        {
            GameObject clientUI = GameObject.FindGameObjectWithTag("ClientUI");
            if (clientUI != null)
                _clientUIController = clientUI.GetComponent<ClientUIController>();
        }

        if (_clientUIController != null)
        {
            _clientUIController.HideClientConnectionUI();
            _clientUIController.ShowClientWaitUI();
        }
    }

    [TargetRpc]
    public void TargetStartClientGame(NetworkConnection conn)
    {
        if (_clientUIController != null)
        {
            _clientUIController.HideClientWaitUI();
            _clientUIController.ShowClientGameUI();

            _clientUIController.DisableAllButtons();
        }

    }

    [TargetRpc]
    public void TargetSlowMoFieldOver(NetworkConnection conn)
    {
        if (_clientUIController != null)
        {
            _clientUIController.DisableAllButtons();
        }
    }

    [TargetRpc]
    public void TargetSlowMoFieldBegins(NetworkConnection conn)
    {
        if (_clientUIController != null)
        {
            _clientUIController.InteractivateAllButtons();
#if UNITY_ANDROID
            Handheld.Vibrate();
#endif
        }
    }

    [ClientRpc]
    public void RpcStopClientGame()
    {
        if (_clientUIController != null)
        {
            _clientUIController.HideClientGameUI();
            _clientUIController.ShowClientWaitUI();
        }
    }

    [Command]
    public void CmdSetPlayerInput(int playerNumber, InputManager.SingleInputType type)
    {
        Debug.Log("CmdSetPlayerInput: " + type.ToString());

        var playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            var passiveCharController = playerObject.GetComponent<PassiveCharacterController>();
            if (passiveCharController != null)
            {
                passiveCharController.SetInputType(playerNumber, type);
            }
        }
    }

    public void DisableSkipTutorialButton()
    {
        TargetDisableSkipTutorialButton(connectionToClient);
    }

    [Command]
    public void CmdSetSkipTutorial(int playerNumber)
    {
        if (_networkManager.ClientWantToSkipTutorial(connectionToClient))
        {
            foreach (var networkPlayer in GameObject.FindGameObjectsWithTag("NetworkPlayer"))
            {
                networkPlayer.GetComponent<NetworkPlayerController>().DisableSkipTutorialButton();
            }
        }
    }

    [TargetRpc]
    public void TargetDisableSkipTutorialButton(NetworkConnection conn)
    {
        Debug.Log("!!skip button off");
        if (_clientUIController != null)
        {
            Debug.Log("skip button off");
            _clientUIController.DisableSkipTutorialButton();
        }
    }

    private void Awake()
    {
        var networkManagerObject = GameObject.Find("Network Manager");
        if (networkManagerObject != null)
            _networkManager = networkManagerObject.GetComponent<MyNetworkManager>();

        var passwordGeneratorObject = GameObject.Find("PasswordKeyGenerator");
        if (passwordGeneratorObject != null)
            _passwordKeyGenerator = passwordGeneratorObject.GetComponent<PasswordKeyGenerator>();
    }
}
