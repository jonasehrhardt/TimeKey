using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject startStagePrefab;
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
            if (startStagePrefab.name != child.name)
            {
                Destroy(child.gameObject);
            }
            else
            {
                //assumes there always is a start stage in the scene already
                lastStage = child.gameObject;
            }
        }
                
        for (int i = 0; i < obstaclesToLoadAtStart; i++)
        {
            GameObject stage = Instantiate(getRandomObstacle(), transform);
            stage.transform.position = lastStage.transform.position + new Vector3(20, 0, 0);
            lastStage = stage;
        }
    }
}
