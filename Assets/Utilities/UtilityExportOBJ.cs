using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using SimpleFileBrowser;
using System;

public class UtilityExportOBJ : MonoBehaviour {
    
    public static UtilityExportOBJ S;
    
    void Awake() {
        S = this;
    }

    void ExportMeshToOBJ(string path, Mesh mesh) {
		
		string data = "";
		Vector3[] vertices = mesh.vertices;
		for (int i = 0; i < vertices.Length; i++) {
			data += "v " + vertices[i].x + " " + vertices[i].y + " " + vertices[i].z + "\n";
		}

		int[] indices = mesh.triangles;
		for (int i = 0; i < indices.Length; i += 3) {
			data += "f " + indices [i] + "//" + indices [i] + " " + indices [i + 1] + "//" + indices [i + 1] + " "
				+ indices [i + 2] + "//" + indices [i + 2] + "\n";
		}

		File.WriteAllText (path, data);
    }

    //Mesh parseOBJ(string path) {
    //    if (path != "") {

    //        Mesh mesh = new Mesh();
    //        List<Vector3> vertices = new List<Vector3>();
    //        List<int> indices = new List<int>();

    //        Transform correctionTransform = new GameObject().transform;
    //        correctionTransform.rotation = Quaternion.Euler(0, 90, 90);

    //        using (StreamReader reader = new StreamReader(path)) {
    //            string line;
    //            char[] ignore = new char[] { ' ', '/' };
    //            while ((line = reader.ReadLine()) != null) {
    //                // Do something with the line.
    //                string[] parts = line.Split(ignore, StringSplitOptions.RemoveEmptyEntries);
    //                if (parts.Length == 0)
    //                    continue;
    //                if (parts[0] == "v") {
    //                    Vector3 newVert = new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
    //                    newVert = correctionTransform.TransformPoint(newVert);
    //                    vertices.Add(newVert);
    //                }
    //                else if (parts[0] == "f") {
    //                    indices.Add(int.Parse(parts[1]) - 1);
    //                    indices.Add(int.Parse(parts[3]) - 1);
    //                    indices.Add(int.Parse(parts[5]) - 1);
    //                }
    //            }
    //        }

    //        mesh.SetVertices(vertices);
    //        mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
    //        mesh.RecalculateBounds();
    //        mesh.RecalculateNormals();

    //        Destroy(correctionTransform.gameObject);
    //        return mesh;
    //    }
    //    else {
    //        return null;
    //    }
    //}

}