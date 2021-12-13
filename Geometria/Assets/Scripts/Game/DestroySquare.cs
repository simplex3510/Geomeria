using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySquare : MonoBehaviour
{
    public Transform enemyTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = enemyTransform.position;

        if(transform.localScale.y <= 0)
        {
            transform.localScale = new Vector3(100, 1.6f, 1);
            this.gameObject.SetActive(false);
        }

        transform.localScale -= new Vector3(0, 2.4f * Time.deltaTime, 0);
    }
}
