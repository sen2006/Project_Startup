using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ObjectiveBox") || other.CompareTag("RightAnswer") || other.CompareTag("WrongAnswer"))
        {
            Destroy(other.gameObject);
        }
    }
}
