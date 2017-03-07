using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonChangeScene : MonoBehaviour {

    public int scene;

    public void OnClick() {
        SceneManager.LoadScene(scene);
    }
}
