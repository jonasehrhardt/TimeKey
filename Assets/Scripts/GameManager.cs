using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //singleton!

    public PassiveCharacterController pcharacter;
    public InputManager inputManager;
    private Vector3 characterStartingPosition;
    private Rigidbody characterRigidbody;

    [HideInInspector]
    public int currentPoints = 0;
    private float gameStartTime = 0;

    private ServerUIController _serverUIController;

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

    void Start()
    {
        inputManager = new InputManager();
        var startingPosition = GameObject.Find("BallStartingPosition").transform.position;
        characterStartingPosition = startingPosition != null ? startingPosition : pcharacter.transform.position;
        pcharacter.transform.position = characterStartingPosition;

        characterRigidbody = pcharacter.GetComponent<Rigidbody>();

        GameObject serverUIObject = GameObject.FindGameObjectWithTag("ServerUI");
        if (serverUIObject != null)
            _serverUIController = serverUIObject.GetComponent<ServerUIController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCharacter();
        }

        //update UI
        if (_serverUIController != null)
        {
            _serverUIController.ChangeGamePointsText(currentPoints);
            gameStartTime += Time.deltaTime;
            _serverUIController.ChangeGameTimeText(gameStartTime);
        }
    }

    public void ResetCharacter()
    {
        if (_serverUIController != null)
        {
            _serverUIController.ChangeGamePointsText(0);
            _serverUIController.ChangeGameTimeText(0);
        }

        pcharacter.transform.position = characterStartingPosition;
        characterRigidbody.velocity = Vector3.zero;
        characterRigidbody.angularVelocity = Vector3.zero;
        pcharacter.ResetSize();
        pcharacter.ResetSmash();
        pcharacter.ResetDash();
        inputManager.ResetPlayerInputs();
    }

    public void AddPointsForObstacleCompletion()
    {
        //TODO: Change points depending on speed?
        currentPoints += 1;

        if (Time.timeScale < 3)
            Time.timeScale += 0.01f;
    }

    public void UpdatePlayerStartingPosition(Vector3 newPosition)
    {
        characterStartingPosition = newPosition;
    }
}
