using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public string verticalAxis;
    public string horizontalAxis;

    public SingleInputType currentInputPlayer0 = SingleInputType.None;
    public SingleInputType currentInputPlayer1 = SingleInputType.None;

    private bool inputIsEnabled = false;

    public enum SingleInputType
    {
        None,   //none
        Jump,   //up 
        Shrink, //down
        Smash,   //right
        Wait,   //left
    }

    public enum CombinedInputType
    {
        None,           //none + none
        Jump,           //up + none
        Shrink,         //down + none
        Smash,           //right + none
        Wait,           //left + none

        DoubleJump,     //up + up
        DoubleShrink,   //down + down
        DoubleSmash,     //right + right
        Invulnerable,   //left + left

        ShrinkJump,     //up + down
        SmashJump,       //up + right
        HighJump,       //up + left
        ShrinkSmash,     //down + right
        WaitAndShrink,  //down + left
        ChargedDash,    //left + right
    }
    
    public void UpdatePlayerInput (SingleInputType input, int playerNumber)
    {
        if (inputIsEnabled)
        {
            switch (playerNumber)
            {
                case 0:
                    currentInputPlayer0 = input;
                    break;
                case 1:
                    currentInputPlayer1 = input;
                    break;
                default:
                    Debug.LogError("Could not set player input.");
                    break;
            }
        }
    }

    public void ResetPlayerInputs ()
    {
        currentInputPlayer0 = SingleInputType.None;
        currentInputPlayer1 = SingleInputType.None;
    }

    public void EnablePlayerInput()
    {
        inputIsEnabled = true;
    }

    public void DisablePlayerInput()
    {
        inputIsEnabled = false;
    }

    public CombinedInputType getCurrentInput()
    {
        //Debug.Log("Player 0: " + currentInputPlayer0);
        //Debug.Log("Player 1: " + currentInputPlayer1);
        
        switch (currentInputPlayer0)
        {
            case SingleInputType.None:
                switch (currentInputPlayer1)
                {
                    case SingleInputType.None:
                        return CombinedInputType.None;
                    case SingleInputType.Jump:
                        return CombinedInputType.Jump;
                    case SingleInputType.Shrink:
                        return CombinedInputType.Shrink;
                    case SingleInputType.Smash:
                        return CombinedInputType.Smash;
                    case SingleInputType.Wait:
                        return CombinedInputType.Wait;
                }
            break;
            case SingleInputType.Jump:
                switch (currentInputPlayer1)
                {
                    case SingleInputType.None:
                        return CombinedInputType.Jump;
                    case SingleInputType.Jump:
                        return CombinedInputType.DoubleJump;
                    case SingleInputType.Shrink:
                        return CombinedInputType.ShrinkJump;
                    case SingleInputType.Smash:
                        return CombinedInputType.SmashJump;
                    case SingleInputType.Wait:
                        return CombinedInputType.HighJump;
                }
                break;
            case SingleInputType.Shrink:
                switch (currentInputPlayer1)
                {
                    case SingleInputType.None:
                        return CombinedInputType.Shrink;
                    case SingleInputType.Jump:
                        return CombinedInputType.ShrinkJump;
                    case SingleInputType.Shrink:
                        return CombinedInputType.DoubleShrink;
                    case SingleInputType.Smash:
                        return CombinedInputType.ShrinkSmash;
                    case SingleInputType.Wait:
                        return CombinedInputType.WaitAndShrink;
                }
                break;
            case SingleInputType.Smash:
                switch (currentInputPlayer1)
                {
                    case SingleInputType.None:
                        return CombinedInputType.Smash;
                    case SingleInputType.Jump:
                        return CombinedInputType.SmashJump;
                    case SingleInputType.Shrink:
                        return CombinedInputType.ShrinkSmash;
                    case SingleInputType.Smash:
                        return CombinedInputType.DoubleSmash;
                    case SingleInputType.Wait:
                        return CombinedInputType.ChargedDash;
                }
                break;
            case SingleInputType.Wait:
                switch (currentInputPlayer1)
                {
                    case SingleInputType.None:
                        return CombinedInputType.Wait;
                    case SingleInputType.Jump:
                        return CombinedInputType.HighJump;
                    case SingleInputType.Shrink:
                        return CombinedInputType.WaitAndShrink;
                    case SingleInputType.Smash:
                        return CombinedInputType.ChargedDash;
                    case SingleInputType.Wait:
                        return CombinedInputType.Invulnerable;
                }
                break;
        }
        Debug.LogError("Could not retreive CombinedInputType. Set to None.");
        return CombinedInputType.None;
    }
}
