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
        if (selectedCrate != null && selectedCrate.combinedMesh != null)
        {
            VisualizeCombinedMesh();
        }

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

            if (selectedCrate.analogMaterial != null)
            {
                meshRenderer.material = selectedCrate.analogMaterial;
            }
            else
            {
                Debug.LogWarning("ANALOG material not found in the crate.");
            }
        }

        meshFilter.mesh = selectedCrate.combinedMesh;

        combinedMeshObject.transform.position = transform.position;
        combinedMeshObject.transform.rotation = transform.rotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.1f);

        if (selectedCrate != null && selectedCrate.combinedMesh != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawMesh(selectedCrate.combinedMesh, transform.position, transform.rotation);

            Bounds meshBounds = selectedCrate.combinedMesh.bounds;
            Gizmos.color = Color.white;
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
