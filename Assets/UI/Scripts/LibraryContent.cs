using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

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

	IEnumerator LoadInFiles () {

		saveFolder = Application.dataPath + "/SaveFiles";
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
        saveFolder = Application.dataPath + "/SaveFiles";
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
    public IEnumerator GetAllEntriesCoroutine(UpdateCount updateCount, ReturnEntries returnEntries) {
        saveFolder = Application.dataPath + "/SaveFiles";
        DirectoryInfo dir = new DirectoryInfo(saveFolder);
        DirectoryInfo[] info = dir.GetDirectories();

        List<DepthMatrixData> entries = new List<DepthMatrixData>();
        foreach (DirectoryInfo d in info) {
            if (d.Name[0] == 'm' || d.Name[0] == 'M')
                continue;
            string file = saveFolder + "/" + d.Name + "/diff.txt";
            if (File.Exists(file)) {
                entries.Add(DepthMatrixData.Import(file));
                updateCount(entries.Count, -1); // Don't know how many entries until they are all loaded
                yield return null;
            }
        }
        returnEntries(entries);
    }

    

    public IEnumerator GetWeights(Mesh compareWith, UpdateCount updateCount, ReturnWeights returnWeights) {
        saveFolder = Application.dataPath + "/SaveFiles";
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
                float diffValue = 0;
                for (int x = 0; x < diff.GetWidth(); x++) {
                    for (int y = 0; y < diff.GetHeight(); y++) {
                        diffValue += Mathf.Abs(diff.depths[x, y]);
                    }
                }
                diffValues.Add(diffValue);
                updateCount(diffValues.Count, 0);
                yield return null;
            }
        }

		diffValues.Sort ();
		float max = diffValues [diffValues.Count - 1];
		float min = diffValues [0];
		foreach (float diffVal in diffValues) {
			float p = (max - diffVal) / (max - min);
			p = p * p;
			toReturn.Add (p);
		}
        //        float min = float.MaxValue;
        //        float max = 0;
        //        foreach (float diffVal in diffValues) {
        //            if (diffVal < min)
        //                min = diffVal;
        //            if (diffVal > max)
        //                max = diffVal;
        //        }
        //        float center = (min + max) / 2f;
        //        float radius = (max - min) / 2f;
        //
        //        foreach (float diffVal in diffValues) {
        //            float p = (diffVal - center) / radius;
        //            // if diffVal is positive it should be used less
        //            // if diffval is negative, it should be used more
        //            p = - p;
        //
        //            //Just go 0 to 2 for now
        //            p = 1 + p;
        //
        //            toReturn.Add(p);
        //        }

        //float pFinal = 0;
        //foreach (float p in toReturn) {
        //    pFinal += p;
        //}
        //Debug.Log(pFinal);
        returnWeights(toReturn);
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
		string savePath = Application.dataPath + "/SaveFiles/" + folderName;

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
		string savePath = Application.dataPath + "/SaveFiles/" + fileName;

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
		string savePath = Application.dataPath + "/SaveFiles/" + fileName;

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
