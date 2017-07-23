using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //singleton!

    public PassiveCharacterController pcharacter;
    public InputManager inputManager;
    [HideInInspector]
    public Vector3 characterStartingPosition;
    private Rigidbody characterRigidbody;

    [HideInInspector]
    public int currentPoints = 0;
    private float gameStartTime = 0;

    private ServerUIController _serverUIController;

    [Header("Time")]
    [Range(1f, 10f)]
    [Tooltip("High values may result in unexpected behaviour")]
    public float minTimeScale = 1;
    [Range(1f, 10f)]
    public float maxTimeScale = 3;
    [Range(0.01f, 0.1f)]
    public float timeScaleIncrements = 0.02f;

    private void Awake()
    {
        //Debug.Log("Awakening GameManager");
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
        Time.timeScale = minTimeScale;

        //Debug.Log("Starting GameManager");
        inputManager = new InputManager();

        var startingPosition = GameObject.Find("BallStartingPosition");
        characterStartingPosition = (startingPosition != null) ? startingPosition.transform.position : pcharacter.transform.position;

        characterRigidbody = pcharacter.GetComponent<Rigidbody>();

        GameObject serverUIObject = GameObject.FindGameObjectWithTag("ServerUI");
        if (serverUIObject != null)
            _serverUIController = serverUIObject.GetComponent<ServerUIController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetLevel();
        }

        //update UI
        if (_serverUIController != null)
        {
            _serverUIController.ChangeGamePointsText(currentPoints);
            gameStartTime += Time.deltaTime;
            _serverUIController.ChangeGameTimeText(gameStartTime);
        }
    }

    public void ResetLevel()
    {
        if (_serverUIController != null)
        {
            _serverUIController.ChangeGamePointsText(0);
            _serverUIController.ChangeGameTimeText(0);
        }

        pcharacter.ResetCharacter();
        Time.timeScale = minTimeScale;
    }

    public void AddPointsForObstacleCompletion()
    {
        //TODO: Change points depending on speed?
        currentPoints += 1;

        if (Time.timeScale < maxTimeScale)
            Time.timeScale += 0.1f; // timeScaleIncrements;
    }

    public void UpdatePlayerStartingPosition(Vector3 newPosition)
    {
        characterStartingPosition = newPosition;
    }
}
