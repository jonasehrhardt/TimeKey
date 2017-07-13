using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Transform slowMoField;

    [Tooltip("First input should be intended input")]
    public InputManager.CombinedInputType[] validInputs;
}