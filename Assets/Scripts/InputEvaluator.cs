using UnityEngine;

public class InputEvaluator : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger Enter");
        if (other.tag == "Player")
        {            
            InputManager.CombinedInputType currentInputType = GameManager.instance.inputManager.getCurrentInput();
            //Debug.Log(currentInputType);

            GameManager.instance.ShowWaitToPressAButtonText();

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
                case InputManager.CombinedInputType.Dash:
                    GameManager.instance.pcharacter.Dash(false);
                    break;
                case InputManager.CombinedInputType.DoubleDash:
                    GameManager.instance.pcharacter.Dash(true);
                    break;
                case InputManager.CombinedInputType.DashJump:
                    GameManager.instance.pcharacter.Dash(false);
                    GameManager.instance.pcharacter.Jump(false);
                    break;
                case InputManager.CombinedInputType.ShrinkDash:
                    GameManager.instance.pcharacter.Dash(false);
                    GameManager.instance.pcharacter.Shrink(false);
                    break;
                case InputManager.CombinedInputType.SmashDash:
                    GameManager.instance.pcharacter.Smash(false);
                    GameManager.instance.pcharacter.Dash(false);
                    break;
            }
        }
    }
}
