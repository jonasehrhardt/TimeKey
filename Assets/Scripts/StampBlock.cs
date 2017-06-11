using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampBlock : MonoBehaviour {
	private int timer;
	// Status: True, if the block is raised, False, if not
	private bool status; 
	private GameObject block;
	private Vector3 dummy;

	// Use this for initialization
	void Start () {
		timer = 0;
		status = true;
		block = GameObject.Find ("Obstacle_Stamp");
		dummy = new Vector3 (0, 4, 0);
	}
	
	// Update is called once per frame
	void Update () {
		timer++;
		if (timer == 15 && status)
			Sink ();
		else if (timer == 10 && !status)
			Rise ();
		
	}

	void Rise () {
		block.gameObject.transform.localScale -= dummy;
		timer = 0;
		status = true;
	}

	void Sink() {		
		block.gameObject.transform.localScale += dummy;
		timer = 0;
		status = false;
	}
}
