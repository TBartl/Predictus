using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public static SoundManager SM;

	public AudioSource buttonSound;
	public AudioSource transformSound;

	// Use this for initialization
	void Start () {
		SM = this;
	}


	public void PlayButtonSound() {
		buttonSound.Play ();
	}

	public IEnumerator PlayTransformSound() {
		yield return new WaitForSeconds (0.25f);
		transformSound.Play ();
	}
}
