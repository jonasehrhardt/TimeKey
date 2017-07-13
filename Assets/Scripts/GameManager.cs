using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //singleton!
        
    public PassiveCharacterController pcharacter;
    public InputManager inputManager;
    private Vector3 characterStartingPosition;
    private Rigidbody characterRigidbody;

    [Space(10)]
    public Text pointsText;
    private string prePointsText = "Points\n";
    [HideInInspector]
    public int currentPoints = 0;
    public Text timeText;
    private string preTimeText = "Time\n";
    private float gameStartTime = 0;



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
        characterStartingPosition = startingPosition != null ? startingPosition : pcharacter.transform.position;
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
        pointsText.text = prePointsText + currentPoints;
        gameStartTime += Time.deltaTime;
        timeText.text = preTimeText + gameStartTime.ToString("0.0"); //Time.realtimeSinceStartup
    }

    public void ResetCharacter()
    {
        gameStartTime = 0;
        currentPoints = 0;
        pcharacter.transform.position = characterStartingPosition;
        characterRigidbody.velocity = Vector3.zero;
        characterRigidbody.angularVelocity = Vector3.zero;
        pcharacter.ResetSize();
    }

    public void addPointsForObstacleCompletion()
    {
        //TODO: Change points depending on speed?
        currentPoints += 1;
    }
}
