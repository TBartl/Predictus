using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileBrowserSound : MonoBehaviour {

	public static FileBrowserSound SM;

	public AudioSource buttonSound;

		// Use this for initialization
	void Start () {
		SM = this;
	}
		
	public void PlayButtonSound() {
		buttonSound.Play ();
	}
}
