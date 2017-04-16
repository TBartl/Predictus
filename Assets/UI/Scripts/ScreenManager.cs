using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour {

    public static ScreenManager S;
    public Screen firstScreen;
    [HideInInspector] public bool transitioning;
    [HideInInspector] public bool noReset;
    Screen currentScreen;

    void Awake() {
        S = this;
    }

    void Start() {
        ChangeScreen(firstScreen);
        LibraryContent.LC.SetupLibraryFirstTimeIfNeeded();
    }

    public void ChangeScreen(Screen to) {
        StartCoroutine(SmoothChangeScreen(to));
    }

    IEnumerator SmoothChangeScreen(Screen to) {
        transitioning = true;
        InfoPanel.IP.HideThis();
        if (currentScreen)
            yield return currentScreen.LeaveScreen();
        currentScreen = to;
        yield return to.EnterScreen();
        transitioning = false;
        noReset = false;
    }

    public bool IsTransitioning() {
        return transitioning;
    }
}
