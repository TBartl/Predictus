using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInfo : MonoBehaviour {

    [TextArea]
    public string textToDisplay;
	private RectTransform RT;

	void Awake(){
		RT = this.GetComponent<RectTransform> ();
	}

    public void OnClick() {
        if (ScreenManager.S.IsTransitioning())
            return;
        SoundManager.SM.PlayButtonSound ();

		InfoPanel.IP.ChangeText (textToDisplay, this.RT.position);

    }
}
