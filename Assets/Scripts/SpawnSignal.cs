using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSignal : MonoBehaviour
{
    public float spawnerAmount = 0;

    private float spawnTimer = 3;
    private float spawnTimeLeft;

    public bool spawnSignalGiven = false;
    public float spawnSignalNumber = 0;
    public bool spawnsLeft = true;

    private LevelEditorHitBoxes levelEdit;

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
                spawnSignalNumber = (int)Random.Range(1, spawnerAmount + 1);
                spawnSignalGiven = true;
                spawnTimeLeft = spawnTimer;
            }
        }
    }
}
