using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Crate")]
public class Crate : ScriptableObject
{
    public string Barcode = "THISWILLAUTOCREATE";
    public string Title;
    public string Description;
    public bool Redacted = false;
    public string[] Tags;

    public GameObject CrateSpawnable;

    [HideInInspector] public Mesh combinedMesh; // Combined mesh for the crate.
    [HideInInspector] public Material analogMaterial; // Material for visualization.

    private string analogMaterialPath = "Assets/Analog SDK/Materials/ANALOG.mat";
    private string saveFolderPath = "Assets/Analog SDK/Crates"; // Folder to save generated meshes

    // Method to regenerate the combined mesh
    public void RegenerateCombinedMesh()
    {
        if (CrateSpawnable == null)
        {
            Debug.LogWarning("No crate prefab set.");
            return;
        }

        // Collect all mesh renderers in the prefab
        MeshFilter[] meshFilters = CrateSpawnable.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        foreach (var meshFilter in meshFilters)
        {
            combine[i].mesh = meshFilter.sharedMesh;
            combine[i].transform = meshFilter.transform.localToWorldMatrix;
            i++;
        }

        // Create the combined mesh
        combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine, true, true);

        // Load the ANALOG material from the custom path
        analogMaterial = AssetDatabase.LoadAssetAtPath<Material>(analogMaterialPath);

        if (analogMaterial == null)
        {
            Debug.LogWarning("ANALOG material not found at the specified path.");
        }
        else
        {
            Debug.Log("Loaded ANALOG material.");
        }

        Debug.Log("Combined mesh regenerated.");
    }

    // Method to save the mesh to the specified folder
    public void SaveMeshToFolder()
    {
        if (combinedMesh == null)
        {
            Debug.LogWarning("No combined mesh to save.");
            return;
        }

        // Create the save path using crate's name
        string meshPath = $"{saveFolderPath}/{name}_CombinedMesh.asset";

        // Check if the folder exists, if not, create it
        if (!AssetDatabase.IsValidFolder(saveFolderPath))
        {
            AssetDatabase.CreateFolder("Assets/Analog SDK", "Crates");
        }

        // Save the mesh asset to the folder
        AssetDatabase.CreateAsset(combinedMesh, meshPath);
        AssetDatabase.SaveAssets();

        Debug.Log($"Mesh saved to: {meshPath}");
    }

    // Button functionality to regenerate the mesh and save it
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnCrateSelected()
    {
        if (Selection.activeObject is Crate crate)
        {
            crate.RegenerateCombinedMesh(); // Automatically regenerate combined mesh on selection
        }
    }
}
