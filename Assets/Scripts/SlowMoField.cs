﻿using UnityEngine;

public class SlowMoField : MonoBehaviour
{
    public InputManager.SingleInputType Type1;
    public InputManager.SingleInputType Type2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.pcharacter.TriggerEnter(Type1, Type2);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.pcharacter.TriggerExit(Type1, Type2);
        }
    }
}
