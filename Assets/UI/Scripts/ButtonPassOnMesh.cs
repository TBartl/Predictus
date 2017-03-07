using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPassOnMesh : MonoBehaviour {

    public MeshFilter from;
    public List<MeshFilter> to;

    public void OnClick() {
		SoundManager.SM.PlayTransformSound ();
        
        foreach (MeshFilter m in to) {
            m.mesh = from.mesh;
        }
    }
}
