using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using SimpleFileBrowser;
using System;

public class ButtonAddModel : MonoBehaviour {

	public void OnClick() {

		StartCoroutine (OpenBrowser ());
	}

	IEnumerator OpenBrowser() {

		ScreenManager.S.transitioning = true;

		FileBrowser.SetFilters(true, new FileBrowser.Filter("OBJ Files", ".obj"));

		FileBrowser.SetDefaultFilter(".obj");

		FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

		FileBrowser.AddQuickLink(null, "Users", "C:\\Users");

		yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");

		if (FileBrowser.Success == false) {
			ScreenManager.S.transitioning = false;
			yield break;
		}

		Debug.Log (FileBrowser.Result);

		LibraryContent.LC.LoadOneFile (FileBrowser.Result);

		// save file into folder
	}
}
