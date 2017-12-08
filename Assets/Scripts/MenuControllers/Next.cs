using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Next : MonoBehaviour {

	public GameObject submit;
	public GameObject dropdown;
	public EventSystem es;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis("Boost_P1") > 0) {
			es.SetSelectedGameObject(submit);
		}
	}
}
