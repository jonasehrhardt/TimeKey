using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject startStage;
    public List<GameObject> obstaclePrefabs;
    public int obstaclesToLoadAtStart = 500;

    private GameObject lastStage;

    GameObject getRandomObstacle()
    {
        var randomID = Random.Range(0, obstaclePrefabs.Count);
        return obstaclePrefabs[randomID];
    }

    // Use this for initialization
    void Start()
    {
        //Destroy everything but the starting area
        foreach (Transform child in transform)
        {
            if (startStage.name != child.name) Destroy(child.gameObject);
        }

        lastStage = startStage;
                
        for (int i = 0; i < obstaclesToLoadAtStart; i++)
        {
            GameObject stage = Instantiate(getRandomObstacle(), transform);
            stage.transform.position = lastStage.transform.position + new Vector3(20, 0, 0);
            lastStage = stage;
        }
    }
}
