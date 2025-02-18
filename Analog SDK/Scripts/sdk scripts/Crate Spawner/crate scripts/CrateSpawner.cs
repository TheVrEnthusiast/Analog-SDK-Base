using UnityEngine;

public class CrateSpawner : MonoBehaviour
{
    public Crate selectedCrate;
    public string barcodeInput;
    private GameObject spawnedCrate;
    public bool autoSpawn = true;

    private bool gizmoLogged = false;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private GameObject combinedMeshObject;

    void Update()
    {
        SearchCrateByBarcode();

        if (autoSpawn && selectedCrate != null && spawnedCrate == null)
        {
            SpawnCrate();
        }
    }

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

        Destroy(gameObject);
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

    private void VisualizeCombinedMesh()
    {
        if (combinedMeshObject == null)
        {
            combinedMeshObject = new GameObject("CombinedMeshObject");
            combinedMeshObject.transform.SetParent(transform);

            meshFilter = combinedMeshObject.AddComponent<MeshFilter>();
            meshRenderer = combinedMeshObject.AddComponent<MeshRenderer>();
        }

        meshFilter.mesh = selectedCrate.combinedMesh;

        combinedMeshObject.transform.position = transform.position;
        combinedMeshObject.transform.rotation = transform.rotation;
    }

    private void OnDrawGizmos()
    {
        if (selectedCrate != null && selectedCrate.combinedMesh != null)
        {
            Gizmos.color = selectedCrate.gizmoColor;

            Gizmos.DrawMesh(selectedCrate.combinedMesh, transform.position, transform.rotation);

            Gizmos.color = Color.white;
            Bounds meshBounds = selectedCrate.combinedMesh.bounds;
            Gizmos.DrawWireCube(transform.position + meshBounds.center, meshBounds.size);
        }

        if (!gizmoLogged)
        {
            Debug.Log("Gizmo: Showing center of CrateSpawner.");
            gizmoLogged = true;
        }
    }

    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            SearchCrateByBarcode();
        }
    }
}
