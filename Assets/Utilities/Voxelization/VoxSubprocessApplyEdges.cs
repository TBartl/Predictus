using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VoxSubprocessApplyEdges : VoxSubProcess {

    public int DotProduct(IntVector3 a, IntVector3 b) {
        int result = a.x * b.x + a.y * b.y + a.z * a.z;
        return result;
    }

    int Orient2D(IntVector3 vc, IntVector3 vb, IntVector3 va) {
        return (vb.x - va.x) * (vc.y - va.y) - (vb.y - va.y) * (vc.x - va.x);
    }


    public override void Execute(ref VoxData voxData) {

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
                        IntVector3 p = new IntVector3(x, y, z);
                        int w0 = Orient2D(iv_b, iv_c, p);
                        int w1 = Orient2D(iv_c, iv_a, p);
                        int w2 = Orient2D(iv_a, iv_b, p);
                        Plane plane = new Plane(iv_a, iv_b, iv_c);
                        if (w0 >= 0 && w1 >= 0 && w2 >= 0 && Mathf.Abs(plane.GetDistanceToPoint(p)) < .5f)
                            voxData.matrix[x, y, z] = true;
                    }
                }
            }
        }
    }
}
