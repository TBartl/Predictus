using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChangeMode : MonoBehaviour {
    public OrientInputMesh orientInputMesh;
    Text text;

    void Awake() {
        text = this.GetComponentInChildren<Text>();
    }

    public void OnClick() {
        if (ScreenManager.S.IsTransitioning())
            return;

        SoundManager.SM.PlayButtonSound();
        orientInputMesh.modifyPositionNotRotation = !orientInputMesh.modifyPositionNotRotation;

        if (orientInputMesh.modifyPositionNotRotation)
            text.text = "Change Mode to Rotation";
        else
            text.text = "Change Mode to Position";
    }
}
