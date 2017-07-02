using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MyNetworkManager : NetworkManager
{
    private GameObject _waitUI;
    private GameObject _serverWaitUI;
    private GameObject _serverUI;
    private GameObject _connectionUI;
    private GameObject _gameUI;

    private ServerWaitUIController _serverWaitUIController;

    private List<NetworkConnection> _currentPlayers = new List<NetworkConnection>();
    private List<NetworkConnection> _readyPlayers = new List<NetworkConnection>();

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        _waitUI.SetActive(false);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        _waitUI.SetActive(true);
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

        _serverWaitUIController.ChangeWaitForConnectionsTextTo(2 - _readyPlayers.Count);

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

            _serverWaitUIController.ChangeWaitForConnectionsTextTo(2 - _readyPlayers.Count);

            if (_readyPlayers.Count >= 2)
            {
                StartServerGame();
            }
        }
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        _waitUI.SetActive(false);
        _serverWaitUI.SetActive(true);

        _serverWaitUIController.ChangeWaitForConnectionsTextTo(2);
    }

    public void SetPasswordKeyText(string passwordKey)
    {
        if (_serverWaitUIController != null)
            _serverWaitUIController.SetPasswordKeyText(passwordKey);
    }

    private void StartServerGame()
    {
        _serverWaitUI.SetActive(false);

        GameObject serverUI = GameObject.FindGameObjectWithTag("ServerUI");
        if (serverUI != null) serverUI.SetActive(true);

        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }

    private void StopServerGame()
    {
        _serverWaitUI.SetActive(true);

        GameObject serverUI = GameObject.FindGameObjectWithTag("ServerUI");
        if (serverUI != null) serverUI.SetActive(false);

        SceneManager.UnloadSceneAsync(1);
    }

    private void Awake()
    {
        _serverWaitUI = GameObject.FindGameObjectWithTag("ServerWaitUI");
        _waitUI = GameObject.FindGameObjectWithTag("WaitUI");

        _serverWaitUIController = _serverWaitUI.GetComponent<ServerWaitUIController>();
    }

    private void Start()
    {
        _waitUI.SetActive(true);
        _serverWaitUI.SetActive(false);
    }

    private void Update()
    {
        bool noConnection = (client == null || client.connection == null ||
                                 client.connection.connectionId == -1);

        if (noConnection)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                StartHost();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                StartClient();
            }
        }
    }
}
