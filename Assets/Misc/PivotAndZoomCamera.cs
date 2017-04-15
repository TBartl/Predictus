using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotAndZoomCamera : MonoBehaviour {
    public float rotationSpeed;
    public float zoomSpeed;
    
	// Update is called once per frame
	void Update () {
		if (!ScreenManager.S.IsTransitioning ()) {

			#if UNITY_IOS
			HandleTouchZooming();
			#elif UNITY_ANDROID
			HandleTouchZooming();
			#else
			HandleMouseZooming();

			#endif

			if (this.transform.localScale.x > 4.8f)
				this.transform.localScale = Vector3.one * 4.75f;
			else if (this.transform.localScale.x < 0.7f)
				this.transform.localScale = Vector3.one * 0.75f;
		}
    }

	void HandleMouseZooming() {
		if (Input.GetMouseButton(1)) {
			Vector3 rotation = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * rotationSpeed * Time.deltaTime;
			this.transform.rotation *= Quaternion.Euler(rotation);
		}else {
			this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.identity, .5f * Time.deltaTime);
		}

		this.transform.localScale -= Vector3.one * Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed;
	}

	void HandleTouchZooming() {
		int fingercount = 0;
		foreach (Touch touch in Input.touches) {
			if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled) {
				fingercount++;
			}
		}

		if (fingercount == 2) {

			// Store both touches.
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			this.transform.localScale -= Vector3.one * deltaMagnitudeDiff * (zoomSpeed/10.0f);
		}
	}
}
