using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject startStagePrefab;
    
    [Header("Obstacles")]
    [Tooltip("Single input obstacles")]
    public GameObject[] easyObstaclePrefabs;
    [Tooltip("Double input obstacles")]
    public GameObject[] middleObstaclePrefabs;
    [Tooltip("Only combo obstacles here")]
    public GameObject[] hardObstaclePrefabs;
    public int obstaclesToLoadAtStart = 50;

    [Header("Difficulty related stuff")]
    [Range(-1f, 1f)]
    public float easyStartThreshold = 0f;
    [Range(0f, 1f)]
    public float maxEasyThreshold = 0.9f;
    [Range(0f, 0.1f)]
    public float easyThresholdIncreasePerObstacle = 0.05f;
    [Space(10)]
    [Range(-1f, 1f)]
    public float middleStartThreshold = 0f;
    [Range(0f, 1f)]
    public float maxMiddleThreshold = 0.7f;
    [Range(0f, 0.1f)]
    public float middleThresholdIncreasePerObstacle = 0.03f;

    private float currentEasyThreshold = 0;
    private float currentMiddleThreshold = 0;

    private GameObject lastStage;
    private GameObject[] loadedObstacles;
    [HideInInspector]
    public int obstaclesCleared = 1; //starting area has no resetter -> +1

    // Use this for initialization
    void Start()
    {
        GameManager.instance.levelManager = this;

        loadedObstacles = new GameObject[obstaclesToLoadAtStart + 1];

        //Destroy everything but the starting area
        foreach (Transform child in transform)
        {
            if (startStagePrefab.name != child.name)
            {
                Destroy(child.gameObject);
            }
            else
            {
                //assumes there always is a start stage in the scene already
                loadedObstacles[0] = child.gameObject;
                lastStage = child.gameObject;
            }

            currentEasyThreshold = easyStartThreshold;
            currentMiddleThreshold = middleStartThreshold;
        }
                
        for (int i = 1; i <= obstaclesToLoadAtStart; i++)
        {
            GameObject stage = Instantiate(getRandomObstacle(), transform);
            stage.transform.position = lastStage.transform.position + new Vector3(20, 0, 0);
            lastStage = stage;
            loadedObstacles[i] = stage;
        }
    }

    private GameObject getRandomObstacle()
    {
        //check for easy vs hard obstacle
        currentEasyThreshold += easyThresholdIncreasePerObstacle;
        currentEasyThreshold = Mathf.Clamp(currentEasyThreshold, -1f, maxEasyThreshold);
        currentMiddleThreshold += middleThresholdIncreasePerObstacle;
        currentMiddleThreshold = Mathf.Clamp(currentMiddleThreshold, -1f, maxMiddleThreshold);

        var threshold = Random.value;

        GameObject nextObstacle;

        //select random obstacle from chosen list
        if (currentEasyThreshold < threshold)
        {
            int randomIndex = Random.Range(0, easyObstaclePrefabs.Length);
            nextObstacle = easyObstaclePrefabs[randomIndex];
        }
        else if (currentMiddleThreshold < threshold)
        {
            int randomIndex = Random.Range(0, middleObstaclePrefabs.Length);
            nextObstacle = middleObstaclePrefabs[randomIndex];
        }
        else
        {
            int randomIndex = Random.Range(0, hardObstaclePrefabs.Length);
            nextObstacle = hardObstaclePrefabs[randomIndex];
        }

        return nextObstacle;
    }

    public Vector3 placeNewStartingArea()
    {
        var oldObstacle = loadedObstacles[obstaclesCleared];
        loadedObstacles[obstaclesCleared] = Instantiate(startStagePrefab, oldObstacle.transform.position, Quaternion.identity, transform);
        Destroy(oldObstacle);
        
        return loadedObstacles[obstaclesCleared].transform.Find("BallStartingPosition").transform.position;
    }

    private void OnDestroy()
    {
        if (GameManager.instance.levelManager == this)
        {
            GameManager.instance.levelManager = null;
        }
    }
}