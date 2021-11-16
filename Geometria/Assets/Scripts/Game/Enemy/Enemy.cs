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
            BattleManager.Instance.enemies.Enqueue(this);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (BattleManager.Instance.battleResult == EState.win)
            {
                gameObject.SetActive(false);
                BattleManager.Instance.enemies.Dequeue();
            }
        }
    }
}
