using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] public delegate void UpdateCount(int num, int outOf);
[System.Serializable] public delegate void UpdateText(string text);
[System.Serializable] public delegate void ReturnEntries(List<DepthMatrixData> entries, List<Mesh> befores, List<Mesh> afters);
[System.Serializable] public delegate void ReturnWeightsAndConfidence(List<float> weights, float confidence);

[System.Serializable]
public class SimilairMeshInfo {
    public MeshFilter before;
    public MeshFilter after;
    public Text text;
}

public class PredictAndModifyMesh : MonoBehaviour, Resettable{

    public LibraryContent library;
    public Text bottomText;
    public Text countText;
	public Text confidence;
    public GameObject loadBar;
	string confidenceLevel;

    MeshFilter meshFilter;

    List<DepthMatrixData> entries;
    List<float> weights;

    public List<SimilairMeshInfo> similair;
    List<Mesh> befores;
    List<Mesh> afters;

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
        if (ScreenManager.S.noReset)
            return;
        loadBar.SetActive(true);
        StartCoroutine(library.GetAllEntriesCoroutine(OnBottomTextUpdated, OnCountUpdated, OnRecievedEntries));
        this.GetComponent<MeshRenderer>().enabled = false;
		confidence.gameObject.SetActive (false);
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

    public void OnRecievedEntries(List<DepthMatrixData> entries, List<Mesh> befores, List<Mesh> afters) {
        this.entries = entries;
        this.befores = befores;
        this.afters = afters;
        StartCoroutine(library.GetWeights(meshFilter.mesh, OnBottomTextUpdated, OnCountUpdated, OnRecievedWeights));
    }

    public void OnRecievedWeights(List<float> weights, float confidence) {
		if (confidence > 25)
			confidenceLevel = "extremely high";
		else if (confidence > 15)
			confidenceLevel = "high";
		else if (confidence > 3)
			confidenceLevel = "medium";
		else
			confidenceLevel = "low";
		
        this.weights = weights;
        DepthMatrixData toApply = DepthMatrixData.GetWeighted(entries, weights);
        StartCoroutine(ApplyToMesh(toApply));
    }

    IEnumerator ApplyToMesh(DepthMatrixData toApply) {
        OnBottomTextUpdated("Applying to 3D model");
        OnCountUpdated(entries.Count * 2 + 1, entries.Count);
        yield return null;
        meshFilter.mesh = UtilityApplyDepthMatrixToMesh.Apply(meshFilter.mesh, toApply);
        SoundManager.SM.PlayTransformSound();
        yield return null;
        loadBar.SetActive(false);
        this.GetComponent<MeshRenderer>().enabled = true;

        float totalWeight = 0;
        foreach (float weight in weights) {
            totalWeight += weight;
        }
        for (int i = 0; i < 3; i++) {
            float maxWeight = 0;
            int maxWeightIndex = -1;
            for (int j = 0; j < weights.Count; j++) {
                if (weights[j] > maxWeight) {
                    maxWeight = weights[j];
                    maxWeightIndex = j;
                }
            }

            if (maxWeightIndex == -1)
                continue;

            similair[i].before.mesh = befores[maxWeightIndex];
            similair[i].after.mesh = afters[maxWeightIndex];
            similair[i].text.text = (maxWeight / totalWeight).ToString("0.##") + "%";

            befores.RemoveAt(maxWeightIndex);
            afters.RemoveAt(maxWeightIndex);
            weights.RemoveAt(maxWeightIndex);

			confidence.gameObject.SetActive (true);
			confidence.text = "This was created with a " + confidenceLevel + " amount of confidence";
        }

    }
}
