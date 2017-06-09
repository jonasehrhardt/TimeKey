using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TestButton : NetworkBehaviour
{
    public Transform ball;

    public void Click()
    {
        Debug.Log("button clicked");

        var but = GameObject.Find("Button");
        var button = but.GetComponent<Button>();
        ColorBlock colors = button.colors;
        colors.highlightedColor = GetComponent<Button>().colors.highlightedColor == Color.red ? Color.white : Color.red;
        button.colors = colors;

        NetworkServer.Spawn(Instantiate(ball, null).gameObject);
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
