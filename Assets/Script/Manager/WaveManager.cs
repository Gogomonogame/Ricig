using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaveType
{
    OnlyMeteors,
    OnlyScouts,
    OnlySnipers,
    OnlyKamikaze,
    MixedChaos  
}

[System.Serializable]
public class WaveConfig
{
    public string waveName;          
    public WaveType waveType;        
    public int baseObjectCount = 5;  
    public float delayBetweenSpawns = 0.8f;
}

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [Header("Налаштування хвиль")]
    [SerializeField] List<WaveConfig> wavesOrder; 
    [SerializeField] float timeBetweenWaves = 5f; 
    [SerializeField] float globalAmountScaler = 1.3f; 

    [Header("Префаби об'єктів")]
    [SerializeField] GameObject[] meteorPrefabs;
    [SerializeField] GameObject scoutPrefab;
    [SerializeField] GameObject sniperPrefab;
    [SerializeField] GameObject kamikazePrefab;

    [Header("Позиції спавну")]
    [SerializeField] float[] spawnXLimits = { -1.8f, 1.8f };
    [SerializeField] float spawnYPosition = 6f;

    int currentWaveIndex = 0;
    int loopCount = 1;
    bool isWaveActive = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (wavesOrder != null && wavesOrder.Count > 0)
        {
            StartCoroutine(GameplayLoopCO());
        }
    }

    private IEnumerator GameplayLoopCO()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves);

            WaveConfig currentWave = wavesOrder[currentWaveIndex];

            isWaveActive = true;
            StartCoroutine(SpawnWaveCO(currentWave));

            while (isWaveActive)
            {
                yield return null;
            }

            currentWaveIndex++;

            if (currentWaveIndex >= wavesOrder.Count)
            {
                currentWaveIndex = 0;
                loopCount++;
            }
        }
    }

    private IEnumerator SpawnWaveCO(WaveConfig config)
    {
        int objectsToSpawn = Mathf.RoundToInt(config.baseObjectCount * Mathf.Pow(globalAmountScaler, loopCount - 1));

        Debug.Log($"--- Запуск хвилі: {config.waveName} (Кількість: {objectsToSpawn}) ---");

        for (int i = 0; i < objectsToSpawn; i++)
        {
            SpawnObjectBasedOnType(config.waveType);
            yield return new WaitForSeconds(config.delayBetweenSpawns);
        }

        isWaveActive = false;
    }

    private void SpawnObjectBasedOnType(WaveType type)
    {
        GameObject prefabToSpawn = null;

        switch (type)
        {
            case WaveType.OnlyMeteors:
                prefabToSpawn = GetRandomMeteor();
                break;
            case WaveType.OnlyScouts:
                prefabToSpawn = scoutPrefab;
                break;
            case WaveType.OnlySnipers:
                prefabToSpawn = sniperPrefab;
                break;
            case WaveType.OnlyKamikaze:
                prefabToSpawn = kamikazePrefab;
                break;
            case WaveType.MixedChaos:
                int randomChoice = Random.Range(0, 4);
                if (randomChoice == 0) prefabToSpawn = GetRandomMeteor();
                else if (randomChoice == 1) prefabToSpawn = scoutPrefab;
                else if (randomChoice == 2) prefabToSpawn = sniperPrefab;
                else prefabToSpawn = kamikazePrefab;
                break;
        }

        if (prefabToSpawn != null)
        {
            float randomX = Random.Range(spawnXLimits[0], spawnXLimits[1]);
            Vector3 spawnPos = new Vector3(randomX, spawnYPosition, 0f);

            GameObject spawned = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

            Meteor meteorScript = spawned.GetComponent<Meteor>();
            if (meteorScript != null)
            {
                meteorScript.InitializeRandomProperties();
            }
        }
    }

    private GameObject GetRandomMeteor()
    {
        if (meteorPrefabs == null || meteorPrefabs.Length == 0) return null;
        return meteorPrefabs[Random.Range(0, meteorPrefabs.Length)];
    }
}