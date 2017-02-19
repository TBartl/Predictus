using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityOpenOBJ : MonoBehaviour {
    
    public static Mesh OpenOBJ() {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) {
            return null;
        } else {
            return null;
        }
    }
}
