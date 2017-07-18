using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject startingArea;
    [Header("Obstacles")]
    public Obstacle[] easyObstacles; //only 1 input required
    public Obstacle[] hardObstacles; //2 inputs required

    private Obstacle[] currentLoadedObstacles;
    private Obstacle lastObstacle;

    [Header("Difficulty related stuff")]
    //public int easyStartObstacles = 2; //how many easy obstacles before hard ones can appear
    [Range(0f, 1f)]
    public float maxEasyHardRatio = 0.9f;
    [Range(0f, 0.1f)]
    public float ratioIncreasePerObstacle = 0.05f; 
    private float currentEasyHardRatio = 0;

	void Start ()
    {
		
	}

    public void PlaceNextObstacle()
    {        
        //check for easy vs hard obstacle
        currentEasyHardRatio += ratioIncreasePerObstacle;
        currentEasyHardRatio = Mathf.Clamp(currentEasyHardRatio, 0f, maxEasyHardRatio);

        var threshold = Random.value;
        var hardObstacle = threshold < currentEasyHardRatio;

        Obstacle newObstacle;
        
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
        Instantiate(newObstacle.gameObject, transform);
        //change slowmo field width
        //delete obstacle 1 or 2 behind
        //move starting area in case of player death
    }
}