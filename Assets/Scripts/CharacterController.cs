using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float movementSpeed = 0.1f;
    private Vector3 forwardMovement;

    public float jumpForce = 1f;
    public float dashForce = 1f;
    private Rigidbody rigidBody;

    private decimal pastTime = 0;
    Vector3 scale = new Vector3(1f,1f,1f);

    void Start ()
    {
        scale = this.gameObject.transform.localScale;
        pastTime = getTime();
        forwardMovement = new Vector3(movementSpeed, 0, 0);
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Transform trans = this.gameObject.transform;

        if (Input.GetButtonDown("Jump") || (Input.GetButtonDown("Vertical") &&  Input.GetAxisRaw("Vertical") > 0))
        {            
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
        {
            rigidBody.AddForce(Vector3.right * dashForce, ForceMode.Impulse);
        }

        if (Input.GetKey(KeyCode.RightControl)|| Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) trans.localScale = scale / 2;
        else if (trans.localScale.y < 1f) trans.localScale = scale;
    }

    decimal getTime()
    {
        return System.DateTime.Now.Ticks / (decimal)System.TimeSpan.TicksPerMillisecond;
    }

    bool tick(decimal millisec)
    {
        decimal current = getTime();
        if ((current - pastTime) >= millisec)
        {
            pastTime = getTime();
            return true;
        }
        return false;
    }

    private void FixedUpdate()
    {
        if (!Input.GetKey(KeyCode.A)) {
            if (rigidBody.velocity.x > 0 && rigidBody.velocity.x <= 5) rigidBody.AddForce(Vector3.right * 1f, ForceMode.Impulse);
            else rigidBody.AddForce(Vector3.right * 0.1f, ForceMode.Impulse);
        }
    }
}
