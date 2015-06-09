using UnityEngine;
using System.Collections;

public class Monster : vp_DamageHandler {
	
	private GameObject mySpawn;
	private GameObject playerObject;

	private string AIState;
	private Animation charAnim;
	private CharacterController controller;

	//private float heading;
	private float nextDirectionChange;
	private float moveDecision;
	private float nextIdleDecision;
	private float currentSpeed;
	private float distToPlayer;
	private float nextPlayerTest;
	private float nextFire;
	private float dotToPlayer;
	private float hurtingEnd;
	private float fireEnd;
//	private float maxHeadingChange = 180;
	private float visionAngle = 0.25f;
	private float idleDecisionInterval = 2;
	private float rotateSpeed = 3;
	private float testPlayerDelay = 0.2f;

	private Vector3 targetRotation;
	private Vector3 randDir;
	private Vector3 diffFromSpawn;
	private Vector3 diffFromPlayer;

	private bool heardPlayer = false;
	private bool firing;
	private bool hurting;

	public float speed = 1;
	public float runspeed = 3;
	public float directionChangeInterval = 1;
	public float maxWanderingDistance = 20;
	public float sightDistance = 15;
	public float hearDistance = 5;
	public float fireDelay = 1;

	public GameObject bullet;
	public GameObject firingPosition;

	private vp_DamageHandler DamageHandler;
	
	void Start () {

		playerObject = GameObject.FindGameObjectWithTag("Player");

		charAnim = gameObject.GetComponentInChildren<Animation>();
		controller = GetComponent<CharacterController>();
				
		//heading = Random.Range(0, 360);
		transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);

		nextPlayerTest = Time.time + testPlayerDelay;
		
		AIState = "Idle";
		firing = false;
		
		DecideMove();
	}

	void AssignSpawn (GameObject spawn) {

		mySpawn = spawn;	

	}


	void TakeDamageAnim () {
		if (AIState != "Dead") {
			Debug.Log("health :"+CurrentHealth);
			charAnim.CrossFade("hit");
			
			hurting = true;
			hurtingEnd = Time.time + charAnim.animation["hit"].clip.length;
			
			nextIdleDecision = Time.time + charAnim.animation["hit"].clip.length;
			
			if (CurrentHealth <= 0) {
				
				charAnim.CrossFade("death");
				
				AIState = "Dead";
				
				CharacterController charCont = GetComponent<CharacterController>();
				charCont.enabled = false;
			}
		}

	}

	public override void Damage(float damage){
		Damage(new vp_DamageInfo(damage, null));
	}

	public override void Damage(vp_DamageInfo damageInfo){
		base.Damage(damageInfo);
		TakeDamageAnim ();
	}

	void DecideMove () {

		if (AIState == "Idle"){

			float randIdle = Random.Range(0,2.0f);
			
			if (randIdle < 2.0f) {
				currentSpeed = speed;
				charAnim.CrossFade("walk");
				nextIdleDecision = Time.time + charAnim.animation["walk"].clip.length;
			}
			
			if (randIdle < 1.4f) {
				currentSpeed = runspeed;
				charAnim.CrossFade("run");
				nextIdleDecision = Time.time + charAnim.animation["run"].clip.length;
			}
			
			if (randIdle < 0.9f) {
				currentSpeed = 0;
				charAnim.CrossFade("idle1");
				nextIdleDecision = Time.time + charAnim.animation["idle1"].clip.length;
			}
			
			if (randIdle < 0.6f) {
				currentSpeed = 0;
				charAnim.CrossFade("idle2");
				nextIdleDecision = Time.time + charAnim.animation["idle2"].clip.length;
			}
			
			if (randIdle < 0.3f) {
				currentSpeed = 0;
				charAnim.CrossFade("idle3");
				nextIdleDecision = Time.time + charAnim.animation["idle3"].clip.length;
			}	
		}
	}


	void TestVision () {

		float visionTest = Vector3.Dot(gameObject.transform.forward, playerObject.transform.position);

		if (visionTest > visionAngle) {

			AIState = "Attack";

		}else{

			AIState = "Idle";
		}
	}


	void Update () {

		if (AIState == "Dead") {

			return;
		}

		// détection du joueur
		if (Time.time > nextPlayerTest) {

			diffFromPlayer = playerObject.transform.position - gameObject.transform.position;

			distToPlayer = diffFromPlayer.magnitude;

			if (distToPlayer < hearDistance) {
				
				AIState = "Attack";
				
				heardPlayer = true;
			}

			if (distToPlayer < sightDistance && !heardPlayer && AIState != "Attack"){

				TestVision();
			}

			nextPlayerTest = Time.time + testPlayerDelay;
		}

		// choix d'animation d'attente
		if (Time.time > nextIdleDecision && AIState == "Idle") {
			
			nextIdleDecision = Time.time + (idleDecisionInterval + Random.Range(-0.5f, 1f));

			DecideMove();
		}
			
		
		if (AIState == "Idle" && Time.time > nextDirectionChange && currentSpeed > 0){			

			diffFromSpawn = mySpawn.transform.position - gameObject.transform.position;	

			// direction random

			float randX = Random.Range(-1,1);
			float randZ = Random.Range(-1,1);
			
			randDir = new Vector3(randX, 1, randZ);

			// retourne vers le spawn si parti trop loin

			if (diffFromSpawn.magnitude > maxWanderingDistance) {		
		
				randDir = new Vector3(diffFromSpawn.x, 1, diffFromSpawn.z);		
			}
			
			nextDirectionChange = Time.time + directionChangeInterval;
		}

		if (Time.time >= hurtingEnd){

			hurting = false;
		}

		if (AIState == "Attack") {
					
			//Debug.DrawLine(gameObject.transform.position, playerObject.transform.position, Color.red);

			randDir = new Vector3(diffFromPlayer.x, 1, diffFromPlayer.z);

			currentSpeed = 0;

			if (Time.time >= fireEnd && firing) {

				charAnim.CrossFade("fireidle");

				firing = false;
			}

			if (!firing && !hurting) {

				charAnim.CrossFade("fireidle");
			}

			nextPlayerTest = Time.time;

			// Tire en boucle

			if (Time.time > nextFire) {

				fireEnd = Time.time + charAnim.animation["fire"].clip.length;

				firing = true;

				charAnim.Play("fire");

				nextFire = Time.time + charAnim.animation["fire"].clip.length + fireDelay;

				GameObject newBullet = Instantiate(bullet, firingPosition.transform.position, Quaternion.identity) as GameObject;
				
				newBullet.SendMessage("AssignDestination", playerObject.transform.position);
			}
		}


		//float rotationCheck = Vector3.Dot(transform.forward, randDir);

		//if(rotationCheck > -0.5f){

			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(randDir), rotateSpeed * Time.deltaTime);
			transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
		//}

		var forward = transform.TransformDirection(Vector3.forward);
		controller.SimpleMove(forward * currentSpeed);
		
	}
}
