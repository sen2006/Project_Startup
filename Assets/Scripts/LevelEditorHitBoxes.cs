using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorHitBoxes : MonoBehaviour
{
    [SerializeField] private int randomBoxAmount;
    [SerializeField] private int obstacleAmount;
    [SerializeField] private string targetWord = "test";

    public float boxMoveSpeed;
    public float boxSpawnDelay;

    public GameObject letterBox;
    public GameObject obstacle;

    public List<int> spawnQueue = new List<int>();

    public string alphabet = "abcdefghijklmnopqrstuvwxyz";

    private SpawnSignal spawnSignal;

    void Start()
    {
        spawnSignal = GameObject.Find("Spawn Handler").GetComponent<SpawnSignal>();

        for (int i = 0; i < targetWord.Length; i++)
        {
            for(int j = 0; j < alphabet.Length; j++)
            {
                if (targetWord[i] == alphabet[j])
                {
                    spawnQueue.Add(j);
                    break;
                }
            }
        }

        for (int i = 0; i < randomBoxAmount; i++)
        {
            spawnQueue.Add(UnityEngine.Random.Range(0, 27));
        }

        for (int i = 0; i < obstacleAmount; i++)
        {
            spawnQueue.Add(26);
        }
    }

    public int ObjectToSpawn()
    {
        int pickNumber = UnityEngine.Random.Range(0, spawnQueue.Count);
        int returnNumber = spawnQueue[pickNumber];
        spawnQueue.RemoveAt(pickNumber);
        if (spawnQueue.Count == 0)
        {
            spawnSignal.spawnsLeft = false;
        }
        return returnNumber;
    }
}
