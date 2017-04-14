using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen : MonoBehaviour {
    static float transitionTime = .2f;
    List<CanvasRenderer> canvasItems;
    List<Resettable> itemsToReset;

    void Awake() {
        canvasItems = new List<CanvasRenderer>(this.GetComponentsInChildren<CanvasRenderer>());
        itemsToReset = new List<Resettable>(this.GetComponentsInChildren<Resettable>());
    }

    protected virtual void OnChangedToThisScene() {
        this.gameObject.SetActive(true);
        foreach (Resettable r in itemsToReset) {
            r.Reset();
        }
    }
    protected virtual void OnLeavedThisScene() {
        this.gameObject.SetActive(false);
    }

    public IEnumerator LeaveScreen() {
        for (float t = 0; t < transitionTime; t+=Time.deltaTime) {
            SetCanvasAlpha(1 - t / transitionTime);
            yield return null;
        }
        SetCanvasAlpha(0);
        OnLeavedThisScene();
    }

    public IEnumerator EnterScreen() {
        OnChangedToThisScene();
        for (float t = 0; t < transitionTime; t += Time.deltaTime) {
            SetCanvasAlpha(t / transitionTime);
            yield return null;
        }
        SetCanvasAlpha(1);
    }

    void SetCanvasAlpha(float amount) {
		canvasItems = new List<CanvasRenderer>(this.GetComponentsInChildren<CanvasRenderer>());
        foreach (CanvasRenderer canvasItem in canvasItems) {
            canvasItem.SetAlpha(amount);
        }
    }

}
