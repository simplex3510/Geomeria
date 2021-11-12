using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform playerTransform;
    public float speed = 1.5f;

    Transform enemyTransform;

    void Start()
    {
        enemyTransform = GetComponent<Transform>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(enemyTransform.position, playerTransform.position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.SetParent(BattleManager.Instance.GetComponent<Transform>());
        }
    }
}
