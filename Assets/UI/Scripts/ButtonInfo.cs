using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInfo : MonoBehaviour {

    public string textToDisplay;

    public void OnClick() {
        if (ScreenManager.S.IsTransitioning())
            return;
        SoundManager.SM.PlayButtonSound ();


    }
}
