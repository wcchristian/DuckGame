using UnityEngine;

public class AudioManager : MonoBehaviour {

	private static AudioSource audioSource;

	void Awake()
	{
		audioSource = gameObject.GetComponent<AudioSource>();
	}

	public static void PlayClip(AudioClip clip)
	{
		audioSource.clip = clip;
		audioSource.Play();
	}

}
