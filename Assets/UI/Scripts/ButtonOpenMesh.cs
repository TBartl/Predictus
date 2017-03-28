using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOpenMesh : MonoBehaviour, Resettable {
    
    public MeshFilter meshToDrawTo;
    public GameObject continueButton;

    public void OnClick() {
        SoundManager.SM.PlayButtonSound ();
        UtilityOpenOBJ.S.StartCoroutine(UtilityOpenOBJ.S.OpenOBJ(OnReturnedMesh));
    }

    // Coroutines can't return values nicely, so  have the coroutine call this function once it's done
    public void OnReturnedMesh(Mesh m) {
        Debug.Log(m.vertices.Length);
        if (m == null) {
            return;
        }
        meshToDrawTo.mesh = m;
        continueButton.SetActive(true);
    }

    public void Reset() {
        meshToDrawTo.mesh = null;
        continueButton.SetActive(false);
    }
}
