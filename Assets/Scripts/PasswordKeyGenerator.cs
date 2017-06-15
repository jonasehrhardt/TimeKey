using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordKeyGenerator : MonoBehaviour
{
    private int _passwordKey;

    internal int PasswordKey { get { return _passwordKey; } }

    private void Start()
    {
        StartCoroutine(PasswordKeyGeneratorLoop(3));
    }

    private IEnumerator PasswordKeyGeneratorLoop(float nextPasswordKeyTime)
    {
        float lastPasswordKeyTime = Time.time;
        GeneratePasswordKey();
        ShowPasswordKey();

        while (true)
        {
            if (lastPasswordKeyTime + nextPasswordKeyTime <= Time.time)
            {
                lastPasswordKeyTime = Time.time;
                GeneratePasswordKey();
                ShowPasswordKey();
            }

            yield return false;
        }
    }

    private void ShowPasswordKey()
    {
        Debug.Log(_passwordKey);
    }

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
