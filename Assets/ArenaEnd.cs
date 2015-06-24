using UnityEngine;
using System.Collections;

public class ArenaEnd : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.H))
			Application.LoadLevel("Spaceship_sequence");
	}
}
