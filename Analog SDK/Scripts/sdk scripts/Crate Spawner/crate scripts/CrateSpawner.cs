using UnityEngine;

public class CrateSpawner : MonoBehaviour
{
    public Crate selectedCrate; 
    public string barcodeInput; 
    private GameObject spawnedCrate; 
    public bool autoSpawn = true;

    private bool gizmoLogged = false;

    public void SpawnCrate()
    {
        if (selectedCrate != null && selectedCrate.CrateSpawnable != null)
        {
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

    public void SearchCrateByBarcode()
    {
        if (selectedCrate == null && !string.IsNullOrEmpty(barcodeInput))
        {
            if (selectedCrate != null && selectedCrate.Barcode == barcodeInput)
            {
                Debug.Log($"Selected crate with barcode: {selectedCrate.Barcode}");
            }
            else
            {
                Debug.LogWarning("Crate with this barcode not found.");
            }
        }
        else if (selectedCrate != null)
        {
            barcodeInput = selectedCrate.Barcode;
        }
    }

    void Update()
    {
        SearchCrateByBarcode();

        if (autoSpawn && selectedCrate != null && spawnedCrate == null)
        {
            SpawnCrate();
        }
    }

    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            SearchCrateByBarcode();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.1f);

        if (!gizmoLogged)
        {
            Debug.Log("Gizmo: Showing center of CrateSpawner.");
            gizmoLogged = true; 
        }
    }
}
