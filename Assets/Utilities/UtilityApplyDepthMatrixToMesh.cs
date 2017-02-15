using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityApplyDepthMatrixToMesh : MonoBehaviour {

    public static UtilityApplyDepthMatrixToMesh S;

    void Awake() {
        S = this;
    }

	public IntVector3 TransformToIntVector(Vector3 point) { 
		Vector3 testPoint = point / VoxData.scale;        
		return new IntVector3(Mathf.RoundToInt(testPoint.x), Mathf.RoundToInt(testPoint.y), Mathf.RoundToInt(testPoint.z));
	}


    public Mesh Apply(Mesh originalMesh, DepthMatrixData depthData) {
		Vector3[] vertices = originalMesh.vertices;
		Vector3 offset = (-new Vector3(depthData.GetWidth(), depthData.GetHeight(), 0) + Vector3.one) * VoxData.scale / 2f;

		for (int i = 0; i < vertices.Length; i++) {
			IntVector3 v = TransformToIntVector (vertices [i] - offset);
			if (v.x < 0 || v.x >= depthData.GetWidth () ||
			    v.y < 0 || v.y >= depthData.GetHeight ()) {
				continue;
			} else {
				vertices [i] += (Vector3.back * VoxData.scale * depthData.depths[v.x, v.y]);
			}

		}
		Mesh updatedMesh = new Mesh(); 
		transform.gameObject.AddComponent<MeshFilter>();
		updatedMesh = GetComponent<MeshFilter> ().mesh;
		int[] indices = originalMesh.GetIndices (0);
		updatedMesh.vertices = vertices;
		updatedMesh.triangles = indices;

		updatedMesh.RecalculateBounds ();
		updatedMesh.RecalculateNormals ();
		return updatedMesh;
	
    }
		


}
