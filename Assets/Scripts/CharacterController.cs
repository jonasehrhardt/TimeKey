using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float movementSpeed = 0.1f;
    private Vector3 forwardMovement;

    public float jumpForce = 1f;
    private Rigidbody rigidBody;
	
	void Start ()
    {
        forwardMovement = new Vector3(movementSpeed, 0, 0);
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Jump"))
        {            
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
	}

    private void FixedUpdate()
    {
        transform.Translate(forwardMovement);
    }
}
