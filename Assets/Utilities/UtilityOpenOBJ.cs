using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;

public class UtilityOpenOBJ : MonoBehaviour {

	public TextAsset testOBJ;
    
    public static Mesh OpenOBJ() {

		// If we are using the windows platform, windows editor, or OXX editor...
		// This should be changed later.
        if (Application.platform == RuntimePlatform.WindowsPlayer 
			|| Application.platform == RuntimePlatform.WindowsEditor
			|| Application.platform == RuntimePlatform.OSXEditor) {
            // Here you'll want to open up the File Dialogue to navigate to a .obj file
            // I've got to be honest with you on this one, I have absolutely no idea how you would do this
            // Instead of googling for a Unity specific tutorial, I would just look at how you can do this in C#
            // Hopefully the System function you'll end up using will lock Unity from doing anything further
            //  but if Unity continues to run let me know and I'll see if I can make this into a coroutine or something where it's used.

            // Once you've actually opened the file it should be readable just like any other text file
            // There are a ton of tutorials on line that explain the 3D OBJ format so look to those first.
            // Unfortunately there are some different interpertations of the format and what should be included,
            //  so you may want to look at the OBJ's in the box folder to see how their program outputs it

            // Finally we'll (Chuxuan) need to make a Mesh out of this
            // In addition to writing the vertices and indices to a new mesh, you'll also want to make sure you recalculate the mesh bounds and normals


			// This line opens the window and allows the user to find a file with "obj" as its extension
			string openedFilePath = EditorUtility.OpenFilePanel("obj file", Application.dataPath, "obj");


			// Read OBJ
			if (openedFilePath != "") {
				ObjParser.Obj obj = new ObjParser.Obj ();
				obj.LoadObj (openedFilePath);
				//obj.LoadObj (Application.dataPath + "/Temp.obj");

				// Testing purposes
				foreach (var vertex in obj.VertexList) {
					Debug.Log (vertex);
				}
				foreach (var face in obj.FaceList) {
					Debug.Log (face);
				}

				// After reading in, use this obj object to obtain the obj parsing.
			}

			return null;
        }
        else {
            return null;
        }
    }


}
