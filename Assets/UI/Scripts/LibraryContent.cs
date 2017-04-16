using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

[System.Serializable]
public struct MeshPair {
    public Mesh before;
    public Mesh after;
}

public class LibraryContent : MonoBehaviour {

	public static LibraryContent LC;

	string saveFolder;
	public GameObject modelButton;

	public MeshFilter beforeMesh;
	public MeshFilter afterMesh;

	GameObject selectedButton;
    public GameObject deleteButton;
    public GameObject saveButton;
    public List<GameObject> uploadButtons;
	public GameObject beforeOrientButton;
    public GameObject afterOrientButton;

	public Snapshot savingSnapshot;

    public List<MeshPair> firstTimeMeshes;

	public LibraryContent() {
		LC = this;
	}

    // Use this for initialization
    void Start () {
		StartCoroutine (LoadInFiles ());
        //saveFolder = Application.dataPath + "/SaveFiles";
        foreach (GameObject g in uploadButtons)
            g.SetActive(false);
        beforeOrientButton.SetActive(false);
        afterOrientButton.SetActive(false);
        saveButton.SetActive(false);
        deleteButton.SetActive(false);
        //LC = this;
	}



    //TODO reimplement these
	//void OnEnable() {
	//	ClearFiles ();
	//	StartCoroutine(LoadInFiles());
	//}

	//void ClearFiles() {
	//	foreach (Transform child in transform) {
	//		if (child.name != "AddNew")
	//			Destroy (child.gameObject);
	//	}
	//}

    public void SetupLibraryFirstTimeIfNeeded() {
        saveFolder = Application.persistentDataPath + "/SaveFiles";
        if (!Directory.Exists(Application.persistentDataPath) || !Directory.Exists(saveFolder)) {
            if (!Directory.Exists(Application.persistentDataPath))
                Directory.CreateDirectory(Application.persistentDataPath);
            Directory.CreateDirectory(saveFolder);
            for (int i = 0; i < firstTimeMeshes.Count; i++) {
                string thisFolder = saveFolder + "/builtin" + i.ToString();
                Directory.CreateDirectory(thisFolder);
                UtilityExportOBJ.S.ExportMeshToOBJ(thisFolder + "/before.obj", firstTimeMeshes[i].before);
                UtilityExportOBJ.S.ExportMeshToOBJ(thisFolder + "/after.obj", firstTimeMeshes[i].after);

                DepthMatrixData fromDepths = DepthMatrixData.GetFromMeshUsingRaycasts(firstTimeMeshes[i].before);
                fromDepths.SaveAsPNG(thisFolder + "/from");
                DepthMatrixData toDepths = DepthMatrixData.GetFromMeshUsingRaycasts(firstTimeMeshes[i].after);
                toDepths.SaveAsPNG(thisFolder + "/to");
                DepthMatrixData diff = UtilityCompareDepthMatrices.Compare(fromDepths, toDepths);
                diff.SaveAsPNG(thisFolder + "/diff");
                diff.Export(thisFolder + "/diff.txt");
            }
        }
    }

	IEnumerator LoadInFiles () {
        DirectoryInfo dir = new DirectoryInfo (saveFolder);
		DirectoryInfo[] info = dir.GetDirectories ();

		//int index = 2;
		foreach (DirectoryInfo d in info) {
			if (d.Name[0] == 'm' || d.Name[0] == 'M')
                continue;
			GameObject newButton = Instantiate (modelButton);
			newButton.transform.SetParent(transform);
			ButtonSelectModel buttonScript = newButton.GetComponent<ButtonSelectModel> ();
			buttonScript.savePath = d.ToString ();
			buttonScript.transform.localScale = new Vector3 (1, 1, 1);

			byte[] bytes;
			if (File.Exists (buttonScript.savePath + "/previewImage.png")) {
				bytes = File.ReadAllBytes (buttonScript.savePath + "/previewImage.png");
				Texture2D texture = new Texture2D (512, 256);
				texture.filterMode = FilterMode.Trilinear;
				texture.LoadImage (bytes);
				Sprite sprite = Sprite.Create (texture, new Rect (0, 0, 512, 256), new Vector2 (0.5f, 0.0f), 1.0f);

				buttonScript.image.sprite = sprite;
			}

//			int yPosition = 50 - (180 * index);
//
//			Vector3 tempPos = transform.position;
//			tempPos.y = yPosition;
//			tempPos.x = 0;
//			tempPos.z = 0;
//
//			newButton.transform.position = tempPos;

			yield return null;
		}
	}

    public List<DepthMatrixData> GetAllEntries() {

        DirectoryInfo dir = new DirectoryInfo(saveFolder);
        DirectoryInfo[] info = dir.GetDirectories();

        List<DepthMatrixData> entries = new List<DepthMatrixData>();
        foreach (DirectoryInfo d in info) {
            if (d.Name[0] == 'm' || d.Name[0] == 'M')
                continue;
            string file = saveFolder + "/" + d.Name + "/diff.txt";
            if (File.Exists(file)) {
                entries.Add(DepthMatrixData.Import(file));
            }
        }
        return entries;
    }

    // It's pretty bad practice to copy and paste code
    // but I'm a renegade cop who doesn't care about the rules
    public IEnumerator GetAllEntriesCoroutine(UpdateText updateText, UpdateCount updateCount, ReturnEntries returnEntries) {

        List<DepthMatrixData> entries = new List<DepthMatrixData>();
        List<Mesh> befores = new List<Mesh>();
        List<Mesh> afters = new List<Mesh>();

        saveFolder = Application.persistentDataPath + "/SaveFiles";
        DirectoryInfo dir = new DirectoryInfo(saveFolder);
        DirectoryInfo[] info = dir.GetDirectories();

        int entryCount = 0;
        foreach (DirectoryInfo d in info) {
            if (d.Name[0] == 'm' || d.Name[0] == 'M')
                continue;
            string file = saveFolder + "/" + d.Name + "/diff.txt";
            if (File.Exists(file)) {
                entryCount += 1;
            }
        }

        foreach (DirectoryInfo d in info) {
            if (d.Name[0] == 'm' || d.Name[0] == 'M')
                continue;
            string file = saveFolder + "/" + d.Name + "/diff.txt";
            string fileBefore = saveFolder + "/" + d.Name + "/before.obj";
            string fileAfter = saveFolder + "/" + d.Name + "/after.obj";
            if (File.Exists(file)) {
                entries.Add(DepthMatrixData.Import(file));
                befores.Add(UtilityOpenOBJ.S.parseOBJ(fileBefore));
                afters.Add(UtilityOpenOBJ.S.parseOBJ(fileAfter));
                updateCount(entries.Count, entryCount);
                updateText("Loading Models");
                yield return null;
            }
        }
        returnEntries(entries, befores, afters);
    }

    

    public IEnumerator GetWeights(Mesh compareWith, UpdateText updateText, UpdateCount updateCount, ReturnWeightsAndConfidence returnWeights) {
        saveFolder = Application.persistentDataPath + "/SaveFiles";
        DirectoryInfo dir = new DirectoryInfo(saveFolder);
        DirectoryInfo[] info = dir.GetDirectories();

        DepthMatrixData a = DepthMatrixData.GetFromMeshUsingRaycasts(compareWith);

        List<float> diffValues = new List<float>();
        List<float> toReturn = new List<float>();
        foreach (DirectoryInfo d in info) {
            if (d.Name[0] == 'm' || d.Name[0] == 'M')
                continue;
            string meshFile = saveFolder + "/" + d.Name + "/before.obj";
            string txtFile = saveFolder + "/" + d.Name + "/diff.txt";
            if (File.Exists(txtFile)) {
                DepthMatrixData b = DepthMatrixData.GetFromMeshUsingRaycasts(UtilityOpenOBJ.S.parseOBJ(meshFile));
                DepthMatrixData diff = UtilityCompareDepthMatrices.Compare(a, b);
                float diffValue = diff.GetDiffValOfTwo();
                diffValues.Add(diffValue);
                Debug.Log(diffValue);
                updateCount(diffValues.Count, -1);
                updateText("Processing Models");
                yield return null;
            }
        }

        float min = float.MaxValue;
        float max = 0;
        foreach (float diffVal in diffValues) {
            if (diffVal < min)
                min = diffVal;
            if (diffVal > max)
                max = diffVal;
        }
  //      foreach (float diffVal in diffValues) {
		//	float p = (max - diffVal) / (max - min);
		//	p = p * p;
		//	toReturn.Add (p);
		//}
        float center = (min + max) / 2f;
        float radius = (max - min) / 2f;

        foreach (float diffVal in diffValues) {
            float p = (diffVal - center) / radius;
            // if diffVal is positive it should be used less
            // if diffval is negative, it should be used more
            p = -p;

            //Just go 0 to 2 for now
            p = 1 + p;

            toReturn.Add(p);
        }

        //float pFinal = 0;
        //foreach (float p in toReturn) {
        //    pFinal += p;
        //}
        //Debug.Log(pFinal);

        float confidence = 0;
		float upper = 400;
		float lower = 100;
		foreach (float diffVal in diffValues) {
			if (diffVal > upper)
				continue;
			confidence += 2 * ((upper - diffVal) / (upper - lower)); //0-2 range
		}
		Debug.Log ("Confidence");
		Debug.Log (confidence);
		//scale 0-2:OK, 2-5: GOOD
        returnWeights(toReturn, confidence);
    }

    public void LoadOneFile(string filePath) {
		GameObject newButton = Instantiate (modelButton);
		newButton.transform.SetParent(transform);
		ButtonSelectModel buttonScript = newButton.GetComponent<ButtonSelectModel> ();
		buttonScript.savePath = filePath;
		buttonScript.transform.localScale = new Vector3 (1, 1, 1);

		SelectButton (newButton);

		CopyFileIntoFolder (filePath);
	}

	void CopyFileIntoFolder(string filePath) {
		FileInfo targetFile = new FileInfo (filePath);

		string folderName = filePath.Substring(filePath.LastIndexOf('/') + 1, (filePath.LastIndexOf('.') - (filePath.LastIndexOf('/') + 1)));
		string savePath = Application.persistentDataPath + "/SaveFiles/" + folderName;

		int index = 1;
		string tempPath = savePath;
		while (Directory.Exists(tempPath)) {
			tempPath = savePath + " (" + index + ")";
			index++;
		}
		savePath = tempPath;

		savePath = savePath + "/";
		Directory.CreateDirectory (savePath);

		targetFile.CopyTo (savePath + folderName + ".obj");
	}

	// Not currently being used
	public void SaveNewFileIntoFolder(Mesh newMesh, string name) {

		string prevPath = UtilityOpenOBJ.S.openedFilePath;
		string fileName = prevPath.Substring (prevPath.LastIndexOf ("/") + 1, (prevPath.LastIndexOf(".") - prevPath.LastIndexOf("/") - 1));
		string savePath = Application.persistentDataPath + "/SaveFiles/" + fileName;

		int index = 1;
		string tempPath = savePath;
		while (Directory.Exists(tempPath)) {
			tempPath = savePath + " (" + index + ")";
			index++;
		}
		savePath = tempPath;
		Directory.CreateDirectory (savePath);

		savePath = savePath + "/" + name + ".obj";
		UtilityExportOBJ.S.ExportMeshToOBJ (savePath, newMesh);
	}

	public void SaveTwoFilesIntoFolder (Mesh newMesh1, Mesh newMesh2) {
//		SaveNewFileIntoFolder(newMesh1, "before");
//		SaveNewFileIntoFolder(newMesh2, "after");

		string prevPath = UtilityOpenOBJ.S.openedFilePath;
		string fileName = prevPath.Substring (prevPath.LastIndexOf ("/") + 1, (prevPath.LastIndexOf(".") - prevPath.LastIndexOf("/") - 1));
		string savePath = Application.persistentDataPath + "/SaveFiles/" + fileName;

		int index = 1;
		string tempPath = savePath;
		while (Directory.Exists(tempPath)) {
			tempPath = savePath + " (" + index + ")";
			index++;
		}
		savePath = tempPath;
		Directory.CreateDirectory (savePath);

		string savePath1 = savePath + "/before.obj";
		UtilityExportOBJ.S.ExportMeshToOBJ (savePath1, newMesh1);

		savePath1 = savePath + "/after.obj";
		UtilityExportOBJ.S.ExportMeshToOBJ (savePath1, newMesh2);
	}


    public void SelectButton(GameObject buttonSelect) {
        if (selectedButton != null) {
            selectedButton.transform.GetChild(0).gameObject.SetActive(false);
        }
        deleteButton.SetActive(true);
        saveButton.SetActive(true);
        selectedButton = buttonSelect;
        ButtonSelectModel buttonSelectComponent = buttonSelect.GetComponent<ButtonSelectModel>();
        buttonSelectComponent.selectedText.gameObject.SetActive(true);

        foreach (GameObject g in uploadButtons)
            g.SetActive(true);
        beforeOrientButton.SetActive(false);
        afterOrientButton.SetActive(false);
        beforeMesh.mesh = null;
        afterMesh.mesh = null;

        string path = buttonSelectComponent.savePath;
        if (File.Exists(path + "/before.obj")) {
            beforeMesh.mesh = UtilityOpenOBJ.S.parseOBJ(path + "/before.obj");
            beforeOrientButton.SetActive(true);
        }
        if (File.Exists(path + "/after.obj")) {
            afterMesh.mesh = UtilityOpenOBJ.S.parseOBJ(path + "/after.obj");
            afterOrientButton.SetActive(true);
        }
    }



    public void SaveModel() {
        ButtonSelectModel buttonSelectComponent = selectedButton.GetComponent<ButtonSelectModel>();
        string path = buttonSelectComponent.savePath;
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        if (beforeMesh.sharedMesh != null) {
            UtilityExportOBJ.S.ExportMeshToOBJ(path + "/before.obj", beforeMesh.mesh);
        }
        if (afterMesh.sharedMesh != null) {
            UtilityExportOBJ.S.ExportMeshToOBJ(path + "/after.obj", afterMesh.mesh);
        }
        if (beforeMesh.sharedMesh != null && afterMesh.sharedMesh != null) {
            DepthMatrixData fromDepths = DepthMatrixData.GetFromMeshUsingRaycasts(beforeMesh.mesh);
            fromDepths.SaveAsPNG(path + "/from");
            DepthMatrixData toDepths = DepthMatrixData.GetFromMeshUsingRaycasts(afterMesh.mesh);
            toDepths.SaveAsPNG(path + "/to");
            DepthMatrixData diff = UtilityCompareDepthMatrices.Compare(fromDepths, toDepths);
            diff.SaveAsPNG(path + "/diff");
            diff.Export(path + "/diff.txt");
        }

		// For the preview image
		if (beforeMesh.sharedMesh != null || afterMesh.sharedMesh != null) {
			StartCoroutine (ChangeButtonImage (path, buttonSelectComponent));
		}
    } 

	IEnumerator ChangeButtonImage(string path, ButtonSelectModel buttonSelectComponent) {
		savingSnapshot.TakeSaveSnapshot (path);

		yield return 0;

		byte[] bytes = File.ReadAllBytes(buttonSelectComponent.savePath + "/previewImage.png");
		Texture2D texture = new Texture2D(512, 256);
		texture.filterMode = FilterMode.Trilinear;
		texture.LoadImage(bytes);
		Sprite sprite = Sprite.Create(texture, new Rect(0,0,512, 256), new Vector2(0.5f,0.0f), 1.0f);

		buttonSelectComponent.image.sprite = sprite;
	}

	public void DeleteModel() {
		Destroy (selectedButton);
		deleteButton.SetActive (false);
        saveButton.SetActive(false);
        beforeMesh.mesh = null;
		afterMesh.mesh = null;
        foreach (GameObject g in uploadButtons)
            g.SetActive(false);
        beforeOrientButton.SetActive(false);
        afterOrientButton.SetActive(false);
        beforeMesh.mesh = null;
        afterMesh.mesh = null;
        Directory.Delete(selectedButton.GetComponent<ButtonSelectModel>().savePath, true);
    }

    public void AddNewFolder() {
        GameObject newButton = Instantiate(modelButton);
        newButton.transform.SetParent(transform);
        ButtonSelectModel buttonScript = newButton.GetComponent<ButtonSelectModel>();
        buttonScript.savePath = saveFolder + "/" + 
            System.DateTime.Now.Month.ToString() + "." +
            System.DateTime.Now.Day.ToString() + "_" +
            System.DateTime.Now.Hour.ToString() + "." +
            System.DateTime.Now.Month.ToString() + "." +
            System.DateTime.Now.Second.ToString();

        buttonScript.transform.localScale = new Vector3(1, 1, 1);
        SelectButton(newButton);
    }
}
