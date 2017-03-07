using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientInputMesh : MonoBehaviour {

    bool modifyPositionNotRotation = true;

    void Update() {
        // Eventually we'll want to have this as a UI button, but for now just press space to change between position and rotation changes
        if (Input.GetKeyDown(KeyCode.Space))
            modifyPositionNotRotation = !modifyPositionNotRotation;

        if (modifyPositionNotRotation)
            UpdatePosition();
        else
            UpdateRotation();
    }

    void UpdatePosition() {
        // Here you'll want to implement some way for the user to translate this object around.
        // There's a lot of ways to do this and it's pretty up to you, but I would look at something like the Unity Editor's system
        //   where you just have grabbable arrows you can grab to drag the object on a given axis.

        // Input.GetMouseButtonDown(0) returns true the frame the left mouse button is pressed
        // Input.GetMouseButton(0) returns true every frame the left mouse button is held down
        // This object has a capsule collider and is on layer "Rotatable"
        //   you may want to look into raycasting to see if a mouse is hovering over this object.
        // Changing the position is as simple as doing something like this.transform.position += Vector3.up * Time.deltaTime;
    }

    void UpdateRotation() {

    }
}
