using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour {
	public static InfoPanel IP;
	private Text text;
	private RectTransform RT;

	void Awake (){
		IP = this;
		text = this.GetComponentInChildren<Text> ();
		RT = this.GetComponent<RectTransform> ();
		this.gameObject.SetActive (false);
	}

	public void ChangeText(string new_string, Vector3 position){
		text.text = new_string;
		this.gameObject.SetActive (true);
		this.RT.position = position;
	}

    public void HideThis() {
        this.gameObject.SetActive(false);
    }

	public void OnClick(){
        SoundManager.SM.PlayButtonSound();
		this.gameObject.SetActive (false);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
