using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class LibraryContent : MonoBehaviour {

	public static LibraryContent LC;

	string saveFolder;
	public GameObject modelButton;

	public MeshFilter meshToDrawTo;

	GameObject selectedButton;
	public GameObject deleteButton;

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
		FileInfo[] info = dir.GetFiles ("*.obj");

		//int index = 2;
		foreach (FileInfo f in info) {
			GameObject newButton = Instantiate (modelButton);
			newButton.transform.SetParent(transform);
			ButtonSelectModel buttonScript = newButton.GetComponent<ButtonSelectModel> ();
			buttonScript.savePath = f.ToString ();
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
	}

	void CreateFile() {

	}

	public void SelectButton(GameObject buttonSelect) {
		if (selectedButton != null) {
			selectedButton.transform.GetChild (0).gameObject.SetActive (false);
		}
		deleteButton.SetActive (true);
		selectedButton = buttonSelect;
		ButtonSelectModel buttonSelectComponent = buttonSelect.GetComponent<ButtonSelectModel> ();
		string path = buttonSelectComponent.savePath;
		buttonSelectComponent.selectedText.gameObject.SetActive (true);
		meshToDrawTo.mesh = UtilityOpenOBJ.S.parseOBJ (path);
	}

	public void DeleteModel() {
		Destroy (selectedButton);
		deleteButton.SetActive (false);
		meshToDrawTo.mesh = null;
	}
}
