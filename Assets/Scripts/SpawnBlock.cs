using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;

public class SpawnBlock : MonoBehaviour
{
    [SerializeField] float spawnerNumber = 0;

    private Transform tf;
    private SpawnSignal spawnSignal;
    private LevelEditorHitBoxes levelEdit;

    void Start()
    {
        tf = transform;
        spawnSignal = GameObject.Find("Spawn Handler").GetComponent<SpawnSignal>();
        if (spawnSignal != null )
        {
            spawnerNumber = spawnSignal.spawnerAmount + 1;
            spawnSignal.spawnerAmount++;
        }
        levelEdit = GameObject.Find("Level Editor").GetComponent <LevelEditorHitBoxes>();
        
        // subscribe to the signal event
        spawnSignal.signalOut += recieveSignal;
    }

    private void OnDestroy()
    {
        // unsubscribe from signal event
        spawnSignal.signalOut -= recieveSignal;
    }

    void recieveSignal(int ID) 
    {
        // the spawner called a signal
        if (ID == spawnerNumber)
        {
            Tuple<string, bool> spawnValue = levelEdit.NewWordSpawn();
            if (spawnValue != null )
            {
                GameObject newBlock = Instantiate(levelEdit.letterBox);
                Rigidbody rb = newBlock.GetComponent<Rigidbody>();
                TextMeshPro text = newBlock.GetNamedChild("Letter").GetComponent<TextMeshPro>();

                newBlock.transform.position = tf.position;
                text.text = spawnValue.Item1;
                newBlock.tag = spawnValue.Item2 ? "RightAnswer" : "WrongAnswer";
                rb.velocity = new Vector3(0, 0, -levelEdit.boxMoveSpeed);
            }
        }
    }

    //void FixedUpdate()
    //{
    //    if (spawnSignal != null)
    //    {
    //        if (spawnSignal.spawnSignalGiven && spawnSignal.spawnSignalNumber == spawnerNumber)
    //        {
    //            int spawnIndex = levelEdit.ObjectToSpawn();
    //            if (spawnIndex == 26)
    //            {
    //                GameObject newBlock = Instantiate(levelEdit.obstacle);
    //                Rigidbody rb = newBlock.GetComponent<Rigidbody>();

    //                newBlock.transform.position = new Vector3(tf.position.x, tf.position.y, tf.position.z);
    //                rb.velocity = new Vector3(0, 0, -levelEdit.boxMoveSpeed);

    //                spawnSignal.spawnSignalGiven = false;
    //            }
    //            else
    //            {
    //                GameObject newBlock = Instantiate(levelEdit.letterBox);
    //                Rigidbody rb = newBlock.GetComponent<Rigidbody>();
    //                TextMeshPro text = newBlock.GetNamedChild("Letter").GetComponent<TextMeshPro>();

    //                newBlock.transform.position = new Vector3(tf.position.x, tf.position.y, tf.position.z);
    //                text.text = LevelEditorHitBoxes.alphabet[spawnIndex].ToString();
    //                rb.velocity = new Vector3(0, 0, -levelEdit.boxMoveSpeed);

    //                spawnSignal.spawnSignalGiven = false;
    //            }
    //            //-------------------------------------------


    //        }
    //    }
    //    else
    //    {
    //        Console.WriteLine("null");
    //    }
    //}
}
