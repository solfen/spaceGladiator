using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {
	
	private GameObject mySpawn;
	private GameObject playerObject;

	private string AIState;
	private Animation charAnim;
	private CharacterController controller;

	//private float heading;
	private float nextDirectionChange;
	private float moveDecision;
	private float nextIdleDecision;
	private float distToPlayer;
	private float nextPlayerTest;
	private float nextFire;
    private float nextWanderDir;
    private float nextStrifeDir;
	private float dotToPlayer;
	private float hurtingEnd;
    private float fireEnd;
    private int strifeDir = 1;
//	private float maxHeadingChange = 180;
	private float visionAngle = 0.25f;
	private float idleDecisionInterval = 2;
	private float rotateSpeed = 3;
    private float testPlayerDelay = 0.1f;
    private float wanderColTestInterval = 0.5f;

	private Vector3 targetRotation;
    private Vector3 wanderDir;
	private Vector3 diffFromSpawn;
	private Vector3 diffFromPlayer;

	private bool heardPlayer = false;
	private bool firing;
	private bool hurting;

	public float walkSpeed = 1;
	public float runSpeed = 3;
    public float wanderDirInterval = 5;
    public float strifeDirInterval = 1;
	public float sightDistance = 15;
    public float hearDistance = 5;
    public float fightMovesDist = 5;
	public float fireDelay = 1;

	public GameObject bullet;
	public GameObject firingPosition;

	private vp_DamageHandler damageHandler;
	
	void Start () {

		playerObject = GameObject.FindGameObjectWithTag("Player");

		charAnim = gameObject.GetComponentInChildren<Animation>();
        charAnim.animation["run"].wrapMode = WrapMode.Loop;
        //charAnim.animation["run"].wrapMode = WrapMode.Loop;
		controller = GetComponent<CharacterController>();
		damageHandler = GetComponent<vp_DamageHandler>();
		//heading = Random.Range(0, 360);
		transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);

		nextPlayerTest = Time.time + testPlayerDelay;
		
		AIState = "Wander";
        charAnim.CrossFade("run");
		firing = false;
		
		//playAnimIdle();
	}

    void playerDetection() {
        nextPlayerTest = Time.time + testPlayerDelay;

        diffFromPlayer = playerObject.transform.position - gameObject.transform.position;
        distToPlayer = diffFromPlayer.magnitude;

        if (distToPlayer < hearDistance) {
            AIState = "Attack";
            heardPlayer = true;
            charAnim.CrossFade("run");
        }
        if (distToPlayer < sightDistance && !heardPlayer && AIState != "Attack" && TestVision()) {
                //charAnim.CrossFade("run");
                AIState = "Attack";
        }
        else if (distToPlayer > sightDistance && AIState != "Wander") {
            AIState = "Wander";
            charAnim.CrossFade("run");
            //nextIdleDecision = Time.time;
        }
    }
    void wander() {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(wanderDir), rotateSpeed * Time.deltaTime);
        controller.SimpleMove(transform.forward * runSpeed);

        if (Time.time >= nextWanderDir) {
            nextWanderDir = Time.time + wanderDirInterval;
            wanderDir = new Vector3(0, Random.Range(0,360), 0);
        }
    }

    /*void OnControllerColliderHit(ControllerColliderHit hit) {
        if (AIState == "Wander") {
            nextWanderDir = Time.time;
        }
    }*/

    void fightMove() {
        if (distToPlayer > fightMovesDist+1) {
            controller.SimpleMove(transform.forward * runSpeed);
            charAnim.CrossFade("run");
        }
        else if (distToPlayer < fightMovesDist-1) {
            controller.SimpleMove(-transform.forward * runSpeed);
            charAnim.CrossFade("run");
        }
        else {
            controller.SimpleMove(strifeDir * transform.right * runSpeed);
            charAnim.CrossFade("run");
        }
        if (Time.time > nextStrifeDir) {
            nextStrifeDir = Time.time + strifeDirInterval;
            strifeDir = -strifeDir;
            fightMovesDist += strifeDir;
        }

    }

    void flee() {

    }
    void attackState() {
        fightMove();

        Quaternion goalRot = Quaternion.LookRotation(new Vector3(diffFromPlayer.x, 0, diffFromPlayer.z));
        float angleBetween = Quaternion.Angle(transform.rotation, goalRot);
        transform.rotation = Quaternion.Slerp(transform.rotation, goalRot, rotateSpeed * Time.deltaTime);

        if (Time.time >= fireEnd && firing || (!firing && !hurting) ) {
            charAnim.CrossFade("fireidle");
            firing = false;
        }
        nextPlayerTest = Time.time;

        // Tire en boucle
        if (Time.time > nextFire && angleBetween >= -10 && angleBetween <= 10) {
            fireEnd = Time.time + charAnim.animation["fire"].clip.length;
            firing = true;

            charAnim.Play("fire");

            nextFire = Time.time + charAnim.animation["fire"].clip.length + fireDelay;

            GameObject newBullet = Instantiate(bullet, firingPosition.transform.position, Quaternion.identity) as GameObject;

            newBullet.SendMessage("AssignDestination", playerObject.transform.GetChild(0).position);
        }
    }

    void searchState() {

    }

	void playAnimIdle () {
			float randIdleIndex = Random.Range(1,3);
            charAnim.CrossFade("idle" + randIdleIndex);
            nextIdleDecision = Time.time + charAnim.animation["idle" + randIdleIndex].clip.length;
	}

	bool TestVision () {
		float visionTest = Vector3.Dot(gameObject.transform.forward, playerObject.transform.position);
        return visionTest > visionAngle;
	}

	void Update () {

        if (damageHandler.CurrentHealth <= 0.0f) {
			return;
		}
        if (AIState == "Wander") {
            wander();
        }

		// détection du joueur
		if (Time.time > nextPlayerTest) {
            playerDetection();
		}

		// choix d'animation d'attente
		if (Time.time > nextIdleDecision && AIState == "Idle") {
			nextIdleDecision = Time.time + (idleDecisionInterval + Random.Range(-0.5f, 1f));
			playAnimIdle();
		}

		if (Time.time >= hurtingEnd) {
			hurting = false;
		}

		if (AIState == "Attack") {
            attackState();
		}



		
	}
}
