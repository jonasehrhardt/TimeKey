using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject _waitUI;
    [SerializeField]
    private GameObject _serverWaitUI;
    [SerializeField]
    private GameObject _serverGameUI;
    [SerializeField]
    private GameObject _background;
    [SerializeField]
    private GameObject _smallBackground;

    private MyNetworkManager _networkManager;
    private PasswordKeyGenerator _passwordKeyGenerator;

    private ServerWaitUIController _serverWaitUIController;
    private ServerGameUIController _serverGameUIController;

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

    public void ShowServerWaitUI()
    {
        if (_serverWaitUI != null)
            _serverWaitUI.SetActive(true);

        if (_background != null)
            _background.SetActive(true);

        if (_smallBackground != null)
            _smallBackground.SetActive(false);
    }

    public void HideServerWaitUI()
    {
        if (_serverWaitUI != null)
            _serverWaitUI.SetActive(false);


        if (_background != null)
            _background.SetActive(false);

        if (_smallBackground != null)
            _smallBackground.SetActive(false);
    }

    public void ShowServerGameUI()
    {
        if (_serverGameUI != null)
            _serverGameUI.SetActive(true);


        if (_background != null)
            _background.SetActive(false);

        if (_smallBackground != null)
            _smallBackground.SetActive(true);
    }

    public void HideServerGameUI()
    {
        if (_serverGameUI != null)
            _serverGameUI.SetActive(false);


        if (_background != null)
            _background.SetActive(false);

        if (_smallBackground != null)
            _smallBackground.SetActive(false);
    }

    public void SetPasswordKeyText(string passwordKey)
    {
        if (_serverWaitUIController != null)
            _serverWaitUIController.SetPasswordKeyText(passwordKey);
    }

    public void ChangeWaitForConnectionsTextTo(int waitForConnectionsCount)
    {
        if (_serverWaitUIController != null)
            _serverWaitUIController.ChangeWaitForConnectionsTextTo(waitForConnectionsCount);
    }

    public void SetHighscore(int highscore)
    {
        if (_serverWaitUIController != null)
            _serverWaitUIController.SetHighscore(highscore);
    }

    public void ChangeGamePointsText(int points)
    {
        if (_serverGameUIController != null)
            _serverGameUIController.ChangeGamePointsText(points);
    }

    public void ChangeGameTimeText(float time)
    {
        if (_serverGameUIController != null)
            _serverGameUIController.ChangeGameTimeText(time);
    }

    public void ChangeButtonsPressedText(string firstButton, string secondButton)
    {
        if (_serverGameUIController != null)
            _serverGameUIController.ChangeButtonsPressedText(firstButton, secondButton);
    }

    internal void ShowWaitToPressAButtonText()
    {
        if (_serverGameUIController != null)
            _serverGameUIController.ShowWaitToPressAButtonText();
    }

    internal void ShowNoInputText()
    {
        if (_serverGameUIController != null)
            _serverGameUIController.ShowNoInputText();
    }

    public void ShowPressNowText()
    {
        if (_serverGameUIController != null)
            _serverGameUIController.ShowPressNowText();
    }

    void Awake()
    {
        if (_serverWaitUI != null)
            _serverWaitUIController = _serverWaitUI.GetComponent<ServerWaitUIController>();

        if (_serverGameUI != null)
            _serverGameUIController = _serverGameUI.GetComponent<ServerGameUIController>();
    }

    void Start()
    {
        
    }

	public int UIStatus()
	{
		if (_serverWaitUI.activeSelf)
			return 1;
		else if (_serverGameUI.activeSelf)
			return 2;
		else
			return 0;
	}
}
