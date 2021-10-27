using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    bool isSuccess;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Enter Battle Mode
            
            if(isSuccess)
            {
                Destroy(this.gameObject);
                CameraManager.isZoom = false;
            }
        }
    }
}
