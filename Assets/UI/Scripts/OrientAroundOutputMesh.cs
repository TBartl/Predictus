using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientAroundOutputMesh : MonoBehaviour {
	
	float rotSpeed = 200f;
	Vector3 rotation;

	void OnMouseDrag(){
		float rot_x = Input.GetAxis ("Mouse X") * rotSpeed;
		float rot_y = Input.GetAxis ("Mouse Y") * rotSpeed;
		rotation += new Vector3 (rot_y, -rot_x, 0)  * Time.deltaTime;
		this.transform.rotation = Quaternion.Euler (rotation);
			}

	public void OnClick(){
		//button to reset the rotation

			this.transform.rotation = Quaternion.identity;

	}
//    public float baseAmount;
//    public float baseSpeed;
//
//	private Vector3 screenPos;
//	private float angleOffset;
//	private bool MouseContact; //this boolean makes sure that only clicking on the object would make it rotate
//
//	void Start(){
//		MouseContact = false;
//	}
//	
//	void Update () {
//        // This function is called every frame when the output mesh is being displayed
//        // Your job is to add tools to rotate and zoom in on the model
//        // How you do this is up to you, but here's some things you may want to know about
//        // Input.GetMouseButtonDown(0) returns true the frame the left mouse button is pressed
//        // Input.GetMouseButton(0) returns true every frame the left mouse button is held down
//        // This object has a capsule collider and is on layer "Rotatable"
//        //   you may want to look into raycasting to see if a mouse is hovering over this object.
//        // For rotations, change transform.localRotation
//        // For zooming the camera in and out, you may want to consider using Camera.main.transform.position
//        //  but if you do this you'll want to reset this (look at IResettable and some of its uses)
//        
//        // You may also want to play around with adding UI buttons to do things like reset the transform
//
//        // By default, this will sort of spin just like everything else, but feel free to change that
//        bool inactive = false;
//        if (inactive) {
//            float val = Mathf.Sin(Time.timeSinceLevelLoad * baseSpeed);
//            this.transform.rotation = Quaternion.Euler(0, baseAmount * val, 0);
//        } else {
//			if(Input.GetMouseButtonDown(0)) {
//				if (MouseContact == true){
//					screenPos = Camera.main.WorldToScreenPoint (transform.position);
//					Vector3 v3 = Input.mousePosition - screenPos;
//					angleOffset = (Mathf.Atan2(transform.right.y, transform.right.x) - Mathf.Atan2(v3.y, v3.x))  * Mathf.Rad2Deg;
//				}
//			}
//			if(Input.GetMouseButton(0)) {
//				if (MouseContact == true){
//					Vector3 v3 = Input.mousePosition - screenPos;
//					float angle = Mathf.Atan2(v3.y, v3.x) * Mathf.Rad2Deg;
//					transform.eulerAngles = new Vector3(angle+angleOffset,0,0);
//				}
//			}
//        }
//    }
//
//	void OnMouseDown(){
//		MouseContact = true;
//	}
//
//	void OnMouseUp(){
//		MouseContact = false;
//	}
}
