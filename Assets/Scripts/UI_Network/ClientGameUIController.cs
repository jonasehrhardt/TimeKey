using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientGameUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject _dummyButton;

    private List<GameObject> _buttons = new List<GameObject>();

    public void AddButtons(params string[] buttonNames)
    {
        foreach(string name in buttonNames)
        {
            var button = Instantiate(_dummyButton);
            button.SetActive(true);
            button.transform.parent = _dummyButton.transform.parent;
            button.GetComponent<ClientGameButtonController>().ChangeButtonName(name);
            _buttons.Add(button);
        }
    }

    public void RemoveAllButtons()
    {
        foreach(var button in _buttons)
        {
            Destroy(button);
        }

        _buttons.Clear();
    }


    void Start()
    {
        _dummyButton.SetActive(false);

        AddButtons("Jump", "Shrink", "Dash");
    }
}