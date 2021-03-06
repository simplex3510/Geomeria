using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public DestroyEffect destroyEffect;
    public Transform playerTransform;
    public Rigidbody2D m_rigidbody2D;

    float speed;
    float backSpeed;
    float angle;
    Vector2 direction;
    Vector2 currentVelocity;

    void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        angle = Mathf.Atan2(playerTransform.position.y - transform.position.y,
                            playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    void OnDisable()
    {
        destroyEffect.isEffect = false;
    }

    void Update()
    {
        if (Player.Instance.currentState == EPlayerState.Battle)
        {
            Pause();
        }
        else if (BattleManager.Instance.currentBattleState == EBattleState.Success)
        {
            Resume();
        }
        else if (BattleManager.Instance.currentBattleState == EBattleState.Normal)
        {
            Move();
        }
        else if (BattleManager.Instance.currentBattleState == EBattleState.Miss)
        {
            EnemyKnockBack();
        }
        else if (Player.Instance.dashCount == 0)
        {
            EnemyKnockBack();
        }
        
    }

    void Pause()
    {
        currentVelocity = m_rigidbody2D.velocity;
        m_rigidbody2D.velocity = Vector2.zero;
    }

    void Resume()
    {
        m_rigidbody2D.velocity = currentVelocity;
    }

    void EnemyKnockBack()
    {
        Debug.Log((transform.position - playerTransform.position).magnitude.ToString());
        if ((transform.position - playerTransform.position).magnitude <= 10)
        {
            backSpeed = 100f;
            direction = new Vector2(transform.position.x - playerTransform.position.x,
                                    transform.position.y - playerTransform.position.y).normalized;

            m_rigidbody2D.velocity = direction * backSpeed;
        }
        else
        {
            m_rigidbody2D.velocity = Vector2.zero;
        }
    }

    void Move()
    {
        if (Player.Instance.currentState != EPlayerState.Battle)
        {
            if (speed <= 0.25f)
            {
                speed = 5f;
            }

            angle = Mathf.Atan2(playerTransform.position.y - transform.position.y,
                                playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

            speed = Mathf.Lerp(speed, 0, 0.05f);
            direction = new Vector2(playerTransform.position.x - transform.position.x,
                                    playerTransform.position.y - transform.position.y).normalized;

            m_rigidbody2D.velocity = direction * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_rigidbody2D.velocity = Vector2.zero;
        }
    }
}
