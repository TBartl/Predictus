using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityOpenOBJ : MonoBehaviour {
    
    public static Mesh OpenOBJ() {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) {
            // Here you'll want to open up the File Dialogue to navigate to a .obj file
            // I've got to be honest with you on this one, I have absolutely no idea how you would do this
            // Instead of googling for a Unity specific tutorial, I would just look at how you can do this in C#
            // Hopefully the System function you'll end up using will lock Unity from doing anything further
            //  but if Unity continues to run let me know and I'll see if I can make this into a coroutine or something where it's used.

            return null;
        }
        else {
            return null;
        }
    }
}
