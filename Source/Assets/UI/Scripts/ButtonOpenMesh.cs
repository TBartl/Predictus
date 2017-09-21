using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOpenMesh : MonoBehaviour, Resettable {
    
    public MeshFilter meshToDrawTo;
    public GameObject continueButton;
    public bool doReset = false;
	public bool firstClick = true;

    public void OnClick() {
		if (!firstClick) {
			SoundManager.SM.PlayButtonSound ();
		}
		firstClick = false;
        if (ScreenManager.S.IsTransitioning())
            return;
        UtilityOpenOBJ.S.StartCoroutine(UtilityOpenOBJ.S.OpenOBJ(OnReturnedMesh));
    }

    // Coroutines can't return values nicely, so  have the coroutine call this function once it's done
    public void OnReturnedMesh(Mesh m) {
        if (m == null) {
            return;
        }
        meshToDrawTo.mesh = m;
        continueButton.SetActive(true);
    }

    public void Reset() {
        if (doReset) {
            meshToDrawTo.mesh = null;
            continueButton.SetActive(false);
        }
    }
}
