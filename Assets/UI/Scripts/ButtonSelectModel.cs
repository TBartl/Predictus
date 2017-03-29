using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectModel : MonoBehaviour {

	public Text selectedText;
	public string savePath;

	public void OnClick() {
		selectedText.gameObject.SetActive (true);
		LibraryContent.LC.SelectButton (gameObject);
	}
}
