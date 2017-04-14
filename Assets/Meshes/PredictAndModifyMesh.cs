using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] public delegate void UpdateCount(int num, int outOf);
[System.Serializable] public delegate void UpdateText(string text);
[System.Serializable] public delegate void ReturnEntries(List<DepthMatrixData> entries);
[System.Serializable] public delegate void ReturnWeights(List<float> weights);


public class PredictAndModifyMesh : MonoBehaviour, Resettable{

    public LibraryContent library;
    public Text bottomText;
    public Text countText;
    public GameObject loadBar;
    MeshFilter meshFilter;

    List<DepthMatrixData> entries;

    void Awake() {
        meshFilter = this.GetComponent<MeshFilter>();
    }

    // Old composite solution
    //public void Reset() {
    //    DepthMatrixData toApply = DepthMatrixData.Composite(library.GetAllEntries());
    //    meshFilter.mesh = UtilityApplyDepthMatrixToMesh.Apply(meshFilter.mesh, toApply);
    //}

    // Weighted solution
    public void Reset() {
        loadBar.SetActive(true);
        StartCoroutine(library.GetAllEntriesCoroutine(OnBottomTextUpdated, OnCountUpdated, OnRecievedEntries));
    }

    public void OnBottomTextUpdated(string newText) {
        bottomText.text = newText;
    }

    public void OnCountUpdated(int num, int outOf) {
        int realNum = (outOf != -1) ? num : num + entries.Count;
        if (outOf == -1) // Just use entry count
            outOf = entries.Count;
        int realOutOf = 2 * outOf + 1; // n entries + n process + 1 apply
        countText.text = realNum + "/" + realOutOf;
    }

    public void OnRecievedEntries(List<DepthMatrixData> entries) {
        this.entries = entries;
        StartCoroutine(library.GetWeights(meshFilter.mesh, OnBottomTextUpdated, OnCountUpdated, OnRecievedWeights));
    }

    public void OnRecievedWeights(List<float> weights) {
        DepthMatrixData toApply = DepthMatrixData.GetWeighted(entries, weights);
        StartCoroutine(ApplyToMesh(toApply));
    }

    IEnumerator ApplyToMesh(DepthMatrixData toApply) {
        OnBottomTextUpdated("Applying to 3D model");
        OnCountUpdated(entries.Count * 2 + 1, entries.Count);
        yield return null;
        meshFilter.mesh = UtilityApplyDepthMatrixToMesh.Apply(meshFilter.mesh, toApply);
        StartCoroutine(SoundManager.SM.PlayTransformSound());
        yield return null;
        loadBar.SetActive(false);
    }
}
