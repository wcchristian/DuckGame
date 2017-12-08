using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public string horizontalCtrl = "Horizontal_P1";
	public string verticalCtrl = "Vertical_P1";
	public float speed = 10.0f;
	public float rotateSpeed = 5.0f;

	private Rigidbody2D rb;

	void Start()
	{
		rb = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate()
	{
		float horizontal = Input.GetAxis (horizontalCtrl);
		float vertical = Input.GetAxis (verticalCtrl);

		rb.AddForce (transform.up * speed * vertical);
		rb.transform.Rotate(-transform.forward * rotateSpeed * horizontal);
	}

}
