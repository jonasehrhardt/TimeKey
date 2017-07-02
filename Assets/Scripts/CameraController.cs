using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject follow;
    private float xOffset = 0.0f;
    private float yPosition, zPosition;
    public Texture cameraOverlay;

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

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, 300, 100), cameraOverlay);
    }
}