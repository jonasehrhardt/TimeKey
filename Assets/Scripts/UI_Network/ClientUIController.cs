using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject _waitUI;
    [SerializeField]
    private GameObject _clientWaitUI;
    [SerializeField]
    private GameObject _clientConnectionUI;
    [SerializeField]
    private GameObject _clientGameUI;

    public Text ConnectionPasswordText;
    public InputField ConnectionPasswordInputField;

    public void ShowWaitUI()
    {
        if (_waitUI != null)
            _waitUI.SetActive(true);
    }

    public void HideWaitUI()
    {
        if (_waitUI != null)
            _waitUI.SetActive(false);
    }

    public void ShowClientWaitUI()
    {
        if (_clientWaitUI != null)
            _clientWaitUI.SetActive(true);
    }

    public void HideClientWaitUI()
    {
        if (_clientWaitUI != null)
            _clientWaitUI.SetActive(false);
    }

    public void ShowClientGameUI()
    {
        if (_clientGameUI != null)
            _clientGameUI.SetActive(true);
    }

    public void HideClientGameUI()
    {
        if (_clientGameUI != null)
            _clientGameUI.SetActive(false);
    }

    public void ShowClientConnectionUI()
    {
        if (_clientConnectionUI != null)
        {
            _clientConnectionUI.SetActive(true);
            ConnectionPasswordInputField.Select();
        }
    }

    public void HideClientConnectionUI()
    {
        if (_clientConnectionUI != null)
            _clientConnectionUI.SetActive(false);

        if (ConnectionPasswordText != null)
            ConnectionPasswordText.text = string.Empty;
    }

    public void AddButtonsToGameUI(params string[] buttonNames)
    {
        if (_clientGameUI != null)
            _clientGameUI.GetComponent<ClientGameUIController>().AddButtons(buttonNames);
    }

    public void RemoveAllButtonsFromGameUI()
    {
        if (_clientGameUI != null)
            _clientGameUI.GetComponent<ClientGameUIController>().RemoveAllButtons();
    }

    public void ProvePasswordKey()
    {
        foreach (var networkPlayer in GameObject.FindGameObjectsWithTag("NetworkPlayer"))
        {
            networkPlayer.GetComponent<NetworkPlayerController>().ProvePasswordKey(ConnectionPasswordText.text);
        }
    }

    public void ButtonInput(Text buttonText)
    {
        if (buttonText != null)
        {
            InputManager.SingleInputType inputType;

            switch (buttonText.text)
            {
                case "Jump":
                    inputType = InputManager.SingleInputType.Jump;
                    break;
                case "Shrink":
                    inputType = InputManager.SingleInputType.Shrink;
                    break;
                case "Smash":
                    inputType = InputManager.SingleInputType.Smash;
                    break;
                case "Dash":
                    inputType = InputManager.SingleInputType.Dash;
                    break;
                default:
                    inputType = InputManager.SingleInputType.None;
                    break;
            }

            foreach (var networkPlayer in GameObject.FindGameObjectsWithTag("NetworkPlayer"))
            {
                networkPlayer.GetComponent<NetworkPlayerController>().SetPlayerInput(inputType);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ProvePasswordKey();
        }
    }
}
