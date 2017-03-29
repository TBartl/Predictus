using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScreenChangeToCorrectMesh : MonoBehaviour {
    public Screen correctScreen;
    public MeshFilter destinationMeshFilter;
    public Screen returnToScreen;

    public void OnClick() {
        if (ScreenManager.S.IsTransitioning())
            return;
		SoundManager.SM.PlayButtonSound ();
        ScreenManager.S.ChangeScreen(correctScreen);
        correctScreen.transform.GetComponentInChildren<ButtonScreenChange>().newScreen = this.transform.GetComponentInParent<Screen>();
        ButtonPassOnMeshThenChangeScreen finalContinueButton = correctScreen.transform.GetComponentInChildren<ButtonPassOnMeshThenChangeScreen>();
        finalContinueButton.to = destinationMeshFilter;
        finalContinueButton.toScreen = returnToScreen;
    }
}
