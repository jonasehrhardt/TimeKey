using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //singleton!

    public CharacterController character;
    public InputManager inputManager;
    private Vector3 characterStartingPosition;
    private Rigidbody characterRigidbody;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
        }
    }
        
    void Start ()
    {
        inputManager = new InputManager();
        characterStartingPosition = character.transform.position;
        characterRigidbody = character.GetComponent<Rigidbody>();
    }
		
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCharacter();
        }
    }

    void ResetCharacter()
    {
        character.transform.position = characterStartingPosition;
        characterRigidbody.velocity = new Vector3(0f, 0f, 0f);
        characterRigidbody.angularVelocity = new Vector3(0f, 0f, 0f);
    }
}
