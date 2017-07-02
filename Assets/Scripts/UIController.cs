using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIController : NetworkBehaviour
{
    private MyNetworkManager _networkManager;
    private PasswordKeyGenerator _passwordKeyGenerator;

    [SerializeField]
    private Text _debugText;

    public GameObject ServerUI;
    public GameObject ConnectionUI;
    public GameObject GameUI;

    public Text PasswordText;
    public InputField PasswordInputField;

    void Awake()
    {
        _networkManager = GameObject.Find("Network Manager").GetComponent<MyNetworkManager>();
        UnityEngine.Assertions.Assert.IsNotNull(_networkManager, "NetworkManager is null");

        _passwordKeyGenerator = GameObject.Find("PasswordKeyGenerator").GetComponent<PasswordKeyGenerator>();
        UnityEngine.Assertions.Assert.IsNotNull(_passwordKeyGenerator, "PasswordKeyGenerator is null");
    }

    void Start()
    {
        if (!isLocalPlayer)
        {
            gameObject.SetActive(false);
            return;
        }

        ServerUI.tag = "ServerUI";
        ConnectionUI.tag = "ConnectionUI";
        GameUI.tag = "GameUI";

        ServerUI.SetActive(false);
        ConnectionUI.SetActive(!isServer);
        if (ConnectionUI.activeSelf)
            PasswordInputField.Select();
        GameUI.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ProvePasswordKey();
        }
    }

    public void ProvePasswordKey()
    {
        if (!isServer && isClient)
        {
            CmdProvePasswordKey(PasswordText.text);
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
        ConnectionUI.SetActive(false);
        GameUI.SetActive(true);
    }
}
