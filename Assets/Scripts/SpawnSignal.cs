using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SpawnSignal : MonoBehaviour
{
    public int spawnerAmount = 0;

    private float spawnTimer = 3;
    private float spawnTimeLeft;

    public bool spawnSignalGiven = false;
    public int spawnSignalNumber = 0;
    public bool spawnsLeft = true;

    private LevelEditorHitBoxes levelEdit;

    // setup delegation
    public delegate void SpawnSignalOut(int spawnerID);
    public event SpawnSignalOut signalOut;
    void Start()
    {
        levelEdit = GameObject.Find("Level Editor").GetComponent<LevelEditorHitBoxes>();
        if (levelEdit != null)
        {
            spawnTimer = levelEdit.boxSpawnDelay;
        }
        spawnTimeLeft = spawnTimer;
    }

    void FixedUpdate()
    {
        if (spawnsLeft)
        {
            if (spawnTimeLeft > 0)
            {
                spawnTimeLeft -= Time.deltaTime;
            }
            else
            {
                spawnSignalNumber = Random.Range(1, spawnerAmount + 1);
                if (signalOut != null)
                    signalOut(spawnSignalNumber);

                spawnTimeLeft = spawnTimer;
            }
        }
    }
}
