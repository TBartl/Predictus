using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScreenChange : MonoBehaviour {
    public Screen newScreen;

    public void OnClick() {
		if (!ScreenManager.S.IsTransitioning ())
			SoundManager.SM.PlayButtonSound ();
            ScreenManager.S.ChangeScreen(newScreen);
    }
}
