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
    private GameObject lastObstacle;


    [Header("Difficulty related stuff")]
    [Range(0f, 1f)]
    public float maxEasyHardRatio = 0.9f;
    [Range(0f, 0.1f)]
    public float ratioIncreasePerObstacle = 0.05f; 
    private float currentEasyHardRatio = 0;

	void Start ()
    {
        currentLoadedObstacles = new List<GameObject>();
	}

    public void PlaceNextObstacle()
    {        
        //check for easy vs hard obstacle
        currentEasyHardRatio += ratioIncreasePerObstacle;
        currentEasyHardRatio = Mathf.Clamp(currentEasyHardRatio, 0f, maxEasyHardRatio);

        var threshold = Random.value;
        var hardObstacle = threshold < currentEasyHardRatio;

        GameObject newObstacle;
        
        //select random obstacle from chosen list
        if (hardObstacle)
        {
            int randomIndex = Random.Range(0, hardObstacles.Length);
            newObstacle = hardObstacles[randomIndex];
        }
        else
        {
            int randomIndex = Random.Range(0, easyObstacles.Length);
            newObstacle = easyObstacles[randomIndex];
        }

        //add new obstacle behind last generated one
        var obstacle = Instantiate(newObstacle, transform);
        //change slowmo field width
        Transform slowMoField = obstacle.transform.Find("Slow_Mo_Field");

        var slowMoFieldWidth = 3f;
        slowMoField.localScale = new Vector3(slowMoFieldWidth, 1, 1);

        var newStartingPosition = obstacle.transform.Find("InputResetter").position;
        GameManager.instance.UpdatePlayerStartingPosition(newStartingPosition);
    }
}