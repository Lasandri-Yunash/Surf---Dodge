using UnityEngine;

public class OceanManager: MonoBehaviour
{
    public Transform[] initialWaveSets; 
    public GameObject[] wavePrefabs; 
    public int totalWaves = 30;
    public float waveLength = 20f; 
    public float moveSpeed = 5f; 

    [HideInInspector] public bool startMoving = false; 

    private Transform[] allWaves;

    void Start()
    {
        allWaves = new Transform[totalWaves];

        
        for (int i = 0; i < initialWaveSets.Length; i++)
        {
            allWaves[i] = initialWaveSets[i];
        }

        
        for (int i = initialWaveSets.Length; i < totalWaves; i++)
        {
            int prefabIndex = (i % wavePrefabs.Length);
            GameObject newWave = Instantiate(
                wavePrefabs[prefabIndex],
                Vector3.forward * (i * waveLength),
                Quaternion.identity
            );
            allWaves[i] = newWave.transform;
        }
    }

    void Update()
    {
        if (!startMoving) return; 

        foreach (Transform wave in allWaves)
        {
            wave.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
