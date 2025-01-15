using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSignal : MonoBehaviour
{
    [SerializeField] float spawnTimer = 3;
    public float spawnerAmount = 0;

    public GameObject spawnPrefab;

    private float spawnTimeLeft;

    public bool spawnSignalGiven = false;
    public float spawnSignalNumber = 0;

    void Start()
    {
        spawnTimeLeft = spawnTimer;
    }

    void FixedUpdate()
    {
        if (spawnTimeLeft > 0)
        {
            spawnTimeLeft -= Time.deltaTime;
        }
        else
        {
            spawnSignalNumber = (int)Random.Range(1, spawnerAmount + 1);
            spawnSignalGiven = true;
            spawnTimeLeft = spawnTimer;
        }
    }
}
