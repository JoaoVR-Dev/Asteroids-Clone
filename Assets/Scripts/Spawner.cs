using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Asteroids asteroids;
    public float spawnDistance = 12f;
    public float spawnRate = 1f;
    public int spawnAmout = 1;
    [Range(0f, 45f)]
    public float trajectoryVariance = 15f;

    void Start()
    {
        InvokeRepeating(nameof(Spawn), this.spawnRate, this.spawnRate);
    }

    private void Spawn()
    {
        for (int i = 0; i < this.spawnAmout; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized;
            Vector3 spawnPoint = transform.position + (spawnDirection * spawnDistance);

            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            Asteroids asteroid = Instantiate(asteroids, spawnPoint, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);

            asteroid.SetTrajectory(rotation * -spawnDirection);
        }
    }
}
