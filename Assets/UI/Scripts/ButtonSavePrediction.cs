using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSavePrediction : MonoBehaviour {

	public MeshFilter beforeMesh;
	public MeshFilter afterMesh;

	public void OnClick() {
//		if (beforeMesh.mesh == null)
//			Debug.Log ("1");
//		else if (afterMesh.mesh == null)
//			Debug.Log ("2");
//		else if (beforeMesh == null)
//			Debug.Log ("3");
//		else if (afterMesh == null)
//			Debug.Log ("3");
		LibraryContent.LC.SaveTwoFilesIntoFolder (beforeMesh.mesh, afterMesh.mesh);
	}
}
