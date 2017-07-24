using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordKeyGenerator : MonoBehaviour
{
    [SerializeField]
    private float _nextPasswordKeyTime;

    private MyNetworkManager _networkManager;

    private int _passwordKey;
    private long _lastTime = 0;

    private void Awake()
    {
        _networkManager = GameObject.Find("Network Manager").GetComponent<MyNetworkManager>();
        UnityEngine.Assertions.Assert.IsNotNull(_networkManager, "NetworkManager is null");
    }

    private void Start()
    {
        _lastTime = CurrentMillis.Millis;
  
        GeneratePasswordKey();
        _networkManager.SetPasswordKeyText(_passwordKey.ToString());

        StartCoroutine(PasswordKeyGeneratorLoop(_nextPasswordKeyTime));
    }

    /// <summary>Class to get current timestamp with enough precision</summary>
    private static class CurrentMillis
    {
        private static readonly System.DateTime Jan1St1970 = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        /// <summary>Get extra long current timestamp</summary>
        public static long Millis { get { return (long)((System.DateTime.UtcNow - Jan1St1970).TotalMilliseconds); } }
    }

    private IEnumerator PasswordKeyGeneratorLoop(float nextPasswordKeyTime)
    {
        //float lastPasswordKeyTime = 0;
        long lastPasswordKeyTime = CurrentMillis.Millis;
        long triggerTime = (long)(nextPasswordKeyTime * 1000);

        while (true)
        {
            long time = CurrentMillis.Millis;
            long timeDif = time - lastPasswordKeyTime;

            //if (lastPasswordKeyTime + nextPasswordKeyTime <= Time.time) {
            //lastPasswordKeyTime = Time.time;
            if (timeDif >= triggerTime)
            {
                lastPasswordKeyTime = time;
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
