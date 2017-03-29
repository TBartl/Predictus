using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class DepthMatrixData {

    public DepthMatrixData() { }
    public DepthMatrixData(ref VoxData voxData) {
        //The mesh voxelization matrix and depth matrix have the same X/Z sizes
        for (int x = 0; x < voxData.matrix.GetLength(0); x++) {
            for (int y = 0; y < voxData.matrix.GetLength(1); y++) {
                int depth = -1; // Negative one means that there wasn't any blocks at that point
                for (int z = 0; z < voxData.matrix.GetLength(2); z++) {
                    if (voxData.matrix[x, y, z] == true) {
                        depth = z;
                        break;
                    }
                }
                depths[x, y] = depth;
            }
        }
    }

    /// <summary> The real size (radius) in meters that serves as a bounding box for the region</summary>
    //public static Vector3 realSize = new Vector3(.105f, .2f, .32f);
    public static Vector3 realSize = new Vector3(.09f, .14f, .32f);

    /// <summary> The size (radius) in boxes that serves as the used bounding box for the region</summary>
    public static IntVector3 size = new IntVector3(
        Mathf.CeilToInt(realSize.x / VoxData.scale),
        Mathf.CeilToInt(realSize.y / VoxData.scale),
        Mathf.CeilToInt(realSize.z / VoxData.scale));

    public int[,] depths = new int[size.x * 2 + 1, size.y * 2 + 1];

    public int GetWidth() {
        return depths.GetLength(0);
    }
    public int GetHeight() {
        return depths.GetLength(1);
    }

    public void SaveAsPNG(string name) {
        Texture2D texture = new Texture2D(depths.GetLength(0), depths.GetLength(1), TextureFormat.RGB24, false);

        int maxDepth = -1;
        for (int x = 0; x < GetWidth(); x++) {
            for (int y = 0; y < GetHeight(); y++) {
                maxDepth = Math.Max(maxDepth, depths[x, y]);
            }
        }

        for (int x = 0; x < texture.width; x++) {
            for (int y = 0; y < texture.height; y++) {
                Color color = Color.black;
                float saturation = Mathf.Abs(depths[x, y] / (float)maxDepth);
                if (depths[x, y] < 0)
                    color = new Color(saturation, 0, 0);
                if (depths[x, y] > 0)
                    color = new Color(0, saturation, 0);
                texture.SetPixel(x, y, color);
            }
        }
        File.WriteAllBytes(name + ".png", texture.EncodeToPNG());
    }

    public void Export(string path) {
		//export as a giant 1-d array to a file
		using (StreamWriter w = File.AppendText (path)) {
			int width = depths.GetLength (0);
			int height = depths.GetLength (1);
			//first write width and height to the file
			w.WriteLine (width);
			w.WriteLine (height);
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					w.WriteLine (depths [x, y]);
				}
			}
			w.Close ();
		}
    }
    public DepthMatrixData Import(string path) {
		DepthMatrixData ImportMatrix = new DepthMatrixData ();
		StreamReader r = new StreamReader (path);
		using (r) {
			int width = int.Parse (r.ReadLine ());
			int height = int.Parse (r.ReadLine ());
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					ImportMatrix.depths [x, y] = int.Parse (r.ReadLine ());
				}
			}
			r.Close ();
		}
		return ImportMatrix;
    }

	public DepthMatrixData Composite(List<DepthMatrixData> allLibraryDepths) {
		//average all DepthMatrixData
		DepthMatrixData AllMatrix = new DepthMatrixData ();
		int num = allLibraryDepths.Count; //size of the list
		int width = allLibraryDepths [0].depths.GetLength (0);
		int height = allLibraryDepths [0].depths.GetLength (1);
		foreach (DepthMatrixData m in allLibraryDepths) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					AllMatrix.depths [x, y] += m.depths [x, y];
				}
			}
		}
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				AllMatrix.depths [x, y] /= num;
			}
		}
		return AllMatrix;
	}
}

