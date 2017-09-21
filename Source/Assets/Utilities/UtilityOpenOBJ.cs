using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using SimpleFileBrowser;
using System;

public class UtilityOpenOBJ : MonoBehaviour {

    public TextAsset testOBJ;
    public static UtilityOpenOBJ S;

	public string openedFilePath;

    public delegate void ReturnMesh(Mesh meshToReturn);

    void Awake() {
        S = this;
    }

    public IEnumerator OpenOBJ(ReturnMesh returnMesh) {
        // A bit hacky, but this ensures no other buttons are pressed while opening a mesh.
        ScreenManager.S.transitioning = true;

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
        //string openedFilePath = EditorUtility.OpenFilePanel("obj file", Application.dataPath, "obj");


        // Set filters (optional)
        // It is sufficient to set the filters just once (instead of each time before showing the file browser dialog), 
        // if all the dialogs will be using the same filters
        FileBrowser.SetFilters(true, new FileBrowser.Filter("OBJ Files", ".obj"));

        // Set default filter that is selected when the dialog is shown (optional)
        // Returns true if the default filter is set successfully
        // In this case, set Images filter as the default filter
        FileBrowser.SetDefaultFilter(".obj");

        // Set excluded file extensions (optional) (by default, .lnk and .tmp extensions are excluded)
        // Note that when you use this function, .lnk and .tmp extensions will no longer be
        // excluded unless you explicitly add them as parameters to the function
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

        // Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
        // It is sufficient to add a quick link just once
        // Icon: default (folder icon)
        // Name: Users
        // Path: C:\Users
        FileBrowser.AddQuickLink(null, "Users", "C:\\Users");

        // Show a save file dialog 
        // onSuccess event: not registered (which means this dialog is pretty useless)
        // onCancel event: not registered
        // Save file/folder: file, Initial path: "C:\", Title: "Save As", submit button text: "Save"
        // FileBrowser.ShowSaveDialog( null, null, false, "C:\\", "Save As", "Save" );

        // Show a select folder dialog 
        // onSuccess event: print the selected folder's path
        // onCancel event: print "Canceled"
        // Load file/folder: folder, Initial path: default (Documents), Title: "Select Folder", submit button text: "Select"
        // FileBrowser.ShowLoadDialog( (path) => { Debug.Log( "Selected: " + path ); }, 
        //                                () => { Debug.Log( "Canceled" ); }, 
        //                                true, null, "Select Folder", "Select" );

        //			// Read OBJ
        //			if (openedFilePath != "") {
        //				ObjParser.Obj obj = new ObjParser.Obj ();
        //				obj.LoadObj (openedFilePath);
        //				//obj.LoadObj (Application.dataPath + "/Temp.obj");
        //
        //				// Testing purposes
        //				foreach (var vertex in obj.VertexList) {
        //					Debug.Log (vertex);
        //				}
        //				foreach (var face in obj.FaceList) {
        //					Debug.Log (face);
        //				}
        //
        //				// After reading in, use this obj object to obtain the obj parsing.
        //			}

        // Show a load file dialog and wait for a response from user
        // Load file/folder: file, Initial path: default (Documents), Title: "Load File", submit button text: "Load"
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");

        // Dialog is closed
        // Print whether a file is chosen (FileBrowser.Success)
        // and the path to the selected file (FileBrowser.Result) (null, if FileBrowser.Success is false)
        //Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);
        if (FileBrowser.Success == false) {
            ScreenManager.S.transitioning = false;
            yield break;
        }
		openedFilePath = FileBrowser.Result;
        Mesh m = parseOBJ(FileBrowser.Result);
        ScreenManager.S.transitioning = false;
        returnMesh(m);
    }

    public Mesh parseOBJ(string path) {
        if (path != "") {

            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<int> indices = new List<int>();

            using (StreamReader reader = new StreamReader(path)) {
                string line;
                char[] ignore = new char[] { ' ' };
                char[] ignoreSub = new char[] { '/' };
                while ((line = reader.ReadLine()) != null) {
                    // Do something with the line.
                    string[] parts = line.Split(ignore, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 0)
                        continue;
                    if (parts[0] == "v") {
                        Vector3 newVert = new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
                        vertices.Add(newVert);
                    }
                    else if (parts[0] == "f") {
                        for (int i = 1; i <= 3; i++) {
                            string[] subParts = parts[i].Split(ignoreSub, StringSplitOptions.RemoveEmptyEntries);
                            indices.Add(int.Parse(subParts[0]) - 1);
                        }
                    }
                }
            }
            mesh.SetVertices(vertices);
            mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.name = "Imported";
            return mesh;
        }
        else {
            return null;
        }
    }

}