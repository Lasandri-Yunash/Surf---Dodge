using UnityEngine;

public class OceanManager : MonoBehaviour
{
    [Header("Wave Setup")]
    public Transform[] initialWaveSets;  // Pre-placed waves in the scene
    public GameObject[] wavePrefabs;     // Prefabs used to spawn new waves
    public int totalWaves = 30;          // Total number of waves in play
    public float waveLength = 20f;       // Distance between consecutive waves

    [Header("Movement")]
    public float moveSpeed = 5f;         // Speed waves move toward the camera
    [HideInInspector] public bool startMoving = false;

    // Internal
    private Transform[] allWaves;

    void Start()
    {
        allWaves = new Transform[totalWaves];

        // Keep the manually placed initial waves
        for (int i = 0; i < initialWaveSets.Length; i++)
        {
            allWaves[i] = initialWaveSets[i];
        }

        // Fill the rest with instantiated prefabs
        for (int i = initialWaveSets.Length; i < totalWaves; i++)
        {
            int prefabIndex = i % wavePrefabs.Length;
            Vector3 spawnPos = Vector3.forward * (i * waveLength - 10f);

            GameObject newWave = Instantiate(wavePrefabs[prefabIndex], spawnPos, Quaternion.identity);
            allWaves[i] = newWave.transform;
        }
    }

    void Update()
    {
        if (!startMoving) return;

        for (int i = 0; i < allWaves.Length; i++)
        {
            Transform wave = allWaves[i];

            // Move wave backwards
            wave.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);

            // Recycle wave if it passes behind
            if (wave.position.z < -waveLength)
            {
                float furthestZ = float.MinValue;

                // Find the last wave in front
                foreach (Transform w in allWaves)
                {
                    if (w.position.z > furthestZ)
                        furthestZ = w.position.z;
                }

                // Move this wave to the end of the chain
                wave.position = new Vector3(0, 0, furthestZ + waveLength - 10f);
            }
        }
    }
}
