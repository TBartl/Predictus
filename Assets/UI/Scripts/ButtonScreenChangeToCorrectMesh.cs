using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScreenChangeToCorrectMesh : MonoBehaviour {
    public Screen correctScreen;
    public Screen returnToScreen;

    public void OnClick() {
		if (!ScreenManager.S.IsTransitioning ())
			SoundManager.SM.PlayButtonSound ();
            ScreenManager.S.ChangeScreen(correctScreen);
    }
}
