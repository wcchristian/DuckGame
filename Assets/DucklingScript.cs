using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DucklingScript : MonoBehaviour {

	public GameObject explosion;

	void OnCollisionEnter2D(Collision2D other)
	{
		Vector3 pos = other.transform.position;
		explosion.transform.position = pos;
		Destroy(other.gameObject);
		Instantiate(explosion);
		Destroy(this.gameObject);
	}
}
