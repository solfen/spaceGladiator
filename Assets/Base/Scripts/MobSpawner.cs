using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MobSpawner : MonoBehaviour {
	
	public float spawnInterval;
	public int spawnNumberTotal;
	public int spawnNumberEach;
	public GameObject mob;

	private float lastSpawnedTime = 0;
    private int nbSpawned = 0;
	
	//public GameObject testSphere;
	
	void Start () {}
	
	void spawn () {
		for (int i = 0; i < spawnNumberEach; i++){
			GameObject newMob = Instantiate(mob, transform.position, Quaternion.identity) as GameObject;
            nbSpawned++;
		}
	}
	
	void Update () {
        if (spawnNumberTotal > nbSpawned && Time.time >= lastSpawnedTime + spawnInterval) {
            lastSpawnedTime = Time.time;
            spawn();
        }
	}
}
