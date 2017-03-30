using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSaveMesh : MonoBehaviour {

	public void OnClick() {
        if (ScreenManager.S.IsTransitioning())
            return;
        SoundManager.SM.PlayButtonSound();

        LibraryContent.LC.SaveModel();
    }
}
