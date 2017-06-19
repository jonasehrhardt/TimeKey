using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //singleton!

    public CharacterController character;
    public PassiveCharacterController pcharacter;
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
        var startingPosition = GameObject.Find("BallStartingPosition").transform.position;
        characterStartingPosition = startingPosition != null ? startingPosition : character.transform.position;
        character.transform.position = characterStartingPosition;

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
        characterRigidbody.velocity = Vector3.zero;
        characterRigidbody.angularVelocity = Vector3.zero;
    }
}
