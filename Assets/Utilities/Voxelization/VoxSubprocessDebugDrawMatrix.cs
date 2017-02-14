using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VoxSubprocessDebugDrawMatrix: VoxSubProcess {

    public Material material;

    //public override void Execute(ref VoxData voxData) {

    //    Vector3 boxScale = Vector3.one * voxData.scale;
    //    Vector3 offset = voxData.GetOffset();

    //    for (int z = 0; z < voxData.matrix.GetLength(2); z++) {
    //        for (int y = 0; y < voxData.matrix.GetLength(1); y++) {
    //            for (int x = 0; x < voxData.matrix.GetLength(0); x++) {
    //                if (voxData.matrix[x, y, z]) {
    //                    //Old code, we don't want to make a million objects
    //                    GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //                    box.transform.localScale = boxScale;
    //                    box.transform.position = new Vector3(x, y, z) * voxData.scale + offset;
    //                    if (material)
    //                        box.GetComponent<MeshRenderer>().material = material; 
    //                }
    //            }
    //        }
    //    }
    //}



    public override void Execute(ref VoxData voxData) {

        float halfUnit = VoxData.scale / 2f;

        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();


        for (int z = 0; z < voxData.matrix.GetLength(2); z++) {
            for (int y = 0; y < voxData.matrix.GetLength(1); y++) {
                for (int x = 0; x < voxData.matrix.GetLength(0); x++) {
                    if (vertices.Count > 63000) // Unity supports meshes only up to 65K verts
                        FinishMesh(ref voxData, ref vertices, ref indices);

                    if (voxData.matrix[x, y, z]) {
                        IntVector3 coordinate = new IntVector3(x, y, z);

                        Vector3 boxCenter = (Vector3)coordinate * VoxData.scale + voxData.GetOffset();
                        int start = vertices.Count;
                        vertices.Add(boxCenter + (Vector3.back + Vector3.down + Vector3.left) * halfUnit);
                        vertices.Add(boxCenter + (Vector3.back + Vector3.down + Vector3.right) * halfUnit);
                        vertices.Add(boxCenter + (Vector3.back + Vector3.up + Vector3.left) * halfUnit);
                        vertices.Add(boxCenter + (Vector3.back + Vector3.up + Vector3.right) * halfUnit);
                        vertices.Add(boxCenter + (Vector3.forward + Vector3.down + Vector3.left) * halfUnit);
                        vertices.Add(boxCenter + (Vector3.forward + Vector3.down + Vector3.right) * halfUnit);
                        vertices.Add(boxCenter + (Vector3.forward + Vector3.up + Vector3.left) * halfUnit);
                        vertices.Add(boxCenter + (Vector3.forward + Vector3.up + Vector3.right) * halfUnit);

                        if (!voxData.InRangeAndTrue(coordinate + IntVector3.Back)) {
                            vertices.Add(boxCenter + (Vector3.back + Vector3.down + Vector3.left) * halfUnit);
                            vertices.Add(boxCenter + (Vector3.back + Vector3.down + Vector3.right) * halfUnit);
                            vertices.Add(boxCenter + (Vector3.back + Vector3.up + Vector3.left) * halfUnit);
                            vertices.Add(boxCenter + (Vector3.back + Vector3.up + Vector3.right) * halfUnit);
                            AddSquareIndices(ref vertices, ref indices);
                        }

                        if (!voxData.InRangeAndTrue(coordinate + IntVector3.Left)) {
                            vertices.Add(boxCenter + (Vector3.back + Vector3.down + Vector3.left) * halfUnit);
                            vertices.Add(boxCenter + (Vector3.back + Vector3.up + Vector3.left) * halfUnit);
                            vertices.Add(boxCenter + (Vector3.forward + Vector3.down + Vector3.left) * halfUnit);
                            vertices.Add(boxCenter + (Vector3.forward + Vector3.up + Vector3.left) * halfUnit);
                            AddSquareIndices(ref vertices, ref indices);
                        }


                        if (!voxData.InRangeAndTrue(coordinate + IntVector3.Forward)) {
                            vertices.Add(boxCenter + (Vector3.forward + Vector3.down + Vector3.right) * halfUnit);
                            vertices.Add(boxCenter + (Vector3.forward + Vector3.down + Vector3.left) * halfUnit);
                            vertices.Add(boxCenter + (Vector3.forward + Vector3.up + Vector3.right) * halfUnit);
                            vertices.Add(boxCenter + (Vector3.forward + Vector3.up + Vector3.left) * halfUnit);
                            AddSquareIndices(ref vertices, ref indices);
                        }


                        if (!voxData.InRangeAndTrue(coordinate + IntVector3.Down)) {
                            vertices.Add(boxCenter + (Vector3.back + Vector3.down + Vector3.right) * halfUnit);
                            vertices.Add(boxCenter + (Vector3.back + Vector3.down + Vector3.left) * halfUnit);
                            vertices.Add(boxCenter + (Vector3.forward + Vector3.down + Vector3.right) * halfUnit);
                            vertices.Add(boxCenter + (Vector3.forward + Vector3.down + Vector3.left) * halfUnit);
                            AddSquareIndices(ref vertices, ref indices);
                        }

                        if (!voxData.InRangeAndTrue(coordinate + IntVector3.Right)) {
                            vertices.Add(boxCenter + (Vector3.back + Vector3.up + Vector3.right) * halfUnit);
                            vertices.Add(boxCenter + (Vector3.back + Vector3.down + Vector3.right) * halfUnit);
                            vertices.Add(boxCenter + (Vector3.forward + Vector3.up + Vector3.right) * halfUnit);
                            vertices.Add(boxCenter + (Vector3.forward + Vector3.down + Vector3.right) * halfUnit);
                            AddSquareIndices(ref vertices, ref indices);
                        }

                        if (!voxData.InRangeAndTrue(coordinate + IntVector3.Up)) {
                            vertices.Add(boxCenter + (Vector3.back + Vector3.up + Vector3.left) * halfUnit);
                            vertices.Add(boxCenter + (Vector3.back + Vector3.up + Vector3.right) * halfUnit);
                            vertices.Add(boxCenter + (Vector3.forward + Vector3.up + Vector3.left) * halfUnit);
                            vertices.Add(boxCenter + (Vector3.forward + Vector3.up + Vector3.right) * halfUnit);
                            AddSquareIndices(ref vertices, ref indices);
                        }
                    }
                }
            }
        }

        FinishMesh(ref voxData, ref vertices, ref indices);        
    }

    void AddSquareIndices(ref List<Vector3> vertices, ref List<int> indices) {
        int start = vertices.Count - 4;
        indices.Add(start + 0);
        indices.Add(start + 2);
        indices.Add(start + 1);
        indices.Add(start + 1);
        indices.Add(start + 2);
        indices.Add(start + 3);
    }

    void FinishMesh(ref VoxData voxData, ref List<Vector3> vertices, ref List<int> indices) {
        if (vertices.Count == 0)
            return;

        GameObject newMeshGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        Mesh m = newMeshGO.GetComponent<MeshFilter>().mesh;
        m.SetVertices(vertices);
        m.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0, true);
        m.RecalculateNormals();
        if (material != null)
            newMeshGO.GetComponent<MeshRenderer>().material = material;
        if (voxData.transform != null) {
            newMeshGO.transform.parent = voxData.transform;
            newMeshGO.transform.localPosition = Vector3.zero;
        }

        vertices = new List<Vector3>();
        indices = new List<int>();
    }
}