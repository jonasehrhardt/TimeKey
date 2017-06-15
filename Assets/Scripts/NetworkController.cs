using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkManager))]
public class NetworkController : MonoBehaviour
{
    public PasswordKeyGenerator passwordKeyGenerator;
    private NetworkManager manager;

    void Awake()
    {
        manager = GetComponent<NetworkManager>();

        UnityEngine.Assertions.Assert.IsNotNull(passwordKeyGenerator, "PasswordKeyGenerator is null");
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            manager.matchMaker.CreateMatch(manager.matchName, 3, true, passwordKeyGenerator.PasswordKey.ToString(), "", "", 0, 0, manager.OnMatchCreate);
        }
    }

    public void Connect()
    {
        string passwordKey = passwordKeyGenerator.PasswordKey.ToString();

        if (!manager.IsClientConnected() && !NetworkServer.active && manager.matchMaker == null)
        {
            manager.matchMaker.JoinMatch(new UnityEngine.Networking.Types.NetworkID(), passwordKey, "", "", 0, 0, manager.OnMatchJoined);
            //manager.StartClient();
        }
    }
}
