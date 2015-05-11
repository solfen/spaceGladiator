using UnityEngine;
using System.Collections;

public class BillBoard : MonoBehaviour {
	
	
	private Camera cam;
	private Vector3 lookAtPosition;
	
	void Start () {
	
		cam = Camera.main;		
	}
	
	void Update () {		
			
		lookAtPosition = cam.transform.position;
		//lookAtPosition.x = transform.position.x;
		
		transform.LookAt(lookAtPosition);		
	}
}
