using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject follow;
    private float xOffset = 0.0f;
    private float yPosition, zPosition;

	// Use this for initialization
	void Start ()
    {
        xOffset = transform.position.x - follow.transform.position.x;
        yPosition = transform.position.y;
        zPosition = transform.position.z;
    }

    void LateUpdate ()
    {   
        transform.position = new Vector3(follow.transform.position.x + xOffset, yPosition, zPosition);
    }
}