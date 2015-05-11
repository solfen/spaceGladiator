using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	Vector3 myDestination;
	public float speed;
	public GameObject hitEffect;
	bool moving;
	Vector3 distanceDiff;
	float destroyDistance;
	int damage = 50;
	float nextTargetTest;
	float targetTestDelay = 0.05f;
	GameObject currentTarget;
	Vector3 currentHitPoint;
	float maxTargetDistance = 250;
	RaycastHit hit;
	Vector3 startPosition;
	Vector3 diffToFirstDest;
	Vector3 diffToStart;

	void Start () {
	
		moving = false;
		destroyDistance = 0.5f;
		startPosition = transform.position;

		//TargetTest();

		nextTargetTest = Time.time + targetTestDelay;
	}

	void AssignDestination (Vector3 dest){

		myDestination = dest;
		transform.LookAt(myDestination);
		moving = true;
		diffToFirstDest = transform.position - myDestination;

		TargetTest();
	}

	void TargetTest () {

		if (Physics.Raycast(transform.position, transform.forward, out hit, 100)){
			
			currentTarget = hit.collider.gameObject;
			currentHitPoint = hit.point;
			myDestination = hit.point;
		}
	}

	void Update () {	

		transform.LookAt(myDestination);

		diffToStart = transform.position - startPosition;
		/*
		if (diffToStart.magnitude > diffToFirstDest.magnitude){

			TargetTest();
		}*/

		/*
		if (Time.time > nextTargetTest) {

			TargetTest();

			if (currentTarget){

				if (currentTarget.tag == "Enemy"){

					targetTestDelay = 0;
				}
			}

			nextTargetTest = Time.time + targetTestDelay;
		}*/

		//if (moving){

		Debug.DrawLine(transform.position, myDestination, Color.cyan);

			transform.Translate(0, 0, speed * Time.deltaTime, Space.Self);
		//}

		distanceDiff = transform.position - myDestination;

		//float targetOrientTest = 1;

		/*
		if (currentTarget){

			targetOrientTest = Vector3.Dot(transform.forward.normalized, currentHitPoint.normalized);
		}*/

		if (distanceDiff.magnitude < destroyDistance || distanceDiff.magnitude > maxTargetDistance ||
		    diffToStart.magnitude > diffToFirstDest.magnitude) {

			Instantiate (hitEffect, gameObject.transform.position, Quaternion.identity);

			if (currentTarget){

				if (currentTarget.tag == "Enemy"){
				
					currentTarget.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
				}
			}

			Destroy(gameObject);

		}
	}

}
