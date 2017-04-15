using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotAndZoomCamera : MonoBehaviour {
    public float rotationSpeed;
    public float zoomSpeed;
    
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(1)) {
            Vector3 rotation = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * rotationSpeed * Time.deltaTime;
            this.transform.rotation *= Quaternion.Euler(rotation);
        }else {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.identity, .5f * Time.deltaTime);
        }
		if (!ScreenManager.S.IsTransitioning ()) {
			this.transform.localScale -= Vector3.one * Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed;

			if (this.transform.localScale.x > 4.8f)
				this.transform.localScale = Vector3.one * 4.75f;
			else if (this.transform.localScale.x < 0.7f)
				this.transform.localScale = Vector3.one * 0.75f;
		}
    }
}
