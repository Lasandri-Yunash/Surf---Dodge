using UnityEngine;
using System.Collections.Generic;

public class OceanManager : MonoBehaviour
{
    [Header("References")]
    public Transform plank;               // Assign your static plank (parent of the tiles)

    [Header("Spawning & Movement")]
    public GameObject[] objectPrefabs;    // Obstacles/coins prefabs
    public float spawnInterval = 1.2f;    // Seconds between spawns
    public float moveSpeed = 8f;          // Toward camera (negative Z)
    public float edgeOffset = 1.0f;       // How far beyond the plank edge to spawn (prevents pop-in)
    public float destroyMargin = 5f;      // How far past the back edge before destroying
    public float surfaceYOffset = 0.05f;  // Lift above plank surface so objects don't intersect
    [Range(0.1f, 1f)]
    public float spacingFactor = 0.3f;      // Controls how far apart the lanes are (1 = full width, <1 = closer)

    [HideInInspector] public bool startMoving = false;

    // Internal
    private Bounds plankBounds;
    private float[] laneX = new float[3];
    private float spawnZ;
    private float destroyZ;
    private float topY;
    private float timer;

    private readonly List<Transform> active = new List<Transform>();

    void Start()
    {
        if (plank == null)
        {
            Debug.LogError("OceanManager: Plank reference is missing.");
            enabled = false;
            return;
        }

        // Build combined bounds from all child renderers of the plank
        var rends = plank.GetComponentsInChildren<Renderer>();
        if (rends.Length == 0)
        {
            Debug.LogError("OceanManager: Plank has no child renderers to measure.");
            enabled = false;
            return;
        }

        plankBounds = rends[0].bounds;
        for (int i = 1; i < rends.Length; i++) plankBounds.Encapsulate(rends[i].bounds);

        // Lane centers across the plank width with spacingFactor applied
        float w = plankBounds.size.x * spacingFactor;
        float center = plankBounds.center.x;
        float halfSpan = w / 2f;

        laneX[0] = center - halfSpan;  // left
        laneX[1] = center;             // middle
        laneX[2] = center + halfSpan;  // right

        // Top surface Y for spawning
        topY = plankBounds.max.y + surfaceYOffset;

        // Spawn at the FRONT edge (max.z) and destroy past the BACK edge (min.z)
        spawnZ = plankBounds.max.z + edgeOffset;
        destroyZ = plankBounds.min.z - destroyMargin;
    }

    void Update()
    {
        if (!startMoving) return;

        // Spawn on a timer
        timer += Time.unscaledDeltaTime; // still works if you pause gameplay with timeScale
        if (timer >= spawnInterval)
        {
            SpawnOne();
            timer = 0f;
        }

        // Move and clean up
        for (int i = active.Count - 1; i >= 0; i--)
        {
            Transform t = active[i];
            if (t == null) { active.RemoveAt(i); continue; }

            t.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);

            if (t.position.z < destroyZ)
            {
                Destroy(t.gameObject);
                active.RemoveAt(i);
            }
        }
    }

    void SpawnOne()
    {
        if (objectPrefabs == null || objectPrefabs.Length == 0) return;

        int prefabIdx = Random.Range(0, objectPrefabs.Length);
        int laneIdx   = Random.Range(0, 3);

        Vector3 pos = new Vector3(laneX[laneIdx], topY, spawnZ);
        GameObject go = Instantiate(objectPrefabs[prefabIdx], pos, Quaternion.identity);
        active.Add(go.transform);
    }

    // Optional: visualize lanes & spawn/destroy lines in Scene view
    void OnDrawGizmosSelected()
    {
        if (plank == null) return;

        var rends = plank.GetComponentsInChildren<Renderer>();
        if (rends.Length == 0) return;
        Bounds b = rends[0].bounds;
        for (int i = 1; i < rends.Length; i++) b.Encapsulate(rends[i].bounds);

        float w = b.size.x * spacingFactor;
        float center = b.center.x;
        float halfSpan = w / 2f;
        float[] gx = { center - halfSpan, center, center + halfSpan };

        float top = b.max.y + 0.01f;

        // Spawn/destroy planes
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(b.min.x, top, b.max.z), new Vector3(b.max.x, top, b.max.z));
        Gizmos.DrawLine(new Vector3(b.min.x, top, b.min.z), new Vector3(b.max.x, top, b.min.z));

        // Lane markers at front edge
        Gizmos.color = Color.cyan;
        foreach (var x in gx)
            Gizmos.DrawSphere(new Vector3(x, top, b.max.z), 0.1f);
    }
}
