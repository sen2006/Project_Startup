using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public delegate void HitEvent(bool correctAnswer);
    public event HitEvent hitEvent;
    public bool despawndOtherOnHit = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RightAnswer"))
        {
            hitEvent(true);
            DestroyObjects(other.gameObject);
        }
        else if (other.CompareTag("WrongAnswer"))
        {
            hitEvent(false);
            DestroyObjects(other.gameObject);
        }
    }

    // other option
    // if (other.CompareTag("RightAnswer") || other.CompareTag("WrongAnswer"))
    // {
    //  hitEvent(other.CompareTag("RightAnswer"));
    //  DestroyObjects(other.gameObject);
    // }



    private void DestroyObjects(GameObject other)
    {
        Destroy(gameObject);
        if (despawndOtherOnHit)
        {
            Destroy(other);
        }
    }

}

