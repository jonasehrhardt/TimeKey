using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveCharacterController : MonoBehaviour
{
    public bool useAI = false;
    public bool slowMotion = false;

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

    [Space(10)]
    [Header("Dash")]
    public float dashForce = 5.5f;
    public float doubleDashForce = 8f;
    private BreakScript breakscript;
    private float timeLeft = 5;

    [Space(10)]
    [Header("Smash")]
    private bool smashOn = false; 
    //Grafische Darstellung fuer Smash fehlt noch

    void Start ()
    {
        breakscript = GetComponent<BreakScript>();
        rb = GetComponent<Rigidbody>();
        currentCharacterSpeed = characterSpeedNormal;
		gameObject.GetComponent<TrailRenderer> ().enabled = false;
    }

    public void ResetLevel()
    {
        GameManager.instance.inputManager.ResetPlayerInputs();
        GameManager.instance.inputManager.DisablePlayerInput();
        GameManager.instance.ResetCharacter();

        if (slowMotion) GameManager.instance.pcharacter.SlowDown(false);
        ResetSmash();
        ResetDash();

        timeLeft = 3;
        breakscript.Revive();
    }

    private void Update()
    {
        //if (!breakscript.isAlive())
        //{
        //    timeLeft -= Time.deltaTime;
        //    if (timeLeft < 0) ResetLevel();
        //    return;
        //}

        if(this.transform.position.y < -6)
        {
            ResetLevel();
            return;
        }

        if (useAI) return;

        InputManager.SingleInputType type1 = InputManager.SingleInputType.None;
        InputManager.SingleInputType type2 = InputManager.SingleInputType.None;

        if (Input.GetKey(KeyCode.W)) type1 = InputManager.SingleInputType.Jump;
        if (Input.GetKey(KeyCode.S)) type1 = InputManager.SingleInputType.Shrink;
        if (Input.GetKey(KeyCode.A)) type1 = InputManager.SingleInputType.Smash;
        if (Input.GetKey(KeyCode.D)) type1 = InputManager.SingleInputType.Dash;

        if (Input.GetKey(KeyCode.UpArrow)) type2 = InputManager.SingleInputType.Jump;
        if (Input.GetKey(KeyCode.DownArrow)) type2 = InputManager.SingleInputType.Shrink;
        if (Input.GetKey(KeyCode.LeftArrow)) type2 = InputManager.SingleInputType.Smash;
        if (Input.GetKey(KeyCode.RightArrow)) type2 = InputManager.SingleInputType.Dash;

        InputManager mngr = GameManager.instance.inputManager;
        if (type1 != InputManager.SingleInputType.None) mngr.UpdatePlayerInput(type1, 0);
        if (type2 != InputManager.SingleInputType.None) mngr.UpdatePlayerInput(type2, 1);
    }

    public void SetInputType(int playerNumber, InputManager.SingleInputType type)
    {
        if (type != InputManager.SingleInputType.None)
            GameManager.instance.inputManager.UpdatePlayerInput(type, playerNumber);
    }

    void FixedUpdate ()
    {
        //if (!breakscript.isAlive()) return;
        //Move character
        rb.velocity = new Vector3(currentCharacterSpeed, rb.velocity.y, rb.velocity.z);
	}

    public void TriggerEnter(InputManager.SingleInputType type1, InputManager.SingleInputType type2)
    {
        //Enable Player Input
        GameManager.instance.inputManager.EnablePlayerInput();
        if(slowMotion) GameManager.instance.pcharacter.SlowDown(true);

        if (useAI) {
            InputManager mngr = GameManager.instance.inputManager;
            mngr.UpdatePlayerInput(type1, 0);
            mngr.UpdatePlayerInput(type2, 1);
        }
    }

    public void TriggerExit(InputManager.SingleInputType Type1, InputManager.SingleInputType Type2)
    {
        //Disable Player Input
        GameManager.instance.inputManager.DisablePlayerInput();
        if(slowMotion) GameManager.instance.pcharacter.SlowDown(false);
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
		
		smashOn = true;
        //doubleSmash?

    }

    public void Dash (bool doubleDash)
    {
        var dashFactor = doubleDash ? doubleDashForce : dashForce;
        currentCharacterSpeed += dashFactor;
        rb.velocity = new Vector3(0, 1.5f, 0);
        GameManager.instance.pcharacter.GetComponent<TrailRenderer>().enabled = true;
        //doubleDash?
    }

    public void ResetSize()
    {
        transform.localScale = Vector3.one * normalScale;
    }

	public void ResetSmash()
	{
		smashOn = false;
		
    }

    public void ResetDash()
    {
        currentCharacterSpeed = characterSpeedNormal;
        rb.velocity = Vector3.zero;
        GameManager.instance.pcharacter.GetComponent<TrailRenderer>().enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        //If the one colliding have the tag prey it
        //will get destroyed

        if (collision.collider.tag == "Smashable" && smashOn)
        {
            Destroy(collision.collider.gameObject);
        }

        if (collision.collider.tag == "Obstacle")
        {
            Vector3 direction = (collision.transform.position - transform.position).normalized;
            if ((direction.x > 0 || direction.y > 0) && rb.velocity.x <= currentCharacterSpeed/5)
            {
                breakscript.PlayDeath();
                rb.velocity = Vector3.zero;
            }
        }
    }
}
