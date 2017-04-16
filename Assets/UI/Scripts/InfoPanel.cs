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

	void Awake (){
		IP = this;
		text = this.GetComponentInChildren<Text> ();
        RT = this.GetComponent<RectTransform>();
        RTparent = this.transform.parent.GetComponent<RectTransform>();
        canvasGroup = this.GetComponent<CanvasGroup>();
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
        Debug.Log(RTparent.sizeDelta.x / 2f);
        Debug.Log(Mathf.Abs(position.x));
        Debug.Log(RT.sizeDelta.x / 2f);
        float xExtents = RTparent.sizeDelta.x / 2f - (Mathf.Abs(position.x) + RT.sizeDelta.x / 2f);
        if (xExtents < 0)
            position.x += Mathf.Sign(position.x) * xExtents;

        this.RT.position = position;
    }

    public static Rect RectTransformToScreenSpace(RectTransform transform) {
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        float x = transform.position.x + transform.anchoredPosition.x;
        float y = UnityEngine.Screen.height - transform.position.y - transform.anchoredPosition.y;

        return new Rect(x, y, size.x, size.y);
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
