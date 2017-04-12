using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;

public class ButtonExport : MonoBehaviour {

	public MeshFilter predictedMesh;

	public void OnClick() {

		StartCoroutine (SaveFileBrowser ());

	}

	IEnumerator SaveFileBrowser() {
		
		ScreenManager.S.transitioning = true;

		yield return FileBrowser.WaitForSaveDialog (false, null, "Save File", "Save");

		if (FileBrowser.Success == false) {
			ScreenManager.S.transitioning = false;
			yield break;
		}

		string savedFilePath = FileBrowser.Result;
		ScreenManager.S.transitioning = false;

		UtilityExportOBJ.S.ExportMeshToOBJ (savedFilePath, predictedMesh.mesh);

		ScreenManager.S.transitioning = false;
	}
}
