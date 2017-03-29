using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using SimpleFileBrowser;
using System;

public class UtilityCopyMesh : MonoBehaviour {

    // I want to see if this will fix an error, but hopefully we won't have to use this script in the long run
    public static Mesh Copy(Mesh original) {
        Mesh clone = new Mesh();
        clone.SetVertices(new List<Vector3>(original.vertices));
        clone.SetTriangles(new List<int>(original.triangles), 0);
        clone.RecalculateBounds();
        clone.RecalculateNormals();
        clone.name = original.name + " (clone)";
        return clone;
    }
}