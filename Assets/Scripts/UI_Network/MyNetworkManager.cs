using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MyNetworkManager : NetworkManager
{
    [SerializeField]
    private bool _isHost = false;

    private ServerUIController _serverUIController;
    private ClientUIController _clientUIController;
    private PassiveCharacterController _passiveCharacterController;

    private List<NetworkConnection> _currentPlayers = new List<NetworkConnection>();
    private List<NetworkConnection> _readyPlayers = new List<NetworkConnection>();

    public int ReadyPlayerCount { get { return _readyPlayers.Count; } }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        if (_clientUIController != null)
        {
            _clientUIController.HideWaitUI();
            _clientUIController.ShowClientConnectionUI();
        }
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        if (_clientUIController != null)
        {
            _clientUIController.HideClientGameUI();
            _clientUIController.HideClientConnectionUI();
            _clientUIController.HideClientWaitUI();
            _clientUIController.ShowWaitUI();
        }

        StartClient();
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);

        _currentPlayers.Add(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);

        _currentPlayers.Remove(conn);
        _readyPlayers.Remove(conn);

        if (_serverUIController != null)
            _serverUIController.ChangeWaitForConnectionsTextTo(2 - _readyPlayers.Count);

        if (_readyPlayers.Count < 2)
        {
            StopServerGame();
        }
    }

    public void ClientIsReady(NetworkConnection conn)
    {
        if (_currentPlayers.Exists(c => c.connectionId == conn.connectionId))
        {
            _readyPlayers.Add(conn);

            if (_serverUIController != null)
                _serverUIController.ChangeWaitForConnectionsTextTo(2 - _readyPlayers.Count);

            if (_readyPlayers.Count >= 2)
            {
                StartServerGame();
            }
        }
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        if (_serverUIController != null)
        {
            _serverUIController.HideWaitUI();
            _serverUIController.ShowServerWaitUI();
        }

        if (_serverUIController != null)
            _serverUIController.ChangeWaitForConnectionsTextTo(2);
    }

    public void SetPasswordKeyText(string passwordKey)
    {
        if (_serverUIController != null)
            _serverUIController.SetPasswordKeyText(passwordKey);
    }

    private void StartServerGame()
    {
        if (_serverUIController != null)
        {
            _serverUIController.HideServerWaitUI();
            _serverUIController.ShowServerGameUI();
        }

        if (_passiveCharacterController != null)
        {
            _passiveCharacterController.ResetLevel();
            _passiveCharacterController.useAI = false;
        }

        foreach (var networkPlayer in GameObject.FindGameObjectsWithTag("NetworkPlayer"))
        {
            if (_readyPlayers.Exists(n => n == networkPlayer.GetComponent<NetworkPlayerController>().connectionToClient))
            {
                networkPlayer.GetComponent<NetworkPlayerController>().StartClientGame();
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1 && _passiveCharacterController == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                _passiveCharacterController = player.GetComponent<PassiveCharacterController>();
        }

        if (scene.buildIndex == 1 && _passiveCharacterController != null && _readyPlayers.Count < 2)
            _passiveCharacterController.useAI = true;
        else if (scene.buildIndex == 1 && _passiveCharacterController != null && _readyPlayers.Count >= 2)
            _passiveCharacterController.useAI = false;
    }

    private void StopServerGame()
    {
        if (_serverUIController != null)
        {
            _serverUIController.ShowServerWaitUI();
            _serverUIController.HideServerGameUI();
        }

        if (_passiveCharacterController != null)
        {
            _passiveCharacterController.ResetLevel();
            _passiveCharacterController.useAI = true;
        }

        foreach (var networkPlayer in GameObject.FindGameObjectsWithTag("NetworkPlayer"))
        {
            networkPlayer.GetComponent<NetworkPlayerController>().StopClientGame();
        }
    }

    private void Awake()
    {
        GameObject serverUI = GameObject.FindGameObjectWithTag("ServerUI");
        if (serverUI != null)
            _serverUIController = serverUI.GetComponent<ServerUIController>();

        GameObject clientUI = GameObject.FindGameObjectWithTag("ClientUI");
        if (clientUI != null)
            _clientUIController = clientUI.GetComponent<ClientUIController>();
    }

    private void Start()
    {
        if (_serverUIController != null)
        {
            _serverUIController.ShowWaitUI();
            _serverUIController.HideServerWaitUI();
            _serverUIController.HideServerGameUI();
        }

        if (_clientUIController != null)
        {
            _clientUIController.ShowWaitUI();
            _clientUIController.HideClientWaitUI();
            _clientUIController.HideClientConnectionUI();
            _clientUIController.HideClientGameUI();
        }

        if (_isHost)
        {
            StartHost();

            SceneManager.sceneLoaded += OnSceneLoaded;

            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        }
        else
            StartClient();
    }

    private void Update()
    {
        //bool noConnection = (client == null || client.connection == null ||
        //                         client.connection.connectionId == -1);

        //if (noConnection)
        //{
        //    if (Input.GetKeyDown(KeyCode.H))
        //    {
        //        StartHost();
        //    }

        //    if (Input.GetKeyDown(KeyCode.C))
        //    {
        //        StartClient();
        //    }
        //}
    }
}
