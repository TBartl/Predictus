using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScreenChange : MonoBehaviour {
    public Screen newScreen;
    public bool noReset = false;

    public void OnClick() {
        if (ScreenManager.S.IsTransitioning())
            return;

		SoundManager.SM.PlayButtonSound ();
        if (noReset)
            ScreenManager.S.noReset = true;
        ScreenManager.S.ChangeScreen(newScreen);
    }
}
