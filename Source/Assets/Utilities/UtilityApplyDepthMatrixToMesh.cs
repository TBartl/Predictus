using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityApplyDepthMatrixToMesh : MonoBehaviour {

	public static IntVector3 TransformToIntVector(Vector3 point) { 
		Vector3 testPoint = point / VoxData.scale;        
		return new IntVector3(Mathf.RoundToInt(testPoint.x), Mathf.RoundToInt(testPoint.y), Mathf.RoundToInt(testPoint.z));
	}


    public static Mesh Apply(Mesh originalMesh, DepthMatrixData depthData) {
		Vector3[] vertices = originalMesh.vertices;
        Vector3[] normals = originalMesh.normals;
		Vector3 offset = (-new Vector3(depthData.GetWidth(), depthData.GetHeight(), 0) + Vector3.one) * VoxData.scale / 2f;
        
		for (int i = 0; i < vertices.Length; i++) {
			IntVector3 v = TransformToIntVector (vertices [i] - offset);

            if (Vector3.Dot(normals[i], Vector3.forward) < 0)
            {
                if (v.x < 0 || v.x >= depthData.GetWidth() ||
                    v.y < 0 || v.y >= depthData.GetHeight())
                {
                    continue;
                }
                else
                {

                    if (Mathf.Abs(depthData.depths[v.x, v.y]) > 1)
                        continue;

                    int distToSide = int.MaxValue;
                    distToSide = Mathf.Min(distToSide, v.x);
                    distToSide = Mathf.Min(distToSide, depthData.GetWidth() - v.x);
                    distToSide = Mathf.Min(distToSide, v.y);
                    distToSide = Mathf.Min(distToSide, depthData.GetHeight() - v.y);
                    vertices[i] += Mathf.Clamp01(distToSide / 20f) * (Vector3.back * depthData.depths[v.x, v.y]);
                }
            }
		}
		Mesh updatedMesh = new Mesh();
		int[] indices = originalMesh.GetIndices (0);
		updatedMesh.vertices = vertices;
		updatedMesh.triangles = indices;

		updatedMesh.RecalculateBounds ();
		updatedMesh.RecalculateNormals ();
		return updatedMesh;
	
    }
		


}
