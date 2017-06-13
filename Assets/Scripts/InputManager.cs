using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public string verticalAxis;
    public string horizontalAxis;

    public SingleInputType currentInputPlayer1 = SingleInputType.None;
    public SingleInputType currentInputPlayer2 = SingleInputType.None;

    public enum SingleInputType
    {
        None,   //none
        Jump,   //up 
        Shrink, //down
        Dash,   //right
        Wait,   //left
    }

    public enum CombinedInputType
    {
        None,           //none + none
        Jump,           //up + none
        Shrink,         //down + none
        Dash,           //right + none
        Wait,           //left + none

        DoubleJump,     //up + up
        DoubleShrink,   //down + down
        DoubleDash,     //right + right
        Invulnerable,   //left + left

        ShrinkJump,     //up + down
        DashJump,       //up + right
        HighJump,       //up + left
        ShrinkDash,     //down + right
        WaitAndShrink,  //down + left
        ChargedDash,    //left + right
    }
    
    public void UpdatePlayerInput (SingleInputType input, int playerNumber)
    {
        switch (playerNumber)
        {
            case 1:
                currentInputPlayer1 = input;
                break;
            case 2:
                currentInputPlayer2 = input;
                break;
            default:
                Debug.LogError("Coul not set player input.");
                break;
        }
    }

    public void ResetPlayerInputs ()
    {
        currentInputPlayer1 = SingleInputType.None;
        currentInputPlayer2 = SingleInputType.None;
    }

    public CombinedInputType getCurrentInput()
    {
        switch (currentInputPlayer1)
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
                    case SingleInputType.Dash:
                        return CombinedInputType.Dash;
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
                    case SingleInputType.Dash:
                        return CombinedInputType.DashJump;
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
                    case SingleInputType.Dash:
                        return CombinedInputType.ShrinkDash;
                    case SingleInputType.Wait:
                        return CombinedInputType.WaitAndShrink;
                }
                break;
            case SingleInputType.Dash:
                switch (currentInputPlayer1)
                {
                    case SingleInputType.None:
                        return CombinedInputType.Dash;
                    case SingleInputType.Jump:
                        return CombinedInputType.DashJump;
                    case SingleInputType.Shrink:
                        return CombinedInputType.ShrinkDash;
                    case SingleInputType.Dash:
                        return CombinedInputType.DoubleDash;
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
                    case SingleInputType.Dash:
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
