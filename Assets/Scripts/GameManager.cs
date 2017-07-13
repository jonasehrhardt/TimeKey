using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //singleton!
        
    public PassiveCharacterController pcharacter;
    public InputManager inputManager;
    private Vector3 characterStartingPosition;
    private Rigidbody characterRigidbody;

    /*
    [Space(10)]
    public Text pointsText;
    private string prePointsText = "Points\n";
    [HideInInspector]
    public int currentPoints = 0;
    public Text timeText;
    private string preTimeText = "Time\n";
    */

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
        characterStartingPosition = startingPosition;
        pcharacter.transform.position = characterStartingPosition;

        characterRigidbody = pcharacter.GetComponent<Rigidbody>();
    }
		
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCharacter();
        }

        //update UI
        /*
        pointsText.text = prePointsText + currentPoints;
        timeText.text = preTimeText + Time.realtimeSinceStartup.ToString("0.0");
        */
    }

    void ResetCharacter()
    {
        pcharacter.transform.position = characterStartingPosition;
        characterRigidbody.velocity = Vector3.zero;
        characterRigidbody.angularVelocity = Vector3.zero;
        pcharacter.ResetSize();
    }

    public void addPointsForObstacleCompletion()
    {
        //TODO: Change points depending on speed?
        //currentPoints += 1;
    }
}
