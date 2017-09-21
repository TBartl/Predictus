using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public static SoundManager SM;

	public AudioSource buttonSound;
	public AudioSource transformSound;
	public AudioSource shutterSound;

	// Use this for initialization
	void Start () {
		SM = this;
	}


	public void PlayButtonSound() {
		buttonSound.Play ();
	}

	public void PlayTransformSound() {
		StartCoroutine (WaitPlayTransformSound ());
	}

	IEnumerator WaitPlayTransformSound() {
		yield return new WaitForSecondsRealtime (0.15f);
		transformSound.Play();
	}

	public void PlayShutterSound() {
		shutterSound.Play ();
	}

}
