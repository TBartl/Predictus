using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonChangeScene : MonoBehaviour {

    public int scene;

    public void OnClick() {
        if (ScreenManager.S.IsTransitioning())
            return;
        SoundManager.SM.PlayButtonSound();
        SceneManager.LoadScene(scene);
    }
}
