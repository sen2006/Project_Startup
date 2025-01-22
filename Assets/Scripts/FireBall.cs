using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RightAnswer"))
        {
            DestroyObjects(other.gameObject);
        }
        else if (other.CompareTag("WrongAnswer"))
        {
            DestroyObjects(other.gameObject);
        }
    }

    public void GetDelegate()
    {

    }

    private void DestroyObjects(GameObject other)
    {
        Destroy(other);
        Destroy(gameObject);
    }

}
