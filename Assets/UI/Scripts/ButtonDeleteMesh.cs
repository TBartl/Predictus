using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDeleteMesh : MonoBehaviour {

	public void OnClick() {
		LibraryContent.LC.DeleteModel ();

		// delete the obj file
	}
}
