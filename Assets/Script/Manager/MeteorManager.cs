using System.Collections;
using UnityEngine;

public class MeteorManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float timeUntilNextMeteorWave = 20f;
    [SerializeField] int waveCount = 0;
    [SerializeField] float meteorAmmountScaler = 1.5f;
    [SerializeField] int meteorAmmount = 2;
    [SerializeField] GameObject[] meteorsPrefabs;
    [SerializeField] float spawnYPosition = 6f;
    [SerializeField] float delayBetweenMeteors = 0.5f;

    [SerializeField] float[] meteorSpawnPositions = { -1.8f, 1.8f };

    [SerializeField] GameObject[] bonusPrefabs;

    float timer = 0f;
    bool isSpawning = false;

    private void Start()
    {
        timer = timeUntilNextMeteorWave;
    }

    private void Update()
    {
        if (isSpawning) return;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            StartCoroutine(SpawnWave());
        }
    }

    private IEnumerator SpawnWave()
    {
        isSpawning = true;
        waveCount++;

        int currentWaveMeteors = Mathf.RoundToInt(meteorAmmount * Mathf.Pow(meteorAmmountScaler, waveCount - 1));

        for (int i = 0; i < currentWaveMeteors; i++)
        {
            SpawnSingleMeteor();

            yield return new WaitForSeconds(delayBetweenMeteors);
        }

        timer = timeUntilNextMeteorWave;
        isSpawning = false;
    }

    private void SpawnSingleMeteor()
    {
        if (meteorsPrefabs == null || meteorsPrefabs.Length == 0) return;

        int randomPrefabIndex = Random.Range(0, meteorsPrefabs.Length);
        GameObject meteorPrefab = meteorsPrefabs[randomPrefabIndex];

        float minX = meteorSpawnPositions[0];
        float maxX = meteorSpawnPositions[1];
        float randomX = Random.Range(minX, maxX);

        Vector3 spawnPosition = new Vector3(randomX, spawnYPosition, 0f);

        GameObject spawnedMeteor = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);

        Meteor meteorScript = spawnedMeteor.GetComponent<Meteor>();
        if (meteorScript != null)
        {
            meteorScript.InitializeRandomProperties();
            if(meteorScript.type == MeteorType.Big || meteorScript.type == MeteorType.Medium)
            {
                meteorScript.bonusPrefab = bonusPrefabs[Random.Range(0, bonusPrefabs.Length)];
            }
        }
    }
}