using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject startingArea;
    [Header("Obstacles")]
    [Tooltip("Single input and double input obstacles here")]
    public GameObject[] easyObstacles;
    [Tooltip("Only combo obstacles here")]
    public GameObject[] hardObstacles;

    private List<GameObject> currentLoadedObstacles;

    private int currentXPosition; //used to place new obstacles

    [Header("Difficulty related stuff")]
    [Range(0f, 1f)]
    public float maxEasyHardRatio = 0.9f;
    [Range(0f, 0.1f)]
    public float ratioIncreasePerObstacle = 0.05f; 
    private float currentEasyHardRatio = 0;

    private void Awake()
    {
        GameManager.instance.levelGenerator = this;
    }

    void Start ()
    {
        currentLoadedObstacles = new List<GameObject>();
        currentXPosition = -5;

        var newStartingArea = Instantiate(startingArea, new Vector3(currentXPosition, 0, 0), Quaternion.identity, transform);
        currentLoadedObstacles.Add(newStartingArea);
        currentXPosition += 20;
        PlaceNextObstacle();
        PlaceNextObstacle();
    }

    public void PlaceNextObstacle()
    {
        GameObject newObstacle = GetNextObstacle();

        //add new obstacle behind last generated one
        var obstacle = Instantiate(newObstacle, new Vector3(currentXPosition, 0, 0), Quaternion.identity, transform);
        currentXPosition += 20;

        /*
        //change slowmo field width
        Transform slowMoField = obstacle.transform.Find("Slow_Mo_Field");

        var slowMoFieldWidth = 3f; //TODO: calculate appropriate width
        slowMoField.localScale = new Vector3(slowMoFieldWidth, 1, 1);
        */
        
        if (currentLoadedObstacles.Count >= 3)
        {
            var obstacleToDelete = currentLoadedObstacles[0];
            currentLoadedObstacles.RemoveAt(0);
            Destroy(obstacleToDelete);
        }
        currentLoadedObstacles.Add(obstacle);

        var newStartingPosition = obstacle.transform.Find("InputResetter").position;
    }

    private GameObject GetNextObstacle()
    {
        //check for easy vs hard obstacle
        currentEasyHardRatio += ratioIncreasePerObstacle;
        currentEasyHardRatio = Mathf.Clamp(currentEasyHardRatio, 0f, maxEasyHardRatio);

        var threshold = Random.value;
        var easyObstacle = threshold > currentEasyHardRatio;

        GameObject nextObstacle;

        //select random obstacle from chosen list
        if (easyObstacle)
        {
            int randomIndex = Random.Range(0, easyObstacles.Length);
            nextObstacle = easyObstacles[randomIndex];
        }
        else
        {
            int randomIndex = Random.Range(0, hardObstacles.Length);
            nextObstacle = hardObstacles[randomIndex];
        }

        return nextObstacle;
    }

    public void Reset()
    {
        //place starting area for easier reentering
        if (currentLoadedObstacles[0].name != "StartingArea(Clone)")
        {
            var newStartingArea = Instantiate(startingArea, new Vector3(currentXPosition - 80, 0, 0), Quaternion.identity, transform);
            currentLoadedObstacles.Insert(0, newStartingArea);
            GameManager.instance.UpdatePlayerStartingPosition(newStartingArea.transform.Find("BallStartingPosition").position);
        }
    }
}