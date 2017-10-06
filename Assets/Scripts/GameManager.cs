﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private int _highscore;
	private GameObject[] lifeBar;

    private ServerUIController _serverUIController;

    [Header("Time")]
    [Range(1f, 10f)]
    [Tooltip("High values may result in unexpected behaviour")]
    public float minTimeScale = 1;
    [Range(1f, 10f)]
    public float maxTimeScale = 3;
    [Range(0.01f, 0.1f)]
    public float timeScaleIncrements = 0.1f;

    [HideInInspector]
    public LevelManager levelManager;
        
    private int deathCount = 0;

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

		lifeBar = new GameObject[] {GameObject.Find("Life1"), GameObject.Find("Life2"), GameObject.Find("Life3")};
        //Debug.Log("Starting GameManager");
        inputManager = new InputManager();

        var startingPosition = GameObject.Find("BallStartingPosition");
        characterStartingPosition = (startingPosition != null) ? startingPosition.transform.position : pcharacter.transform.position;

        characterRigidbody = pcharacter.GetComponent<Rigidbody>();

        GameObject serverUIObject = GameObject.FindGameObjectWithTag("ServerUI");
        if (serverUIObject != null)
            _serverUIController = serverUIObject.GetComponent<ServerUIController>();

        _highscore = PlayerPrefs.GetInt("highscore", 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetLevel();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerPrefs.SetInt("highscore", 0);
            _highscore = 0;
        }

        //update UI
        if (_serverUIController != null)
        {
            _serverUIController.ChangeGamePointsText(currentPoints);
            gameStartTime += Time.deltaTime;
            _serverUIController.ChangeGameTimeText(gameStartTime);
            _serverUIController.SetHighscore(_highscore);
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

        if (!pcharacter.useAI && currentPoints > _highscore)
        {
            _highscore = currentPoints;
            PlayerPrefs.SetInt("highscore", _highscore);
        }

        currentPoints = 0;
    }

    public void ObstacleCompleted()
    {
        if (!pcharacter.useAI)
        {
            currentPoints += 1;
        }
        if (levelManager != null)
        {
            levelManager.obstaclesCleared++;
        }

        if (Time.timeScale < maxTimeScale)
            Time.timeScale += timeScaleIncrements;

        // reset AI speed when max time reached
        else if (pcharacter.useAI)
        {
            Time.timeScale = minTimeScale;
            ResetLevel();
        }
    }

    public void UpdatePlayerStartingPosition(Vector3 newPosition)
    {
        characterStartingPosition = newPosition;
    }

    public void Death()
    {
        deathCount++;
		lifeBar [3 - deathCount].GetComponent<MeshRenderer> ().enabled = false;
		if (deathCount == 3) {
			Debug.Log ("Tot");
			deathCount = 0;
			ResetLevel ();
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			Debug.Log (_highscore);
			Debug.Log ("Level Restart");
			for (int i = 0; i < 3; i++)
				lifeBar [i].GetComponent<MeshRenderer> ().enabled = true;
		}
    }
}
	