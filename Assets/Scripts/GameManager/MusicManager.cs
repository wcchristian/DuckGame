using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	public AudioSource source;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
	}

	
}
