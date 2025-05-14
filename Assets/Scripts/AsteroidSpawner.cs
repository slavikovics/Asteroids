using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class AsteroidSpawner : MonoBehaviour
{
    [FormerlySerializedAs("AsteroidPrefab")] [SerializeField]
    private GameObject asteroidPrefab;

    [FormerlySerializedAs("UFOPrefab")] [SerializeField]
    private GameObject ufoPrefab;

    [FormerlySerializedAs("UFOStartCount")] [SerializeField]
    private int ufoStartCount;

    [FormerlySerializedAs("UFOWaveIncrement")] [SerializeField]
    private int ufoWaveIncrement;

    [FormerlySerializedAs("UFOWaveDelay")] [SerializeField]
    private float ufoWaveDelay;

    [FormerlySerializedAs("UFOFirstWaveDelay")] [SerializeField]
    private float ufoFirstWaveDelay;

    [FormerlySerializedAs("AsteroidStartCount")] [SerializeField]
    private int asteroidStartCount;

    [FormerlySerializedAs("AsteroidIncrementDelay")] [SerializeField]
    private float asteroidIncrementDelay;

    [FormerlySerializedAs("AsteroidIncrementCount")] [SerializeField]
    private int asteroidIncrementCount;

    private int _ufoOnSceneCount;

    private void Start()
    {
        SpawnAsteroids();
        StartCoroutine(SetUpUFOsInitialDelay());
        StartCoroutine(IncrementAsteroidCount());
    }

    private IEnumerator SetUpUFOsInitialDelay()
    {
        yield return new WaitForSeconds(ufoFirstWaveDelay);
        SpawnUFOs();
    }

    private IEnumerator IncrementAsteroidCount()
    {
        yield return new WaitForSeconds(asteroidIncrementDelay);

        for (int i = 0; i < asteroidIncrementCount; i++) SpawnAsteroid();
        StartCoroutine(IncrementAsteroidCount());
    }   

    private void SpawnUFOs()
    {
        for (int i = 0; i < ufoStartCount; i++)
        {
            SpawnUFO();
        }

        _ufoOnSceneCount = ufoStartCount;
    }

    private void SpawnAsteroids()
    {
        for (int i = 0; i < asteroidStartCount; i++)
        {
            SpawnAsteroid();
        }
    }

    private void SpawnUFO()
    {
        GameObject ufo = Instantiate(ufoPrefab);
        var ufoScript = ufo.GetComponent<UFO>();
        ufoScript.UfoDestroyed += UFODestructionHandlier;
    }

    private void UFODestructionHandlier()
    {
        _ufoOnSceneCount--;

        if (_ufoOnSceneCount == 0)
        {
            ufoStartCount += ufoWaveIncrement;
            StartCoroutine(PlanNewUFOWave());
        }
    }

    private IEnumerator PlanNewUFOWave()
    {
        yield return new WaitForSeconds(ufoWaveDelay);
        SpawnUFOs();
    }

    private void SpawnAsteroid()
    {
        GameObject asteroid = Instantiate(asteroidPrefab);
        var asteroidScript = asteroid.GetComponent<Asteroid>();
        asteroidScript.AsteroidDestroyed += SpawnAsteroid;
    }
}
