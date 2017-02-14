﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VoxSubprocessApplyEdges : VoxSubProcess {

	public int DotProduct(IntVector3 a, IntVector3 b) {
		int result = a.x * b.x + a.y * b.y + a.z * a.z;
		return result;
	}

    public override void Execute(ref VoxData voxData) {
        // The toughest piece of the voxelization process.
        // Unlike ApplyVertices, you'll now also be looking at voxData.mesh.GetIndices(0)
        // This containers of integers is formatted with triples of ints.
        // The first three ints correspond to the indices (in voxData.mesh.vertices) of the first triangle
        // The next three ints correspond to the indices of the second triangle

        // The goal with this is that you'll be able to update voxData.matrix with the faces of indices
        // You'll probably want to think about this in two steps

        // 1) Fill the cubes alongside an edge.
        //    A simple way to do this is to just Vector3.Lerp between the vertices, and fill them at intervals
        //    along the way the same way that ApplyVertices does for each point.
        //    This interval is going to be the tricky part. Infact, you may want to think about it in terms of when
        //    the point crosses a cube boundrary on either of the 3 axis.

        // 2) Fill lines from point A across line BC.
        //    You can reuse a lot of code from (1) for this. If you're hitting all of the cubes along BC,
        //    you'll also be hitting all the points along the narrower piece of the triangle. Talk to Thomas
        //    if you need more clarified about this.

        int i_a;
        int i_b;
        int i_c;
        Vector3 v_a;
        Vector3 v_b;
        Vector3 v_c;
        IntVector3 iv_a;
        IntVector3 iv_b;
        IntVector3 iv_c;

        Vector3[] vertices = voxData.mesh.vertices;
        int[] indices = voxData.mesh.GetIndices(0);
        for (int i = 0; i < indices.Length; i += 3) {
            i_a = indices[i];
            i_b = indices[i + 1];
            i_c = indices[i + 2];
            v_a = vertices[i_a];
            v_b = vertices[i_b];
            v_c = vertices[i_c];
            iv_a = voxData.TransformToIntVector(v_a);
            iv_b = voxData.TransformToIntVector(v_b);
            iv_c = voxData.TransformToIntVector(v_c);

            IntVector3 minBox = new IntVector3(
                Mathf.Min(iv_a.x, iv_b.x, iv_c.x),
                Mathf.Min(iv_a.y, iv_b.y, iv_c.y),
                Mathf.Min(iv_a.z, iv_b.z, iv_c.z));

            IntVector3 maxBox = new IntVector3(
                Mathf.Max(iv_a.x, iv_b.x, iv_c.x),
                Mathf.Max(iv_a.y, iv_b.y, iv_c.y),
                Mathf.Max(iv_a.z, iv_b.z, iv_c.z));

            for (int x = minBox.x; x <= maxBox.x; x++) {
                if (x < 0 || x >= voxData.matrix.GetLength(0))
                    continue;
                for (int y = minBox.y; y <= maxBox.y; y++) {
                    if (y < 0 || y >= voxData.matrix.GetLength(1))
                        continue;
                    for (int z = minBox.z; z <= maxBox.z; z++) {
                        if (z < 0 || z >= voxData.matrix.GetLength(2))
                            continue;
                        // TODO actually ignore all the stuff up above and just look at this
                        // This approach should be quite a bit easier and faster, so lets do this.
                        // Basically we are going to try to do this similair to how a GPU
                        //     rasterizes triangles onto the screen, since that's some fast stuff
                        // Look into the idea of baycentric coordinates on wikipedia or check this out https://fgiesen.wordpress.com/2013/02/08/triangle-rasterization-in-practice/
                        // Only do the statement below if it's contained in the triangle


//						bool isInside = true;
//
//						IntVector3 iv_p = new IntVector3 (x, y, z);
//
//						IntVector3 v0 = iv_b - iv_a, v1 = iv_c - iv_a, v2 = iv_p - iv_a;
//						float d00 = DotProduct (v0, v0);
//						float d01 = DotProduct (v0, v1);
//						float d11 = DotProduct (v1, v1);
//						float d20 = DotProduct (v2, v0);
//						float d21 = DotProduct (v2, v1);
//						float denominator = d00 * d11 - d01 * d01;
//
//						float v = (d11 * d20 - d01 * d21) / denominator;
//						float w = (d00 * d21 - d01 * d20) / denominator;
//						float u = 1.0f - v - w;
//
//						if (v < 0.0f || w < 0.0f || u < 0.0f) {
//							isInside = false;
//						}
//
//
//
//						if (isInside == true) {
//							voxData.matrix [x, y, z] = true;
//						}

						voxData.matrix [x, y, z] = true;
                    }
                }
            }
        }

    }


}
