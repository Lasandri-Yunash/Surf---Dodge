using UnityEngine;

public class OceanManager : MonoBehaviour
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
                Vector3.forward * (i * waveLength - 10f),
                Quaternion.identity
            );
            allWaves[i] = newWave.transform;
        }
    }

    void Update()
    {
        if (!startMoving) return;

        for (int i = 0; i < allWaves.Length; i++)
        {
            Transform wave = allWaves[i];
            wave.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);

            
            if (wave.position.z < -waveLength)
            {
                
                float furthestZ = float.MinValue;
                foreach (Transform w in allWaves)
                {
                    if (w.position.z > furthestZ)
                        furthestZ = w.position.z;
                }

                
                wave.position = new Vector3(0, 0, furthestZ + waveLength -10f);
            }
        }
    }
}
