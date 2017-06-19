using UnityEngine;

public class StompObstacle : MonoBehaviour {

    public float ySizeSmall = 3.5f;

    float ySizeBig = 5.5f; //level height
    float yPositionSmall, yPositionBig;

    enum StompState
    {
        Up,
        Stomp,
        Down,
        Rise
    }

    StompState currentState;

    [Space(10)]
    public float timeToStomp = 0.2f;
    public float timeDown = 1f;
    public float timeToRise = 1.5f;
    public float timeUp = 5f;

    float lastStateTimeStamp;
        
	// Use this for initialization
	void Start ()
    {
        yPositionBig = transform.position.y;
        yPositionSmall = transform.position.y + (ySizeBig - ySizeSmall) / 2;
        
        currentState = StompState.Down;
        lastStateTimeStamp = Time.time;
    }
	
	// Update is called once per frame
	void Update ()
    {
		switch (currentState)
        {
            case StompState.Up:
                if (Time.time - lastStateTimeStamp > timeUp) {
                    currentState = StompState.Stomp;
                    //Debug.Log("Stomp");
                    lastStateTimeStamp = Time.time;
                }                
                break;
            case StompState.Stomp:
                //do fancy animation
                var timeProgress = (Time.time - lastStateTimeStamp) / timeToStomp;
                var currentYSize = Mathf.Lerp(ySizeSmall, ySizeBig, timeProgress);
                var currentYPosition = Mathf.Lerp(yPositionSmall, yPositionBig, timeProgress);
                transform.localScale = new Vector3(transform.localScale.x, currentYSize, transform.localScale.z);
                transform.position = new Vector3(transform.position.x, currentYPosition, transform.position.z);

                if (timeProgress >= 1)
                {
                    currentState = StompState.Down;
                    lastStateTimeStamp = Time.time;
                    //Debug.Log("Down");
                }
                break;
            case StompState.Down:
                if (Time.time - lastStateTimeStamp > timeDown)
                {
                    currentState = StompState.Rise;
                    //Debug.Log("Rise");
                    lastStateTimeStamp = Time.time;
                }
                break;
            case StompState.Rise:
                //do almost fancy animation
                timeProgress = (Time.time - lastStateTimeStamp) / timeToRise;
                currentYSize = Mathf.Lerp(ySizeBig, ySizeSmall, timeProgress);
                currentYPosition = Mathf.Lerp(yPositionBig, yPositionSmall, timeProgress);
                transform.localScale = new Vector3(transform.localScale.x, currentYSize, transform.localScale.z);
                transform.position = new Vector3(transform.position.x, currentYPosition, transform.position.z);

                if (timeProgress >= 1)
                {
                    currentState = StompState.Up;
                    lastStateTimeStamp = Time.time;
                    //Debug.Log("Up");
                }
                break;
        }
	}
}
