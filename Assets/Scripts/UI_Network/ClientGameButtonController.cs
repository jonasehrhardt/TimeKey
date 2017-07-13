using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientGameButtonController : MonoBehaviour
{
    [SerializeField]
    private Text _buttonNameText;

    public void ChangeButtonName(string name)
    {
        _buttonNameText.text = name;
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
