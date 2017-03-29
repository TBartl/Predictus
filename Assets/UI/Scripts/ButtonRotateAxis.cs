using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonRotateAxis : MonoBehaviour {
    public OrientInputMesh orientInputMesh;
    Text text;
    public Vector3 rotation;

    void Awake() {
    }

    public void OnClick() {
        if (ScreenManager.S.IsTransitioning())
            return;

        SoundManager.SM.PlayButtonSound();
        orientInputMesh.transform.rotation = Quaternion.Euler(rotation);
        orientInputMesh.ApplyMeshTranslationAndRotation();
    }
}
