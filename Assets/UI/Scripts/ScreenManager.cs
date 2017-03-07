using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour {

    public static ScreenManager S;
    public Screen firstScreen;
    bool transitioning;
    Screen currentScreen;

    void Awake() {
        S = this;
    }

    void Start() {
        ChangeScreen(firstScreen);
    }

    public void ChangeScreen(Screen to) {
        StartCoroutine(SmoothChangeScreen(to));
    }

    IEnumerator SmoothChangeScreen(Screen to) {
        transitioning = true;
        if (currentScreen)
            yield return currentScreen.LeaveScreen();
        currentScreen = to;
        yield return to.EnterScreen();
        transitioning = false;
    }

    public bool IsTransitioning() {
        return transitioning;
    }
}
