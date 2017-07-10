using UnityEngine;

public class SlowMoField : MonoBehaviour
{
    static private PassiveCharacterController characterScript = null;
    public InputManager.SingleInputType Type1;
    public InputManager.SingleInputType Type2;

    void Start()
    {
        if (characterScript == null)
        {
            GameObject character = GameObject.Find("Character");
            characterScript = (PassiveCharacterController)character.GetComponent(typeof(PassiveCharacterController));
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            characterScript.TriggerEnter(Type1, Type2);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            characterScript.TriggerExit(Type1, Type2);
        }
    }
}
