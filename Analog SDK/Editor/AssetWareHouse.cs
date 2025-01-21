using UnityEditor;
using UnityEngine;
using System.Linq;

public class AssetWareHouse : EditorWindow
{
    private string searchQuery = "";
    private Crate[] allCrates;
    private Crate[] filteredCrates;

    [MenuItem("ANALOG SDK/AssetWareHouse")]
    public static void ShowWindow()
    {
        GetWindow<AssetWareHouse>("AssetWareHouse");
    }

    private void OnEnable()
    {
        // Load all Crate ScriptableObjects in the project
        allCrates = AssetDatabase.FindAssets("t:Crate")
            .Select(guid => AssetDatabase.LoadAssetAtPath<Crate>(AssetDatabase.GUIDToAssetPath(guid)))
            .ToArray();

        // Initially, show all crates
        filteredCrates = allCrates;
    }

    private void OnGUI()
    {
        // Search Bar
        GUILayout.BeginHorizontal();
        GUILayout.Label("Search Crates", GUILayout.Width(150));
        searchQuery = GUILayout.TextField(searchQuery);
        GUILayout.EndHorizontal();

        // Filter crates based on the search query (case insensitive)
        filteredCrates = string.IsNullOrEmpty(searchQuery)
            ? allCrates
            : allCrates.Where(crate => crate.Title.ToLower().Contains(searchQuery.ToLower())).ToArray();

        // Display the list of crates
        GUILayout.Label($"Found {filteredCrates.Length} Crates", EditorStyles.boldLabel);

        foreach (var crate in filteredCrates)
        {
            // Begin a horizontal layout for Title, Description, and Logo
            GUILayout.BeginHorizontal();

            // Title
            GUILayout.Label(crate.Title, EditorStyles.boldLabel, GUILayout.Width(200));

            // Description
            GUILayout.Label(crate.Description, GUILayout.Width(300));

          

            GUILayout.EndHorizontal();
        }
    }
}
