using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoadUtils : MonoBehaviour {

	public bool shouldDeletePlayerPrefs;
	public bool shouldAI = false;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
		if(shouldDeletePlayerPrefs) {
			PlayerPrefs.DeleteAll();
		}
	}
}
