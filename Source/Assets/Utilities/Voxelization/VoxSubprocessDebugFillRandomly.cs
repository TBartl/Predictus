using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VoxSubprocessDebugFillRandomly : VoxSubProcess {
    public float fillChance = .3f;

    public override void Execute(ref VoxData voxData) {
        for (int z = 0; z < voxData.matrix.GetLength(2); z++) {
            for (int y = 0; y < voxData.matrix.GetLength(1); y++) {
                for (int x = 0; x < voxData.matrix.GetLength(0); x++) {
                    if (Random.value <= fillChance)
                        voxData.matrix[x, y, z] = true;
                }
            }
        }
    }
}
