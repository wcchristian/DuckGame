using UnityEngine;

public class Player : MonoBehaviour {

	// Default player 1 controlls
	public string horizontalCtrl = "Horizontal_P1";
	public string verticalCtrl = "Vertical_P1";
	public string boostCtrl = "Boost_P1";
	public float speed = 10.0f;
	public float boostSpeed = 50.0f;
	public float rotateSpeed = 5.0f;
	public GameObject duckExplode;
	public GameObject heldItem;


	private Rigidbody2D rb;
	public int countKills;
	public int countDeaths;
	public bool isDead;
	private float powerupStartTime;
	private float powerupDuration = 5.0f;
	private bool hasShell;
	private float boostStartTime;
	private float boostCooldown = 3.0f;


	void Start()
	{
		isDead = false;
		hasShell = false;
		heldItem = null;
		rb = GetComponent<Rigidbody2D> ();
		countKills = 0;
		countDeaths = 0;
		boostStartTime = Time.time;
		Debug.Log("Starting");
	}

	void FixedUpdate()
	{
		// Movement
		float horizontal = Input.GetAxis (horizontalCtrl);
		float vertical = Input.GetAxis (verticalCtrl);

		rb.AddForce (transform.up * speed * vertical);
		rb.transform.Rotate(-transform.forward * rotateSpeed * horizontal);
	}

	void Update()
	{

		// Dead
		if(isDead)
		{
			this.gameObject.SetActive(false);
		}

		// Boost
		if(Input.GetAxis (boostCtrl) > 0 && Time.time > boostStartTime + boostCooldown)
		{
			rb.AddForce (transform.up * speed * boostSpeed);
			boostStartTime = Time.time;
		}

		// Powerup/Shell
		if(hasShell && Time.time > powerupStartTime + powerupDuration)
		{
			hasShell = false;
			this.gameObject.transform.Find("ButtonCollider").gameObject.SetActive(true);
			this.gameObject.transform.Find("turtle").gameObject.SetActive(false);
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
		}

		// If this duck hits a power up (shell)
		if(col.gameObject.name.Equals("turtle"))
		{
			powerupStartTime = Time.time;
			this.gameObject.transform.Find("ButtonCollider").gameObject.SetActive(false);
			this.gameObject.transform.Find("turtle").gameObject.SetActive(true);
			hasShell = true;
			Destroy(col.gameObject);
		}

		// If this duck hits a bread
		if(col.gameObject.name.Equals("bread"))
		{
			powerupStartTime = Time.time;
			heldItem = Resources.Load("Prefabs/Game/bread") as GameObject;
			Destroy(col.gameObject);
		}


	}

}
