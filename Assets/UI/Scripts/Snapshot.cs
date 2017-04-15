using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Snapshot : MonoBehaviour {

	private bool takeHiResShot = false;

    Camera cam;

    void Awake() {
        cam = this.GetComponent<Camera>();
    }

	public string ScreenShotName(int width, int height) {
		return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png", 
			Application.dataPath, 
			width, height, 
			System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
	}

	public void TakeSnapshot() {
		SoundManager.SM.PlayButtonSound ();
		takeHiResShot = true;
	}

//	void Update() {
//		//Debug.Log(Input.GetKeyDown("k"));
//		if (Input.GetKeyDown (KeyCode.K)) {
//			TakeHiResShot ();
//			Debug.Log ("key pressed");
//		}
//	}

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

	}
}