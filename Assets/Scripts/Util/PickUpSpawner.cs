using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour {

	public GameObject[] pickUps;

	private float maxY = 5f;
	private float maxX = 5.5f;
	private float minY = -3f;
	private float minX = -6f;
	private float startTime;
	private float spawnDelay;
	
	// Use this for initialization
	void Start () {
		startTime = Time.time;	
		spawnDelay = Random.Range(2.5f, 8f);
	}
	
	// Update is called once per frame
	void Update () {

		if(Time.time > startTime + spawnDelay && CanSpawn())
		{
			Vector3 newPos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
			spawnDelay = Random.Range(2.5f, 8f);
			startTime = Time.time;

			GameObject pickUp = pickUps[Random.Range(0, pickUps.Length)];
			Instantiate(pickUp, newPos, Quaternion.identity);

		}

	}

	private bool CanSpawn()
	{
		int total = GameObject.FindGameObjectsWithTag("bread").Length + GameObject.FindGameObjectsWithTag("turtle").Length + GameObject.FindGameObjectsWithTag("ducklings").Length;
		return total < 5;
	}
}
