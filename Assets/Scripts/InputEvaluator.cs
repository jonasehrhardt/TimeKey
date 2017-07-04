﻿using UnityEngine;

public class InputEvaluator : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {            
            InputManager.CombinedInputType currentInputType = GameManager.instance.inputManager.getCurrentInput();
            //Debug.Log(currentInputType);

            //Do Action depending on player input
            switch (currentInputType)
            {
                case InputManager.CombinedInputType.None:
                    //Debug.Log("...");
                    break;
                case InputManager.CombinedInputType.Jump:
                    GameManager.instance.pcharacter.Jump(false);
                    break;
                case InputManager.CombinedInputType.DoubleJump:
                    GameManager.instance.pcharacter.Jump(true);
                    break;
                case InputManager.CombinedInputType.Shrink:
                    GameManager.instance.pcharacter.Shrink(false);
                    break;
                case InputManager.CombinedInputType.DoubleShrink:
                    GameManager.instance.pcharacter.Shrink(true);
                    break;
                case InputManager.CombinedInputType.ShrinkJump:
                    GameManager.instance.pcharacter.Shrink(false);
                    GameManager.instance.pcharacter.Jump(false);
                    break;
				case InputManager.CombinedInputType.Smash:
					GameManager.instance.pcharacter.Smash (false);
					break;
				case InputManager.CombinedInputType.DoubleSmash:
					GameManager.instance.pcharacter.Smash (true);
					break;
				case InputManager.CombinedInputType.SmashJump:
					GameManager.instance.pcharacter.Smash(false);
					GameManager.instance.pcharacter.Jump(false);
					break;
				case InputManager.CombinedInputType.ShrinkSmash:
					GameManager.instance.pcharacter.Smash (false);
					GameManager.instance.pcharacter.Shrink (false);
					break;
            }
        }
    }
}
