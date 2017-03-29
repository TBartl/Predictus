using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPassOnMeshThenChangeScreen : MonoBehaviour {

    public MeshFilter from;
    public MeshFilter to;
    public Screen toScreen;

    public void OnClick() {
        if (ScreenManager.S.IsTransitioning())
            return;

        SoundManager.SM.PlayButtonSound ();
        to.mesh = from.mesh;
        ScreenManager.S.ChangeScreen(toScreen);
    }
}
