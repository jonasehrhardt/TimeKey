using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : MonoBehaviour {

	public void ToggleActivity()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
