using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {


	public GameObject hitEffect;
	public float speed;

	private GameObject currentTarget;

	private int damage = 50;

	private float destroyDistance;
	private float nextTargetTest;
	private float targetTestDelay = 0.05f;
	private float maxTargetDistance = 250;

	private Vector3 myDestination;
	private Vector3 distanceDiff;
	private Vector3 currentHitPoint;
	private Vector3 startPosition;
	private Vector3 diffToFirstDest;
	private Vector3 diffToStart;

	private bool moving;
	private RaycastHit hit;

	void Start () {

		//Debug.Log("bullet");
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
