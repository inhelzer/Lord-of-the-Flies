using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Flyes_spowner : MonoBehaviour
{
    [Header("Spawn")]
    [FormerlySerializedAs("foodPrefab")]
    [SerializeField] private GameObject flyesPrefab;
    [SerializeField] private Spider_YH spider;

    [Header("Area")]
    [SerializeField] private float minSpawnX = -7.5f;
    [SerializeField] private float maxSpawnX = 7.5f;
    [SerializeField] private float spawnY = 16f;
    [SerializeField] private float randomYRange = 1.5f;
    [SerializeField] private float minDistanceBetweenFlyes = 0.8f;

    [Header("Wave")]
    [FormerlySerializedAs("maxFoodOnScene")]
    [SerializeField] private int maxFlyesOnScene = 150;
    [SerializeField] private int flyesPerWave = 24;
    [SerializeField] private float waveInterval = 0.8f;
    [SerializeField] private float waveSpawnDuration = 0.45f;
    [SerializeField] private bool spawnImmediatelyWhenAngry = true;

    private float nextWaveTime;
    private bool wasAngry;
    private Coroutine waveRoutine;

    void Start()
    {
        if (spider == null)
        {
            spider = FindFirstObjectByType<Spider_YH>();
        }

        nextWaveTime = Time.timeSinceLevelLoad + waveInterval;
    }

    void Update()
    {
        if (flyesPrefab == null || spider == null)
        {
            return;
        }

        if (!spider.IsAngry)
        {
            wasAngry = false;

            if (waveRoutine != null)
            {
                StopCoroutine(waveRoutine);
                waveRoutine = null;
            }

            return;
        }

        if (!wasAngry)
        {
            wasAngry = true;
            nextWaveTime = spawnImmediatelyWhenAngry ? Time.timeSinceLevelLoad : Time.timeSinceLevelLoad + waveInterval;
        }

        if (waveRoutine != null || Time.timeSinceLevelLoad < nextWaveTime)
        {
            return;
        }

        waveRoutine = StartCoroutine(SpawnWave());
        nextWaveTime = Time.timeSinceLevelLoad + waveInterval;
    }

    IEnumerator SpawnWave()
    {
        int freeSlots = maxFlyesOnScene - CountExistingFlyes();
        int amountToSpawn = Mathf.Min(flyesPerWave, freeSlots);

        if (amountToSpawn <= 0)
        {
            waveRoutine = null;
            yield break;
        }

        List<float> spawnXs = BuildSpreadXPositions(amountToSpawn);
        float delay = amountToSpawn > 1 ? waveSpawnDuration / (amountToSpawn - 1) : 0f;

        for (int i = 0; i < spawnXs.Count; i++)
        {
            SpawnFlyes(spawnXs[i]);

            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }
        }

        waveRoutine = null;
    }

    List<float> BuildSpreadXPositions(int amount)
    {
        List<float> positions = new List<float>();
        float width = Mathf.Max(0f, maxSpawnX - minSpawnX);
        float safeMinDistance = Mathf.Max(0.1f, minDistanceBetweenFlyes);
        int laneCount = Mathf.Max(amount, Mathf.FloorToInt(width / safeMinDistance));

        for (int i = 0; i < laneCount; i++)
        {
            float t = laneCount <= 1 ? 0.5f : (float)i / (laneCount - 1);
            positions.Add(Mathf.Lerp(minSpawnX, maxSpawnX, t));
        }

        Shuffle(positions);

        if (positions.Count > amount)
        {
            positions.RemoveRange(amount, positions.Count - amount);
        }

        return positions;
    }

    void Shuffle(List<float> positions)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            int randomIndex = Random.Range(i, positions.Count);
            float current = positions[i];
            positions[i] = positions[randomIndex];
            positions[randomIndex] = current;
        }
    }

    void SpawnFlyes(float spawnX)
    {
        float randomY = spawnY + Random.Range(-randomYRange, randomYRange);
        Vector3 spawnPosition = new Vector3(spawnX, randomY, 0f);
        Instantiate(flyesPrefab, spawnPosition, Quaternion.identity);
    }

    int CountExistingFlyes()
    {
        GameObject[] allFlyes = GameObject.FindGameObjectsWithTag("flyes");
        return allFlyes.Length;
    }
}
