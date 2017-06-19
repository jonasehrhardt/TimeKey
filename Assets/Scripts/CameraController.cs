using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject follow = null;
    private float dist = 0.0f;

	// Use this for initialization
	void Start () {
        dist = this.gameObject.transform.position.x - follow.transform.position.x;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void LateUpdate()
    {
        Vector3 pos = this.gameObject.transform.position;
        Vector3 tpos = follow.transform.position;
        this.gameObject.transform.position = new Vector3(tpos.x + dist, pos.y, pos.z);
    }
}
