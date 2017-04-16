using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour {
	public static InfoPanel IP;
	private Text text;
    private RectTransform RT;
    private RectTransform RTparent;
    CanvasGroup canvasGroup;
    Canvas canvas;

	void Awake (){
		IP = this;
		text = this.GetComponentInChildren<Text> ();
        RT = this.GetComponent<RectTransform>();
        RTparent = this.transform.parent.GetComponent<RectTransform>();
        canvasGroup = this.GetComponent<CanvasGroup>();
        canvas = this.GetComponentInParent<Canvas>();
        this.gameObject.SetActive (false);
	}

	public void ChangeText(string new_string, Vector3 position) {
        text.text = new_string;
        this.gameObject.SetActive(true);
        StartCoroutine(WaitForContentFitter(position));
	}

    IEnumerator WaitForContentFitter(Vector3 position) {
        canvasGroup.alpha = 0;
        yield return new WaitForFixedUpdate();
        canvasGroup.alpha = 1;

        float xLeft = (Mathf.Abs(position.x) - RT.sizeDelta.x * canvas.scaleFactor / 2f);
        if (xLeft < 0)
            position.x -= xLeft;

        float xRight = UnityEngine.Screen.width - (Mathf.Abs(position.x) + RT.sizeDelta.x * canvas.scaleFactor / 2f);
        if (xRight < 0)
            position.x += xRight;

        float yLeft = (Mathf.Abs(position.y) - RT.sizeDelta.y * canvas.scaleFactor / 2f);
        if (yLeft < 0)
            position.y -= yLeft;

        float yRight = UnityEngine.Screen.height - (Mathf.Abs(position.y) + RT.sizeDelta.y * canvas.scaleFactor / 2f);
        if (yRight < 0)
            position.y += yRight;

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
