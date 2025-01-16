using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlock : MonoBehaviour
{
    [SerializeField] float spawnerNumber = 0;

    private Transform tf;
    private SpawnSignal spawnSignal;

    void Start()
    {
        tf = transform;
        spawnSignal = GameObject.Find("Spawn Handler").GetComponent<SpawnSignal>();
        if (spawnSignal != null )
        {
            spawnerNumber = spawnSignal.spawnerAmount + 1;
            spawnSignal.spawnerAmount++;
        }
    }

    void FixedUpdate()
    {
        if (spawnSignal != null)
        {
            if (spawnSignal.spawnSignalGiven && spawnSignal.spawnSignalNumber == spawnerNumber)
            {
                GameObject newBlock = Instantiate(spawnSignal.spawnPrefab);
                newBlock.transform.position = new Vector3(tf.position.x, tf.position.y, tf.position.z);
                spawnSignal.spawnSignalGiven = false;
            }
        }
        else
        {
            Console.WriteLine("null");
        }
    }
}
