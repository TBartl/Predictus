using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientInputMesh : MonoBehaviour, Resettable {

    Vector3 initialMousePos;
    Vector3 offset;
    public bool modifyPositionNotRotation = true;
    MeshFilter meshFilter;
    public float rotationSpeed = 20f;

    void Awake() {
        meshFilter = this.GetComponent<MeshFilter>();
    }
    
    public void Reset() {
        this.transform.position = -meshFilter.mesh.bounds.center;
        ApplyMeshTranslationAndRotation();
        List<Vector3> verts = new List<Vector3>(meshFilter.mesh.vertices);
        Vector3 center = Vector3.zero;
        foreach (Vector3 vert in verts) {
            center += vert;                    
        }
        center /= verts.Count;
        this.transform.position -= center;
        ApplyMeshTranslationAndRotation();
    }

    private void OnMouseDrag() {
        Vector3 mousePos = GetMousePos();
        offset = mousePos - initialMousePos;
        offset.z = 0.0f;
        if (modifyPositionNotRotation) {
            transform.position = offset;
        }
        else {
            transform.rotation = Quaternion.Euler(0, -offset.x * rotationSpeed, 0);

        }

    }

    private void OnMouseDown() {
        Vector3 mousePos = GetMousePos();
        initialMousePos = mousePos;
    }

    Vector3 GetMousePos() {
        Vector3 mousePos = initialMousePos;
        Plane objectPlane = new Plane(Vector3.back, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayDistance;
        if (objectPlane.Raycast(ray, out rayDistance)) {
            mousePos = ray.GetPoint(rayDistance);
        }
        return mousePos;
    }

    void OnMouseUp() {
        ApplyMeshTranslationAndRotation();
    }

    public void ApplyMeshTranslationAndRotation() {
        List<Vector3> vertices = new List<Vector3>(meshFilter.mesh.vertices);
        for (int i = 0; i < vertices.Count; i++) {
            vertices[i] = transform.TransformPoint(vertices[i]);
        }
        meshFilter.mesh.SetVertices(vertices);
        meshFilter.mesh.RecalculateNormals();
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
    }
}
