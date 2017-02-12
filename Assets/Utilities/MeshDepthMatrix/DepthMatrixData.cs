using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DepthMatrixData {

    /// <summary> The real size (radius) in meters that serves as a bounding box for the region</summary>
    public static Vector3 realSize = new Vector3(.105f, .35f, .2f);

    /// <summary> The size (radius) in boxes that serves as the used bounding box for the region</summary>
    public static IntVector3 size = new IntVector3(
        Mathf.CeilToInt(realSize.x / VoxData.scale),
        Mathf.CeilToInt(realSize.y / VoxData.scale),
        Mathf.CeilToInt(realSize.z / VoxData.scale));

    public int[,] depths = new int[size.x * 2 + 1, size.y * 2 + 1];

}
