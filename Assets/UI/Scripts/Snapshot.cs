using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Snapshot : MonoBehaviour {
	int resWidth = UnityEngine.Screen.width; 
	int resHeight = UnityEngine.Screen.height;

	private bool takeHiResShot = false;

	public string ScreenShotName(int width, int height) {
		return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png", 
			Application.dataPath, 
			width, height, 
			System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
	}

	public void TakeHiResShot() {
		takeHiResShot = true;
	}

	void Update() {
		//Debug.Log(Input.GetKeyDown("k"));
		if (Input.GetKeyDown ("k")) {
			TakeHiResShot ();
			Debug.Log ("key pressed");
		}
	}

	void LateUpdate() {
		//Debug.Log ("hi");
		if (takeHiResShot) {
			//RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
			//camera.targetTexture = rt;
			Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
			//camera.Render();
			//RenderTexture.active = rt;
			screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
			screenShot.Apply ();
			//camera.targetTexture = null;
			//RenderTexture.active = null; // JC: added to avoid errors
			//Destroy(rt);
			byte[] bytes = screenShot.EncodeToPNG();
			string filename = ScreenShotName(resWidth, resHeight);
			System.IO.File.WriteAllBytes(filename, bytes);
			Debug.Log(string.Format("Took screenshot to: {0}", filename));
			takeHiResShot = false;
		}
	}
}