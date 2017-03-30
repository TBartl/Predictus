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
	public GameObject orientateButtonBefore;
	public GameObject orientateButtonAfter;

	// Use this for initialization
	void Start () {
		StartCoroutine (LoadInFiles ());
		//saveFolder = Application.dataPath + "/SaveFiles";

		LC = this;
	}

	IEnumerator LoadInFiles () {

		saveFolder = Application.dataPath + "/SaveFiles";
		Debug.Log (saveFolder);
		DirectoryInfo dir = new DirectoryInfo (saveFolder);
		DirectoryInfo[] info = dir.GetDirectories ();

		//int index = 2;
		foreach (DirectoryInfo d in info) {
			if (d.ToString ().Contains ("Materials"))
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
			selectedButton.transform.GetChild (0).gameObject.SetActive (false);
		}
		deleteButton.SetActive (true);
		selectedButton = buttonSelect;
		ButtonSelectModel buttonSelectComponent = buttonSelect.GetComponent<ButtonSelectModel> ();
		buttonSelectComponent.selectedText.gameObject.SetActive (true);
		string path = buttonSelectComponent.savePath;

		DirectoryInfo dir = new DirectoryInfo (path);
		FileInfo[] info = dir.GetFiles ("*.obj");

		if (info.Length == 2) {
			beforeMesh.mesh = UtilityOpenOBJ.S.parseOBJ (info [1].ToString ());
			afterMesh.mesh = UtilityOpenOBJ.S.parseOBJ (info [0].ToString ());
			orientateButtonBefore.SetActive (true);
			orientateButtonAfter.SetActive (true);
		} else if (info.Length == 1) {
			beforeMesh.mesh = UtilityOpenOBJ.S.parseOBJ (info [0].ToString ());
			orientateButtonBefore.SetActive (true);
		} else {
			Debug.LogError ("Error: Not the correct number of obj files");
		}

		// Attach orient input mesh
//		OrientInputMesh orientInputMeshBefore = beforeMesh.gameObject.AddComponent<OrientInputMesh> ();
//		OrientInputMesh orientInputMeshAfter = afterMesh.gameObject.AddComponent<OrientInputMesh> ();
//		orientInputMeshBefore.modifyPositionNotRotation = true;
//		orientInputMeshAfter.modifyPositionNotRotation = true;
	}

	public void DeleteModel() {
		Destroy (selectedButton);
		deleteButton.SetActive (false);
		beforeMesh.mesh = null;
		afterMesh.mesh = null;
		orientateButtonAfter.SetActive (false);
		orientateButtonBefore.SetActive (false);
	}
}
