using UnityEditor;
using UnityEngine;

public class CustomSDKEditor : Editor
{
    // Menu item for creating the Prefab Spawner
    [MenuItem("GameObject/SDK/Spawner/PrefabSpawner")]
    public static void CreatePrefabSpawner(MenuCommand command)
    {
        // Create a new empty GameObject
        GameObject go = new GameObject("Prefab Spawner");

        go.AddComponent<PrefabSpawner>();

        // Register the object creation in the Undo system
        Undo.RegisterCreatedObjectUndo(go, "Created Prefab Spawner");

        // Print confirmation in the console
        Debug.Log("Created custom Prefab Spawner.");
    }

    // Menu item for creating a Zone
    [MenuItem("GameObject/SDK/Zones/SceneZone")]
    public static void CreateZone(MenuCommand command)
    {
        // Create a new GameObject for the Scene Zone
        GameObject sceneZone = new GameObject("Scene Zone");

        // Add the BoxCollider and set it as a trigger
        BoxCollider boxCollider = sceneZone.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;  // Make the BoxCollider a trigger

        // Add custom components
        sceneZone.AddComponent<SceneZone>();
        sceneZone.AddComponent<ZoneLinks>();

        Undo.RegisterCreatedObjectUndo(sceneZone, "Create Scene Zone");

        // Select the newly created object in the Hierarchy
        Selection.activeObject = sceneZone;
    }

    // Menu item for creating a Zone Chunk
    [MenuItem("GameObject/SDK/Zones/ZoneChunk")]
    public static void CreateZoneChunk(MenuCommand command)
    {
        // Create a new GameObject for the Scene Zone Chunk
        GameObject sceneZonechunk = new GameObject("Scene Chunk");

        // Add the BoxCollider and set it as a trigger
        BoxCollider boxCollider = sceneZonechunk.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;  // Make the BoxCollider a trigger

        // Add custom components
        sceneZonechunk.AddComponent<SceneZone>();
        sceneZonechunk.AddComponent<ZoneLinks>();
        sceneZonechunk.AddComponent<SceneChunk>();

        Undo.RegisterCreatedObjectUndo(sceneZonechunk, "Create Scene Chunk");

        // Select the newly created object in the Hierarchy
        Selection.activeObject = sceneZonechunk;
    }

    // Menu item for creating the CrateSpawner Spawner
    [MenuItem("GameObject/SDK/Crates/CrateSpawner")]
    public static void CreateCrateSpawner(MenuCommand command)
    {
        // Create a new empty GameObject
        GameObject go = new GameObject("Crate Spawner");

        go.AddComponent<CrateSpawner>();

        // Register the object creation in the Undo system
        Undo.RegisterCreatedObjectUndo(go, "Created Crate Spawner");

        // Print confirmation in the console
        Debug.Log("Created custom Prefab Spawner.");
    }

    // Menu item for creating the CrateSpawner Spawner
    [MenuItem("GameObject/SDK/Crates/LevelCrate")]
    public static void CreateLevelCrate(MenuCommand command)
    {
        // Create a new empty GameObject
        GameObject go = new GameObject("Level Crate");

        go.AddComponent<CrateSpawner>();

        // Register the object creation in the Undo system
        Undo.RegisterCreatedObjectUndo(go, "Created Level Crate");

        // Print confirmation in the console
        Debug.Log("Created custom level crate.");
    }


}
