using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleFileBrowser;

public class Snapshot : MonoBehaviour {

	private bool takeHiResShot = false;
	private bool takeSaveShot = false;

    Camera cam;
	string savePath;

    void Awake() {
        cam = this.GetComponent<Camera>();
    }

	public string ScreenShotName(int width, int height) {

		return string.Format("{0}/screen_{1}x{2}_{3}.png", 
			savePath, 
			width, height, 
			System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));

//		return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png", 
//			Application.dataPath, 
//			width, height, 
//			System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
	}

	public void TakeSnapshot() {
		SoundManager.SM.PlayButtonSound ();
		StartCoroutine(WaitTakeSnapshot());
	}

	IEnumerator WaitTakeSnapshot() {

		ScreenManager.S.transitioning = true;

		yield return FileBrowser.WaitForSaveDialog (false, null, "Save File", "Save");

		if (FileBrowser.Success == false) {
			ScreenManager.S.transitioning = false;
			yield break;
		}

		savePath = FileBrowser.Result;
		savePath = savePath.Substring(0, savePath.LastIndexOf("/"));
		//Debug.Log (savePath);
		ScreenManager.S.transitioning = false;
		takeHiResShot = true;
		SoundManager.SM.PlayShutterSound ();
	}

	public void TakeSaveSnapshot(string savePathIn) {
		takeSaveShot = true;
		savePath = savePathIn;
	}

	void LateUpdate() {
		
		// snapshot button
		if (takeHiResShot) {
            RenderTexture rt = cam.targetTexture;
            RenderTexture.active = rt;
			Texture2D screenShot = new Texture2D(rt.width, rt.height);
			screenShot.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
			screenShot.Apply ();


			byte[] bytes = screenShot.EncodeToPNG();
			string filename = ScreenShotName(rt.width, rt.height);
			System.IO.File.WriteAllBytes(filename, bytes);
			takeHiResShot = false;
		}

		// saving snapshot
		if (takeSaveShot) {
			RenderTexture rt = cam.targetTexture;
			RenderTexture.active = rt;
			Texture2D screenShot = new Texture2D(rt.width, rt.height);
			screenShot.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
			screenShot.Apply ();


			byte[] bytes = screenShot.EncodeToPNG();
			string filename = savePath + "/previewImage.png";
			System.IO.File.WriteAllBytes(filename, bytes);
			takeSaveShot = false;
		}
	}
}