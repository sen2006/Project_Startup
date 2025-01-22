using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandFireballSpawn : MonoBehaviour
{
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] Transform spawnLocation;
    [SerializeField] float spawnSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void onTrigger()
    {
        GameObject fireball = Instantiate(fireballPrefab);
        fireball.transform.position = spawnLocation.position;
        Rigidbody fireBallRB = fireball.GetComponent<Rigidbody>();
        fireBallRB.velocity = transform.up*spawnSpeed;
        FireBall fireBallScript = fireball.GetComponent<FireBall>();
        Debug.Assert( fireBallScript != null , "add fireball script to fireball prefab");
        fireBallScript.hitEvent += AnswerHit;
    }

    private void AnswerHit(bool isCorrect) { 
    
    }
}
