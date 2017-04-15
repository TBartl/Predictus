using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveSnapshot : MonoBehaviour {

	private bool takeSaveShot = false;

	Camera cam;
	string savePath;

	void Awake() {
		cam = this.GetComponent<Camera>();
	}

	public void TakeSaveSnapshot(string savePathIn) {
		savePath = savePathIn;
		takeSaveShot = true;
	}

	void LateUpdate() {

		// snapshot button
		if (takeSaveShot) {
			RenderTexture rt = cam.targetTexture;
			RenderTexture.active = rt;
			Texture2D screenShot = new Texture2D(rt.width, rt.height);
			screenShot.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
			screenShot.Apply ();


			byte[] bytes = screenShot.EncodeToPNG();
			string filename = savePath + "image.png";
			System.IO.File.WriteAllBytes(filename, bytes);
			takeSaveShot = false;
		}
	}
}