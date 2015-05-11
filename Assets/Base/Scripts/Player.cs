using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float firingSpeed;
	public float firingDuration;
	public GameObject firingEffect;
	public GameObject bullet;
	Vector3 shootPoint;
	bool firing;
	float firingHideTimer;
	float firingRepeatTimer;
	Renderer[] firingEffectRenderers;
	Light[] firingEffectLights;
	Vector3 firingScreenPos;


	void Start () {
	
		firingEffectRenderers = firingEffect.gameObject.GetComponentsInChildren<Renderer>();
		firingEffectLights = firingEffect.gameObject.GetComponentsInChildren<Light>();
		HideFiringEffect();
		firing = false;
		firingHideTimer = Time.time;
		firingRepeatTimer = Time.time;

		firingScreenPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);

	}

	void HideFiringEffect () {

		foreach (Renderer rend in firingEffectRenderers){			
			rend.enabled = false;
		}

		foreach (Light l in firingEffectLights){			
			l.enabled = false;
		}
	}

	void ShowFiringEffect () {

		foreach (Renderer rend in firingEffectRenderers){			
			rend.enabled = true;
		}

		foreach (Light l in firingEffectLights){			
			l.enabled = true;
		}
	}


	void Update () {	

		if (firing) {

			if (Time.time > firingHideTimer) {

				HideFiringEffect ();
				firing = false;
			}
		}

		if (Input.GetMouseButton(0)){

			if (Time.time > firingRepeatTimer) {

				ShowFiringEffect();
				
				firingHideTimer = Time.time + firingDuration;
				firingRepeatTimer = Time.time + firingSpeed;

				firing = true;

				Ray ray = Camera.main.ScreenPointToRay(firingScreenPos);

				RaycastHit hit;

				if (Physics.Raycast(ray, out hit, 1000)){

					//Debug.DrawLine(ray.origin, hit.point);
					shootPoint = hit.point;
				}
				
				GameObject newBullet = Instantiate(bullet, firingEffect.transform.position, Quaternion.identity) as GameObject;
				
				newBullet.SendMessage("AssignDestination", shootPoint);
			}
		}
	}
}
