using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    public Transform enemyTransform;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = enemyTransform.position;
    }
}
