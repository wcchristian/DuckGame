using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	// Default player 1 controlls
	public string horizontalCtrl = "Horizontal_P1";
	public string verticalCtrl = "Vertical_P1";
	public string boostCtrl = "Boost_P1";
	public string fire1Ctrl = "Fire1_P1";
	public float speed = 10.0f;
	public float boostSpeed = 50.0f;
	public float rotateSpeed = 5.0f;
	public float quackSpeed = 50.0f;
	public GameObject duckExplode;
	public GameObject heldItem;
	public GameObject inventoryIcon;
	public GameObject inventory;
	public bool stunned;
	public AudioClip turtleAudioClip;
	public AudioClip turtleEquipAudioClip;
	public AudioClip breadAudioClip;
	public AudioClip boostAudioClip;
	

	private Rigidbody2D rb;
	public int countKills;
	public int countDeaths;
	public bool isDead;
	private float powerupStartTime;
	private float powerupDuration = 5.0f;
	private bool hasShell;
	private float boostStartTime;
	private float boostCooldown = 3.0f;
	private float stunnedStartTime;
	private float stunnedDuration = 2.0f;
	private Vector3 startingPosition;
	private bool ducklingActive;


	void Start()
	{
		isDead = false;
		stunned = false;
		hasShell = false;
		heldItem = null;
		ducklingActive = false;
		rb = GetComponent<Rigidbody2D> ();
		countKills = 0;
		countDeaths = 0;
		boostStartTime = Time.time;

		//Setting initial positions
		this.startingPosition = this.transform.position;
	}

	void FixedUpdate()
	{
		if(!stunned)
		{
			// Movement
			float horizontal = Input.GetAxis (horizontalCtrl);
			float vertical = Input.GetAxis (verticalCtrl);

			rb.AddForce (transform.up * speed * vertical);
			rb.transform.Rotate(-transform.forward * rotateSpeed * horizontal);
		}

		if(Time.time > stunnedStartTime + stunnedDuration)
		{
			stunned = false;
		}

	}

	void Update()
	{

		// Dead
		if(isDead)
		{
			heldItem = null;
			Destroy(inventoryIcon);
			this.gameObject.SetActive(false);
			GameManager.WaitAndRespawn(this.gameObject, this.startingPosition);
		}

		// Boost
		if(Input.GetAxis (boostCtrl) > 0 && Time.time > boostStartTime + boostCooldown && !stunned)
		{
			rb.AddForce (transform.up * speed * boostSpeed);
			AudioManager.PlayClip(boostAudioClip);
			boostStartTime = Time.time;
		}

		// Powerup/Shell
		if(hasShell && Time.time > powerupStartTime + powerupDuration)
		{
			hasShell = false;
			this.gameObject.transform.Find("ButtonCollider").gameObject.SetActive(true);
			this.gameObject.transform.Find("turtle").gameObject.SetActive(false);
		}

		if(ducklingActive && Time.time > powerupStartTime + powerupDuration) {
			this.ducklingActive = false;
			GameObject[] ducklinglist = GameObject.FindGameObjectsWithTag("ducklings");
			foreach(GameObject duckling in ducklinglist) {
				Destroy(duckling);
			}
		}

		// Shoot a "quack"
		if(Input.GetAxis(fire1Ctrl) > 0 && heldItem != null && heldItem.name.Equals("quack"))
		{
			Rigidbody2D rb = Instantiate(heldItem.GetComponent<Rigidbody2D>(), transform.position + transform.up * 1.5f, transform.rotation);
			Physics2D.IgnoreCollision(rb.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
			rb.AddForce(transform.up * quackSpeed);
			heldItem = null;
			Destroy(inventoryIcon);
		}

		// Equip turtle shell
		if(Input.GetAxis(fire1Ctrl) > 0 && heldItem != null && heldItem.name.Equals("turtle"))
		{
			powerupStartTime = Time.time;
			this.gameObject.transform.Find("ButtonCollider").gameObject.SetActive(false);
			heldItem.SetActive(true);
			AudioManager.PlayClip(turtleEquipAudioClip);
			hasShell = true;
			heldItem = null;
			Destroy(inventoryIcon);
		}

		// Shoot Duck Dudes
		if(Input.GetAxis(fire1Ctrl) > 0 && heldItem != null && heldItem.name.Equals("Ducklings") && Time.time > powerupStartTime + powerupDuration) {
			powerupStartTime = Time.time;
			Instantiate(heldItem);
			heldItem.SetActive(true);
			heldItem = null;
			Destroy(inventoryIcon);
			this.ducklingActive = true;
		}

	}

	void OnTriggerEnter2D(Collider2D col)
	{
		// If this duck hits another ducks button
		if (col.gameObject.CompareTag("DuckButton"))
		{
			this.countKills++;
		}

		// If another duck hits this ducks button
		if(col.gameObject.name.Equals("HeadCollider") || col.gameObject.name.Equals("BodyCollider"))
		{
			this.countDeaths++;
			GameObject explosion = (GameObject)Instantiate (duckExplode);
			explosion.transform.position = col.gameObject.transform.position;
			isDead = true;
			heldItem = null;
		}

		// If this duck hits a power up (shell)
		if(col.gameObject.CompareTag("turtle") && heldItem == null && inventoryIcon == null) 
		{
			// powerupStartTime = Time.time;
			// this.gameObject.transform.Find("ButtonCollider").gameObject.SetActive(false);
			// this.gameObject.transform.Find("turtle").gameObject.SetActive(true);
			// hasShell = true;
			// Destroy(col.gameObject);
			heldItem = this.gameObject.transform.Find("turtle").gameObject;
			AudioManager.PlayClip(turtleAudioClip);
			inventoryIcon = Instantiate(Resources.Load("Prefabs/Game/turtleIcon") as GameObject, inventory.transform.position, Quaternion.identity);
			Destroy(col.gameObject);
		}

		// If this duck hits a bread, pick up "quack" projectile
		if(col.gameObject.CompareTag("bread") && heldItem == null && inventoryIcon == null)
		{
			// powerupStartTime = Time.time;
			heldItem = Resources.Load("Prefabs/Game/quack") as GameObject;
			AudioManager.PlayClip(breadAudioClip);
			inventoryIcon =	Instantiate(Resources.Load("Prefabs/Game/breadIcon") as GameObject, inventory.transform.position, Quaternion.identity);
			Destroy(col.gameObject);
		}

		if (col.gameObject.CompareTag ("ducklings") && heldItem == null && inventoryIcon == null) 
		{
			heldItem = Resources.Load ("Prefabs/Ducklings") as GameObject;
			AudioManager.PlayClip(turtleAudioClip);
			inventoryIcon = Instantiate (Resources.Load("Prefabs/Game/ducklingIcon") as GameObject, inventory.transform.position, Quaternion.identity);
			Destroy (col.gameObject);
		}

	}

	void OnCollisionEnter2D(Collision2D col)
	{
		// If this duck gets hit by a "quack" projectile
		if(col.gameObject.CompareTag("quack") && !hasShell)
		{
			stunned = true;
			stunnedStartTime = Time.time;
		}
	}

}
