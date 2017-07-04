using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveCharacterController : MonoBehaviour
{
    Rigidbody rb;
    [Header("Speed")]
    public float characterSpeedNormal = 5f;
    public float characterSpeedSlowed = 2f;
    float currentCharacterSpeed;

    [Space(10)]
    [Header("Jump")]
    public float jumpForce = 5.5f;
    public float doubleJumpForce = 8f;

    [Space(10)]
    [Header("Shrink")]
    public float normalScale = 1f;
    public float shrinkScale = 0.8f;
    public float doubleShrinkScale = 0.6f;

	private bool smashOn = false;


    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        currentCharacterSpeed = characterSpeedNormal;
		gameObject.GetComponent<TrailRenderer> ().enabled = false;
	}

    private void Update()
    {
        //JUMP
        if (Input.GetKey(KeyCode.W))
        {
            GameManager.instance.inputManager.UpdatePlayerInput(InputManager.SingleInputType.Jump, 0);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            GameManager.instance.inputManager.UpdatePlayerInput(InputManager.SingleInputType.Jump, 1);
        }

        //SHRINK
        if (Input.GetKey(KeyCode.S))
        {
            GameManager.instance.inputManager.UpdatePlayerInput(InputManager.SingleInputType.Shrink, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            GameManager.instance.inputManager.UpdatePlayerInput(InputManager.SingleInputType.Shrink, 1);
        }

		//SMASH
		if (Input.GetKey(KeyCode.D))
		{
			GameManager.instance.inputManager.UpdatePlayerInput(InputManager.SingleInputType.Smash, 0);
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			GameManager.instance.inputManager.UpdatePlayerInput(InputManager.SingleInputType.Smash, 1);
		}
    }

    void FixedUpdate ()
    {
        //Move character
        rb.velocity = new Vector3(currentCharacterSpeed, rb.velocity.y, rb.velocity.z);
	}

    public void SlowDown (bool slowed)
    {
        currentCharacterSpeed = slowed ? characterSpeedSlowed : characterSpeedNormal;
    }

    public void Jump (bool doubleJump)
    {
        var jumpFactor = doubleJump ? doubleJumpForce : jumpForce;

        rb.velocity = new Vector3(0, jumpFactor, 0);
    }

    public void Shrink (bool doubleShrink)
    {
        var scale = doubleShrink ? doubleShrinkScale : shrinkScale;
        transform.localScale = Vector3.one * scale;
    }

	public void Smash (bool doubleSmash)
	{
		GameManager.instance.pcharacter.GetComponent<TrailRenderer> ().enabled = true;
		smashOn = true;
	}

    public void ResetSize()
    {
        transform.localScale = Vector3.one * normalScale;
    }

	public void ResetSmash()
	{
		smashOn = false;
		GameManager.instance.pcharacter.GetComponent<TrailRenderer> ().enabled = false;
	}

	public void OnTriggerEnter(Collider other)
	{       
		//If the one colliding have the tag prey it
		//will get destroyed

		if(other.tag == "smash" && smashOn)
		{
			Destroy(other.gameObject);
		}
	}
}
