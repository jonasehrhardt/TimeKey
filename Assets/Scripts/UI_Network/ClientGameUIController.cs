using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientGameUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject _dummyButton;

    private List<GameObject> _buttons = new List<GameObject>();

    public class ButtonSettings
    {
        public string Name { get; private set; }
        public Color Color { get; private set; }

        public ButtonSettings(string name, Color color)
        {
            Name = name;
            Color = color;
        }
    }

    public void AddButtons(params ButtonSettings[] buttons)
    {
        foreach (var buttonSettings in buttons)
        {
            var button = Instantiate(_dummyButton);
            button.name = buttonSettings.Name;
            button.SetActive(true);
            button.transform.SetParent(_dummyButton.transform.parent);
            button.GetComponent<ClientGameButtonController>().ChangeButtonName(buttonSettings.Name);

            var colors = new ColorBlock()
            {
                normalColor = buttonSettings.Color,
                highlightedColor = Color.white,
                disabledColor = Color.grey,
                pressedColor = Color.white,
                colorMultiplier = 1
            };
            button.GetComponent<Button>().colors = colors;
            _buttons.Add(button);
        }
    }

    public void RemoveAllButtons()
    {
        foreach (var button in _buttons)
        {
            Destroy(button);
        }

        _buttons.Clear();
    }


    void Start()
    {
        _dummyButton.SetActive(false);

        AddButtons(new ButtonSettings("Jump", Color.white), new ButtonSettings("Shrink", Color.green), new ButtonSettings("Smash", Color.yellow), new ButtonSettings("Dash", Color.grey));
    }
}