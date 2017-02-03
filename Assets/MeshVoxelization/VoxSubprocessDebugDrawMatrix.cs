using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VoxSubprocessDebugDrawMatrix: VoxSubProcess {

    public override void Execute(ref VoxData voxData) {

        Vector3 boxScale = Vector3.one * voxData.scale;
        Vector3 offset = voxData.GetOffset();

        for (int z = 0; z < voxData.matrix.GetLength(2); z++) {
            for (int y = 0; y < voxData.matrix.GetLength(1); y++) {
                for (int x = 0; x < voxData.matrix.GetLength(0); x++) {
                    if (voxData.matrix[x, y, z]) {
                        //Old code, we don't want to make a million objects
                        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        box.transform.localScale = boxScale;
                        box.transform.position = new Vector3(x, y, z) * voxData.scale + offset;
                    }
                }
            }
        }
    }
}



















//public override void Execute(ref VoxData voxData) {

//    float halfUnit = voxData.scale / 2f;

//    List<Vector3> vertices = new List<Vector3>();
//    List<int> indices = new List<int>();


//    for (int z = 0; z < voxData.matrix.GetLength(2); z++) {
//        for (int y = 0; y < voxData.matrix.GetLength(1); y++) {
//            for (int x = 0; x < voxData.matrix.GetLength(0); x++) {
//                if (voxData.matrix[x, y, z]) {
//                    //Old code, we don't want to make a million objects
//                    //GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
//                    //box.transform.localScale = boxScale;
//                    //box.transform.position = new Vector3(x, y, z) * voxData.scale + offset;

//                    Vector3 boxCenter = new Vector3(x, y, z) * voxData.scale + voxData.GetOffset();
//                    int start = vertices.Count;
//                    vertices.Add(boxCenter + (Vector3.back + Vector3.down + Vector3.left) * halfUnit);
//                    vertices.Add(boxCenter + (Vector3.back + Vector3.down + Vector3.right) * halfUnit);
//                    vertices.Add(boxCenter + (Vector3.back + Vector3.up + Vector3.left) * halfUnit);
//                    vertices.Add(boxCenter + (Vector3.back + Vector3.up + Vector3.right) * halfUnit);
//                    vertices.Add(boxCenter + (Vector3.forward + Vector3.down + Vector3.left) * halfUnit);
//                    vertices.Add(boxCenter + (Vector3.forward + Vector3.down + Vector3.right) * halfUnit);
//                    vertices.Add(boxCenter + (Vector3.forward + Vector3.up + Vector3.left) * halfUnit);
//                    vertices.Add(boxCenter + (Vector3.forward + Vector3.up + Vector3.right) * halfUnit);

//                    indices.Add(start + 0); // Back
//                    indices.Add(start + 2);
//                    indices.Add(start + 1);
//                    indices.Add(start + 1);
//                    indices.Add(start + 2);
//                    indices.Add(start + 3);

//                    indices.Add(start + 2); // Left
//                    indices.Add(start + 0);
//                    indices.Add(start + 6);
//                    indices.Add(start + 0);
//                    indices.Add(start + 4);
//                    indices.Add(start + 6);

//                    indices.Add(start + 4); // Front
//                    indices.Add(start + 7);
//                    indices.Add(start + 6);
//                    indices.Add(start + 7);
//                    indices.Add(start + 4);
//                    indices.Add(start + 5);

//                    indices.Add(start + 4); // Bottom
//                    indices.Add(start + 0);
//                    indices.Add(start + 1);
//                    indices.Add(start + 4);
//                    indices.Add(start + 1);
//                    indices.Add(start + 5);

//                    indices.Add(start + 1); // Right
//                    indices.Add(start + 7);
//                    indices.Add(start + 5);
//                    indices.Add(start + 7);
//                    indices.Add(start + 1);
//                    indices.Add(start + 3);

//                    indices.Add(start + 2); // Top
//                    indices.Add(start + 6);
//                    indices.Add(start + 3);
//                    indices.Add(start + 6);
//                    indices.Add(start + 7);
//                    indices.Add(start + 3);
//                }
//            }
//        }
//    }

//    GameObject newMeshGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
//    Mesh m = newMeshGO.GetComponent<MeshFilter>().mesh;
//    m.SetVertices(vertices);
//    m.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0, true);
//    m.RecalculateNormals();
//}