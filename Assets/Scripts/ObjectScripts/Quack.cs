using UnityEngine;

public class Quack : MonoBehaviour {


	private float startTime;
	private float decayTime = 2f;

	void Start()
	{
		startTime =Time.time;
	}

	void Update()
	{
		if(Time.time > startTime + decayTime)
		{
			Destroy(gameObject);
		}
	}

}
