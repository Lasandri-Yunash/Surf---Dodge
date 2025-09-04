using UnityEngine;

public class OceanManager : MonoBehaviour
{
    [Header("Wave Setup")]
    public Transform[] initialWaveSets;
    public GameObject[] wavePrefabs;
    public int totalWaves = 30;
    public float waveLength = 20f;

    [Header("Movement")]
    public float moveSpeed = 5f;
    [HideInInspector] public bool startMoving = false;

    private Transform[] allWaves;

    public float verticalAmplitude = 0.5f; // Height of vertical movement
    public float verticalFrequency = 1f;   // Speed of vertical movement


    void Start()
    {
        allWaves = new Transform[totalWaves];

        // Assign initial waves first
        int i;
        for (i = 0; i < initialWaveSets.Length && i < totalWaves; i++)
        {
            allWaves[i] = initialWaveSets[i];
        }

        // Instantiate new waves to fill remaining slots
        for (; i < totalWaves; i++)
        {
            int prefabIndex = i % wavePrefabs.Length;
            Vector3 spawnPos = Vector3.forward * (i * waveLength + 30f);
            GameObject newWave = Instantiate(wavePrefabs[prefabIndex], spawnPos, Quaternion.identity, transform);
            allWaves[i] = newWave.transform;
        }
    }

    
    void Update()
    {
        if (!startMoving) return;

        for (int i = 0; i < allWaves.Length; i++)
        {
            Transform wave = allWaves[i];

            // Move wave backward
            wave.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);

            // Calculate vertical Y offset based on sine wave for smooth up and down motion
            float newY = Mathf.Sin(Time.time * verticalFrequency + i) * verticalAmplitude;

            // Apply vertical oscillation combined with the current Y position offset
            Vector3 wavePos = wave.position;
            wave.position = new Vector3(wavePos.x, newY, wavePos.z);

            // Recycle wave position if it moves past a threshold
            if (wave.position.z < -waveLength)
            {
                float furthestZ = float.MinValue;
                for (int j = 0; j < allWaves.Length; j++)
                {
                    if (allWaves[j].position.z > furthestZ)
                        furthestZ = allWaves[j].position.z;
                }
                wave.position = new Vector3(wave.position.x, wave.position.y, furthestZ + waveLength);
            }
        }
    }


}
