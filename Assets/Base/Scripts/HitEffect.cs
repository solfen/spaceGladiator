using UnityEngine;
using System.Collections;

public class HitEffect : MonoBehaviour {

	public float duration;
	float deathTime;

	void Start () {
	
		deathTime = Time.time + duration;

	}

	void Update () {
	
		if (Time.time > deathTime) {

			Destroy(gameObject);
		}
	}
}
