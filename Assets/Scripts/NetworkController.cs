using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(NetworkManager))]
public class NetworkController : MonoBehaviour
{
    public PasswordKeyGenerator passwordKeyGenerator;

    public GameObject ConnectionUI;
    public GameObject GameUI;

    public Text PasswordText;

    private NetworkManager manager;

    void Awake()
    {
        manager = GetComponent<NetworkManager>();

        UnityEngine.Assertions.Assert.IsNotNull(passwordKeyGenerator, "PasswordKeyGenerator is null");
    }


    // Use this for initialization
    void Start()
    {
        GameUI.SetActive(false);

        manager.StartMatchMaker();
    }

    // Update is called once per frame
    void Update()
    {
        

        //MasterServer.RequestHostList("TimeKeyGame");
        //MasterServer.dedicatedServer = true;

        //Debug.Log(manager.matchMaker.ListMatches(0, 1, "TimeKeyGame", false, 0, 0, null));

        if (!Network.isServer && !Network.isClient  /*&& MasterServer.PollHostList().Length == 0 */ && Input.GetKeyDown(KeyCode.C))
        {
            ConnectionConfig conf = new ConnectionConfig();
            conf.
            manager.StartServer();
            //manager.matchMaker.CreateMatch("TimeKeyGame", 3, true, passwordKeyGenerator.PasswordKey.ToString(), "", "", 0, 0, null);
            //Network.InitializeServer(2, 7777, !Network.HavePublicAddress());
            //MasterServer.RegisterHost("TimeKeyGame", "TimeKey");

            ConnectionUI.SetActive(false);
            GameUI.SetActive(true);
        }

        if (Network.isServer)
            Network.incomingPassword = passwordKeyGenerator.PasswordKey.ToString();
        else if (!Network.isClient && MasterServer.PollHostList().Length > 0 && Input.GetKeyDown(KeyCode.C))
        {
            //var networkError = Network.Connect(MasterServer.PollHostList()[0], PasswordText.text);

            manager.StartClient();

            //Debug.Log(networkError);
            //manager.matchMaker.CreateMatch(manager.matchName, 3, true, passwordKeyGenerator.PasswordKey.ToString(), "", "", 0, 0, manager.OnMatchCreate
        }
    }

    public void Connect()
    {
        string passwordKey = passwordKeyGenerator.PasswordKey.ToString();

        if (!manager.IsClientConnected() && !NetworkServer.active && manager.matchMaker == null)
        {
            manager.matchMaker.JoinMatch(new UnityEngine.Networking.Types.NetworkID(), passwordKey, "", "", 0, 0, manager.OnMatchJoined);
        }
    }

    private void OnConnectedToServer()
    {
        ConnectionUI.SetActive(false);
        GameUI.SetActive(true);
    }

    private void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        GameUI.SetActive(false);
        ConnectionUI.SetActive(true);
    }
}
