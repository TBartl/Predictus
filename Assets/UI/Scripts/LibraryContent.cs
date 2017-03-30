using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

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
        LC = this;
	}

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

	public void LoadOneFile(string filePath) {
		GameObject newButton = Instantiate (modelButton);
		newButton.transform.SetParent(transform);
		ButtonSelectModel buttonScript = newButton.GetComponent<ButtonSelectModel> ();
		buttonScript.savePath = filePath;
		buttonScript.transform.localScale = new Vector3 (1, 1, 1);

		SelectButton (newButton);

		SaveFileIntoFolder (filePath);
	}

	void SaveFileIntoFolder(string filePath) {
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

		// Attach orient input mesh
//		OrientInputMesh orientInputMeshBefore = beforeMesh.gameObject.AddComponent<OrientInputMesh> ();
//		OrientInputMesh orientInputMeshAfter = afterMesh.gameObject.AddComponent<OrientInputMesh> ();
//		orientInputMeshBefore.modifyPositionNotRotation = true;
//		orientInputMeshAfter.modifyPositionNotRotation = true;


    public void SaveModel() {
        ButtonSelectModel buttonSelectComponent = selectedButton.GetComponent<ButtonSelectModel>();
        string path = buttonSelectComponent.savePath;
        if (beforeMesh.mesh != null) {
            UtilityExportOBJ.S.ExportMeshToOBJ(path + "/beforeClone.obj", beforeMesh.mesh);
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
    }
}
