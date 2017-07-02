using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordKeyGenerator : MonoBehaviour
{
    [SerializeField]
    private float _nextPasswordKeyTime;

    private MyNetworkManager _networkManager;

    private int _passwordKey;

    private void Awake()
    {
        _networkManager = GameObject.Find("Network Manager").GetComponent<MyNetworkManager>();
        UnityEngine.Assertions.Assert.IsNotNull(_networkManager, "NetworkManager is null");
    }

    private void Start()
    {
        GeneratePasswordKey();
        _networkManager.SetPasswordKeyText(_passwordKey.ToString());

        StartCoroutine(PasswordKeyGeneratorLoop(_nextPasswordKeyTime));
    }

    private IEnumerator PasswordKeyGeneratorLoop(float nextPasswordKeyTime)
    {
        float lastPasswordKeyTime = 0;

        while (true)
        {
            if (lastPasswordKeyTime + nextPasswordKeyTime <= Time.time)
            {
                lastPasswordKeyTime = Time.time;
                GeneratePasswordKey();
                _networkManager.SetPasswordKeyText(_passwordKey.ToString());
                //ShowPasswordKey();
            }

            yield return false;
        }
    }

    //private void ShowPasswordKey()
    //{
    //    Debug.Log(_passwordKey);
    //}

    private void GeneratePasswordKey()
    {
        _passwordKey = Random.Range(1000, 10000);
    }

    internal bool IsPasswordKeyCorrect(int checkPasswordKey)
    {
        return _passwordKey == checkPasswordKey;
    }
    internal bool IsPasswordKeyCorrect(string checkPasswordKey)
    {
        int passwordKey;
        if (int.TryParse(checkPasswordKey, out passwordKey) == false)
            return false;

        return _passwordKey == passwordKey;
    }
}
