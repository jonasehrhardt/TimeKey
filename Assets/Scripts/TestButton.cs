using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestButton : NetworkBehaviour
{
    public Transform ball;

    public void Click()
    {
        Debug.Log("button clicked");

        NetworkServer.Spawn(Instantiate(ball, null).gameObject);

        gameObject.SetActive(!gameObject.activeSelf);
    }

    [Command]
    void CmdClick()
    {
        Click();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
