using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform playerPosition;
    public ParticleSystem chargingEffect;
    public ParticleSystem chargedEffect;
    public DrawLine drawLine;
    public Sprite[] bossSprites;
    public float battleCnt
    {
        get { return battleCount; }
        set { battleCount = value; }
    }

    readonly float FULL_CHARGE_TIME = 1.2f;

    SpriteRenderer spriteRenderer;
    Rigidbody2D m_rigidbody2D;
    Vector3 startPosition;
    Vector3 currentPosition;
    Vector3 endPosition;
    Vector3 movePosition;
    Vector3 linePoint;
    Vector3 direction;
    EState currentState;

    float currentChargeTime;
    float speed = 100f;
    float angle;
    float battleCount = 5;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();

        currentState = EState.Charging;
        StartCoroutine(Update_FSM());
    }

    // Update is called once per frame
    IEnumerator Update_FSM()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        while (true)
        {
            Debug.Log("FSM");
            switch (currentState)
            {
                case EState.Idle:
                    yield return Pause();
                    break;
                case EState.Charging:
                    yield return Charging();
                    break;
                case EState.Charged:
                    currentState = EState.Moving;
                    break;
                case EState.Moving:
                    yield return Move();
                    break;
                default:
                    yield return null;
                    break;
            }
        }
    }

    IEnumerator Pause()
    {
        while (true)
        {
            Debug.Log("Pause");
            if (currentState != EState.Idle)
            {
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator Charging()
    {
        chargingEffect.Play();
        startPosition = transform.position;

        while (true)
        {
            Debug.Log("Charging");
            if (currentState != EState.Charging)
            {
                chargingEffect.Stop();
                yield break;
            }

            #region 방향(회전) 조정
            angle = Mathf.Atan2(playerPosition.position.y - transform.position.y, playerPosition.position.x - transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            #endregion

            #region 드로우 라인 on
            linePoint = (playerPosition.position - transform.position);
            drawLine.RenderLine(linePoint, linePoint * -1);
            #endregion

            currentChargeTime += Time.deltaTime;
            if (FULL_CHARGE_TIME - 0.15f <= currentChargeTime)
            {
                chargingEffect.Stop();
            }

            if (FULL_CHARGE_TIME <= currentChargeTime)
            {
                // 한 번만 실행
                if (currentState == EState.Charged)
                {
                    yield return new WaitForSecondsRealtime(0.5f);
                    yield break;
                }

                endPosition = playerPosition.position;
                chargedEffect.Play();
                spriteRenderer.sprite = bossSprites[1];
                currentState = EState.Charged;
            }

            yield return null;
        }
    }

    IEnumerator Move()
    {
        // 방향 설정 및 이동 거리 설정
        direction = new Vector2(Mathf.Clamp(endPosition.x - transform.position.x, -10f, 10f),
                                Mathf.Clamp(endPosition.y - transform.position.y, -10f, 10f));

        movePosition = direction;           // 이동 해야 할 거리
        direction = direction.normalized;   // 방향 벡터로 설정

        if (FULL_CHARGE_TIME <= currentChargeTime)
        {
            spriteRenderer.sprite = bossSprites[0];
            m_rigidbody2D.velocity = direction * speed;
            currentChargeTime = 0f;
        }

        #region 드로우 라인 off
        drawLine.EndLine();
        #endregion

        while (true)
        {
            if (currentState != EState.Moving)
            {
                yield break;
            }

            currentPosition = transform.position;
            // 이동해야 할 거리와 실재 이동 거리 비교 연산
            if (movePosition.magnitude <= (currentPosition - startPosition).magnitude)
            {
                m_rigidbody2D.velocity = new Vector2(0, 0);
                break;
            }
            yield return null;
        }

        currentState = EState.Charging;

        yield return new WaitForSecondsRealtime(1.5f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_rigidbody2D.velocity = Vector2.zero;
            currentChargeTime = 0f;
            drawLine.EndLine();
            currentState = EState.Idle;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            currentState = EState.Charging;
        }
    }
}
