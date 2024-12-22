using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class LevelCrateEditorWindow : EditorWindow
{
    private string palletTitle = "New Pallet";
    private string palletBarcode = "Barcode";
    private string palletAuthor = "Author";
    private string palletVersion = "1.0";
    private static Pallet selectedPallet;

    private string levelCrateTitle = "New Level Crate";
    private string levelCrateBarcode = "emptynullbarcode";
    private string levelCrateDescription = "Description of the level crate.";
    private List<string> levelCrateTags = new List<string>();
    private string newTag = "";
    private string levelSceneName = ""; // The scene name for the level crate

    private Vector2 scrollPosition;
    private bool isCreatingLevelCrate = false; // Flag to show level crate creation menu

    // Open the window
    [MenuItem("ANALOG SDK/Editor/LevelCrates")]
    public static void OpenWindow()
    {
        LevelCrateEditorWindow window = GetWindow<LevelCrateEditorWindow>("Level Crate Editor");
        window.Show();
    }

    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        EditorGUILayout.Space();
        GUILayout.Label("Pallet Settings", EditorStyles.boldLabel);

        palletTitle = EditorGUILayout.TextField("Pallet Title", palletTitle);
        palletBarcode = EditorGUILayout.TextField("Barcode", palletBarcode);
        palletAuthor = EditorGUILayout.TextField("Author", palletAuthor);
        palletVersion = EditorGUILayout.TextField("Version", palletVersion);

        if (GUILayout.Button("Create Pallet"))
        {
            CreatePallet();
        }

        EditorGUILayout.Space();
        GUILayout.Label("Select or Create Level Crate", EditorStyles.boldLabel);

        if (selectedPallet != null)
        {
            GUILayout.Label("Selected Pallet: " + selectedPallet.Title);

            if (GUILayout.Button("Create Level Crate"))
            {
                isCreatingLevelCrate = true; // Show level crate creation UI
            }

            if (isCreatingLevelCrate)
            {
                // Show level crate creation menu
                CreateLevelCrateUI();
            }
        }
        else
        {
            if (GUILayout.Button("Select Pallet"))
            {
                SelectPallet();
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private void CreatePallet()
    {
        // Create the "SDK/pallets" folder if it doesn't exist
        string path = "Assets/SDK/pallets";
        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }

        // Create the pallet barcode in the format: palletname.authorsname.version
        palletBarcode = $"{palletTitle}.{palletAuthor}.{palletVersion}";

        // Create a new pallet
        Pallet newPallet = ScriptableObject.CreateInstance<Pallet>();
        newPallet.Title = palletTitle;
        newPallet.Barcode = palletBarcode;
        newPallet.Author = palletAuthor;
        newPallet.Version = palletVersion;

        // Save the pallet to disk
        AssetDatabase.CreateAsset(newPallet, path + "/" + palletTitle + ".asset");
        AssetDatabase.SaveAssets();

        // Select the newly created pallet
        selectedPallet = newPallet;
    }

    private void SelectPallet()
    {
        // Get the selected pallet from the assets
        string path = "Assets/SDK/pallets";
        string[] palletGuids = AssetDatabase.FindAssets("t:Pallet", new[] { path });

        GenericMenu menu = new GenericMenu();

        foreach (string guid in palletGuids)
        {
            string palletPath = AssetDatabase.GUIDToAssetPath(guid);
            Pallet pallet = AssetDatabase.LoadAssetAtPath<Pallet>(palletPath);
            menu.AddItem(new GUIContent(pallet.Title), false, () => SelectPalletFromMenu(pallet));
        }

        menu.ShowAsContext();
    }

    private void SelectPalletFromMenu(Pallet pallet)
    {
        selectedPallet = pallet;
    }

    private void CreateLevelCrateUI()
    {
        EditorGUILayout.Space();
        GUILayout.Label("Level Crate Settings", EditorStyles.boldLabel);

        levelCrateTitle = EditorGUILayout.TextField("Level Crate Title", levelCrateTitle);
        levelCrateBarcode = EditorGUILayout.TextField("Level Crate Barcode", levelCrateBarcode);
        levelCrateDescription = EditorGUILayout.TextField("Level Crate Description", levelCrateDescription);

        // Tags editor
        EditorGUILayout.LabelField("Tags");
        for (int i = 0; i < levelCrateTags.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            levelCrateTags[i] = EditorGUILayout.TextField(levelCrateTags[i]);
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                levelCrateTags.RemoveAt(i);
                return; // Exit to avoid modifying list while iterating
            }
            EditorGUILayout.EndHorizontal();
        }

        newTag = EditorGUILayout.TextField("New Tag", newTag);
        if (GUILayout.Button("Add Tag"))
        {
            if (!string.IsNullOrEmpty(newTag))
            {
                levelCrateTags.Add(newTag);
                newTag = ""; // Reset input field after adding
            }
        }

        // Scene selection
        levelSceneName = EditorGUILayout.TextField("Level Scene Name", levelSceneName);

        if (GUILayout.Button("Save Level Crate"))
        {
            SaveLevelCrate();
        }

        if (GUILayout.Button("Cancel"))
        {
            isCreatingLevelCrate = false; // Close the level crate creation UI
        }
    }

    private void SaveLevelCrate()
    {
        if (selectedPallet == null)
        {
            EditorUtility.DisplayDialog("Error", "Please select or create a pallet first.", "OK");
            return;
        }

        // Create the level crate barcode in the format: levelcratename.authorsname.version
        levelCrateBarcode = $"{levelCrateTitle}.{palletAuthor}.{selectedPallet.Version}"; // Using pallet's author and version

        // Create and save the new level crate
        LevelCrate newLevelCrate = ScriptableObject.CreateInstance<LevelCrate>();
        newLevelCrate.Title = levelCrateTitle;
        newLevelCrate.Barcode = levelCrateBarcode;
        newLevelCrate.Description = levelCrateDescription;
        newLevelCrate.Tags = levelCrateTags.ToArray();
        newLevelCrate.LevelSceneName = levelSceneName;

        // Save the new level crate asset
        string levelCratePath = "Assets/SDK/pallets/" + selectedPallet.Title + "_LevelCrates";
        if (!System.IO.Directory.Exists(levelCratePath))
        {
            System.IO.Directory.CreateDirectory(levelCratePath);
        }

        AssetDatabase.CreateAsset(newLevelCrate, levelCratePath + "/" + newLevelCrate.Title + ".asset");
        AssetDatabase.SaveAssets();

        // Add level crate to the selected pallet's LevelCrates array
        if (selectedPallet.LevelCrates == null)
        {
            selectedPallet.LevelCrates = new LevelCrate[] { newLevelCrate };
        }
        else
        {
            List<LevelCrate> levelCrateList = new List<LevelCrate>(selectedPallet.LevelCrates) { newLevelCrate };
            selectedPallet.LevelCrates = levelCrateList.ToArray();
        }

        // Save the updated pallet
        EditorUtility.SetDirty(selectedPallet);
        AssetDatabase.SaveAssets();

        // Reset level crate creation UI and inform the user
        isCreatingLevelCrate = false;
        EditorUtility.DisplayDialog("Level Crate Created", $"Level Crate {newLevelCrate.Title} has been created for pallet {selectedPallet.Title}.", "OK");
    }
}
