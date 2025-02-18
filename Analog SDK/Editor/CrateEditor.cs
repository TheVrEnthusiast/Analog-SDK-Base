using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Crate))]
public class CrateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Crate crate = (Crate)target;

        if (GUILayout.Button("Generate and Save Combined Mesh"))
        {
            crate.RegenerateCombinedMesh();
            crate.SaveMeshToFolder(); 
        }
    }
}
