using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;

public class ButtonExport : MonoBehaviour {

	public void OnClick() {

		StartCoroutine (SaveFileBrowser ());

	}

	IEnumerator SaveFileBrowser() {
		
		ScreenManager.S.transitioning = true;

		yield return FileBrowser.WaitForSaveDialog (false, null, "Save File", "Save");

		ScreenManager.S.transitioning = false;
	}
}
