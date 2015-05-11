using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MobSpawner : MonoBehaviour {
	
	float wallTestOffset;
	public float spawnRange;
	public int spawnNumber;
	public GameObject mob;
	List<Vector3> spawnList;
	
	//public GameObject testSphere;
	
	void Start () {
	
		wallTestOffset = 1f;
		spawnList = new List<Vector3>();
		
		Spawn();
	}
	
	
	void Spawn () {
		
		for (int i = 0; i < spawnNumber; i++){
			
			float newSpawnX = Random.Range(gameObject.transform.position.x -
											(spawnRange / 2), 
											gameObject.transform.position.x +
											(spawnRange / 2));
			float newSpawnZ = Random.Range(gameObject.transform.position.z -
											(spawnRange / 2), 
											gameObject.transform.position.z +
											(spawnRange / 2));
			
			Vector3 newSpawn = new Vector3(newSpawnX, gameObject.transform.position.y + wallTestOffset,
											newSpawnZ);
			
			Vector3 wallTestOrigin = new Vector3(gameObject.transform.position.x,
												gameObject.transform.position.y + wallTestOffset,
												gameObject.transform.position.z);
			
			
			Vector3 testDir = wallTestOrigin - newSpawn;
			Ray wallTest = new Ray(wallTestOrigin, testDir);
			
			RaycastHit hit;
						
			Vector3 debugRay = wallTestOrigin + testDir;
				//Instantiate(testSphere, debugRay, Quaternion.identity);
			
			if (Physics.Raycast(wallTest, out hit, testDir.magnitude)){
				
				Vector3 hitDistance = hit.point - wallTestOrigin;
				//newSpawn = hit.point - new Vector3(3,0,3);
				//Instantiate(testSphere, hit.point, Quaternion.identity);
				newSpawn = wallTestOrigin;
				//continue;
				
			}
			
			spawnList.Add(newSpawn);			
			
		}
		
		
		foreach (Vector3 pos in spawnList){
			
			GameObject newMob = Instantiate(mob, pos, Quaternion.identity) as GameObject;
			newMob.tag = "Enemy";
			newMob.SendMessage("AssignSpawn", gameObject);
		}
	}
	
	void Update () {
	
	}
}
