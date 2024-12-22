using UnityEngine;

public class CrateSpawner : MonoBehaviour
{
    public Crate selectedCrate; // Reference to the selected crate
    public string barcodeInput; // Barcode input in the Inspector
    private GameObject spawnedCrate; // Track the spawned crate instance
    public bool autoSpawn = true; // Checkbox for automatic crate spawning

    // Method to spawn the selected crate
    public void SpawnCrate()
    {
        // Ensure the crate is selected and its prefab is assigned
        if (selectedCrate != null && selectedCrate.CrateSpawnable != null)
        {
            // Only spawn a crate if one hasn't already been spawned
            if (spawnedCrate == null)
            {
                spawnedCrate = Instantiate(selectedCrate.CrateSpawnable, transform.position, transform.rotation);
                Debug.Log($"Spawned crate: {selectedCrate.CrateSpawnable.name}.");
            }
            else
            {
                Debug.LogWarning("A crate has already been spawned. Only one crate can be spawned.");
            }
        }
        else
        {
            Debug.LogWarning("No crate selected or crate prefab is missing.");
        }
    }

    // Method to select a crate by barcode (called at runtime)
    public void SearchCrateByBarcode()
    {
        // If no crate is selected and barcode input is not empty
        if (selectedCrate == null && !string.IsNullOrEmpty(barcodeInput))
        {
            // Attempt to find the crate based on the barcode input
            if (selectedCrate != null && selectedCrate.Barcode == barcodeInput)
            {
                Debug.Log($"Selected crate with barcode: {selectedCrate.Barcode}");
            }
            else
            {
                // If no crate matches the barcode
                Debug.LogWarning("Crate with this barcode not found.");
            }
        }
        else if (selectedCrate != null)
        {
            // If a crate is already selected, display its barcode in the input field
            barcodeInput = selectedCrate.Barcode;
        }
    }

    void Update()
    {
        SearchCrateByBarcode();

        // If autoSpawn is checked, spawn the crate automatically
        if (autoSpawn && selectedCrate != null && spawnedCrate == null)
        {
            SpawnCrate();
        }
    }

    private void OnValidate()
    {
        // Automatically search for the crate when the barcodeInput is updated (only in editor)
        if (!Application.isPlaying)
        {
            SearchCrateByBarcode();
        }
    }

    // Draw a simple Gizmo at the center of the CrateSpawner (a red sphere)
    private void OnDrawGizmos()
    {
        // Gizmo to show the center of the CrateSpawner (where the crate will spawn)
        Gizmos.color = Color.red; // Color of the Gizmo (red for the sphere)
        Gizmos.DrawSphere(transform.position, 0.1f); // Draw a red sphere at the spawner's position
        Debug.Log("Gizmo: Showing center of CrateSpawner.");
    }
}
