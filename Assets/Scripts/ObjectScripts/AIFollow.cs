using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFollow : MonoBehaviour {

     public Transform target;
     public float speed = 0.05f;

	 public GameObject[] playerList;

	 void Start() {
		 this.target = playerList[Random.Range(0, GameManager.numberOfPlayers)].transform;
	 }
 
     void Update(){
		 if(target != null) {
			Vector3 dest = target.transform.position; 
			transform.position += (dest - transform.position).normalized * speed;
		 } 
     }

}
