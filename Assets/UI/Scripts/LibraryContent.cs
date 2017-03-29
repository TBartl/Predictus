using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class LibraryContent : MonoBehaviour {

	string saveFolder;
	public GameObject modelButton;

	public MeshFilter meshToDrawTo;

	public static LibraryContent LC;

	GameObject selectedButton;

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
	}

	public void SelectButton(GameObject buttonSelect) {
		if (selectedButton != null) {
			selectedButton.transform.GetChild (0).gameObject.SetActive (false);
		}
		selectedButton = buttonSelect;
		string path = buttonSelect.GetComponent<ButtonSelectModel> ().savePath;
		meshToDrawTo.mesh = UtilityOpenOBJ.S.parseOBJ (path);
	}
}
