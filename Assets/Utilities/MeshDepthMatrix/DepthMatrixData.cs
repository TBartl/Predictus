using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class DepthMatrixData {

    /// <summary> The real size (radius) in meters that serves as a bounding box for the region</summary>
    public static Vector3 realSize = new Vector3(.105f, .2f, .32f);

    /// <summary> The size (radius) in boxes that serves as the used bounding box for the region</summary>
    public static IntVector3 size = new IntVector3(
        Mathf.CeilToInt(realSize.x / VoxData.scale),
        Mathf.CeilToInt(realSize.y / VoxData.scale),
        Mathf.CeilToInt(realSize.z / VoxData.scale));

    public int[,] depths = new int[size.x * 2 + 1, size.y * 2 + 1];

    public void SaveAsPNG() {
        Texture2D texture = new Texture2D(depths.GetLength(0), depths.GetLength(1), TextureFormat.RGB24, false);
        for (int x = 0; x < texture.width; x++) {
            for (int y = 0; y < texture.height; y++) {
                texture.SetPixel(x, y, Color.black);
            }
        }
        File.WriteAllBytes("depthMatrix.png", texture.EncodeToPNG());
    }

}
