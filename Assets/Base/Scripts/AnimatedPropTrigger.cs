using UnityEngine;
using System.Collections;

public class AnimatedPropTrigger : MonoBehaviour {

	
	public GameObject animatedObject;
	public float wait;
	float waitCounter;
	bool waiting;
	bool opened;
	public bool close = true;
	
	
	void Start () {
		
		opened = false;
		waiting = false;

		animatedObject.animation["Take 001"].speed = 1;
		animatedObject.animation.Play("Take 001");
		animatedObject.animation["Take 001"].speed = -1;
		animatedObject.animation["Take 001"].time = animatedObject.animation["Take 001"].length;			
		
		animatedObject.animation.Play("Take 001");


		//ssssssanimatedObject.animation.Stop();

	}	
	
	
	void OnTriggerEnter (){
	
		if (!opened){
		
			waitCounter = Time.time + wait;
			
			animatedObject.animation["Take 001"].speed = 1;
			animatedObject.animation.Play("Take 001");

			if (close){
			
				waiting = true;
			}

				opened = true;
		}
	}
	
	
	void Update () {
	
		if (waiting){
			
			if (Time.time > waitCounter && close){
			
				animatedObject.animation["Take 001"].speed = -1;
				animatedObject.animation["Take 001"].time = animatedObject.animation["Take 001"].length;			
				
				animatedObject.animation.Play("Take 001");
				
				waiting = false;
				opened = false;
			}			
		}		
	}
}
