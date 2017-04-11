using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPassOnMeshThenChangeScreen : MonoBehaviour {

    public MeshFilter from;
    public List<MeshFilter> to;
    public Screen toScreen;

    public void OnClick() {
        if (ScreenManager.S.IsTransitioning())
            return;

        SoundManager.SM.PlayButtonSound ();
		StartCoroutine (SoundManager.SM.PlayTransformSound ());
        foreach (MeshFilter m in to) {
            m.mesh = from.mesh;
        }
        ScreenManager.S.ChangeScreen(toScreen);
    }
}
