using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotAndZoomCamera : MonoBehaviour {
    Vector3 rotation = Vector3.zero;
    public float rotationSpeed;
    public float zoomSpeed;
    
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(1)) {
            rotation += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * rotationSpeed;
            this.transform.rotation = Quaternion.Euler(rotation);
        }
        this.transform.localScale -= Vector3.one * Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
    }
}
