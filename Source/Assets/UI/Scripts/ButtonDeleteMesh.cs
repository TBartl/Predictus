using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDeleteMesh : MonoBehaviour {

	public void OnClick() {
        if (ScreenManager.S.IsTransitioning())
            return;
        SoundManager.SM.PlayButtonSound();

        LibraryContent.LC.DeleteModel ();

		// delete the obj file
	}
}
