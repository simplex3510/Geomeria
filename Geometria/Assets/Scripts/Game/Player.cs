using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EPlayerState
{
    Idle = 0,
    Charging,
    Charged,
    Moving,
    Dash,
    Battle,
}

public class Player : MonoBehaviour
{
    public ParticleSystem chargingEffect;
    public ParticleSystem chargedEffect;
    public DrawLine drawLine;
    public Transform targetTransform;
    public List<Sprite> playerSprite;
    public EPlayerState currentState;
    public LayerMask whatIsLayer;
    public int dashCount = 3;

    readonly float SPEED = 100f;
    readonly float FULL_CHARGE_TIME = 1f;
    float currentChargeTime;
    float angle;
    bool isColide = false;

    Camera cameraMain;
    Rigidbody2D m_rigidbody2D;
    SpriteRenderer spriteRenderer;
    Collider2D dashTarget;

    Vector3 startPosition;
    Vector3 movePosition;
    Vector3 currentPosition;

    Vector3 direction;
    Vector3 startPoint;
    Vector3 currentPoint;
    Vector3 endPoint;

    #region Singleton
    private static Player _instance;
    public static Player Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Player>();
                if (_instance == null)
                {
                    Debug.Log("No Player Singleton Object");
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }

        currentState = EPlayerState.Idle;
    }
    #endregion

    void Start()
    {
        cameraMain = Camera.main;
        currentState = EPlayerState.Idle;
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentState != EPlayerState.Battle)
        {
            if(currentState != EPlayerState.Battle)
            {
                currentState = EPlayerState.Charging;
            }
            
            startPosition = transform.position;

            startPoint = transform.position;
            startPoint.z = -10f;

            chargingEffect.Play();
        }

        if (Input.GetMouseButton(0) && currentState != EPlayerState.Battle)
        {
            currentPoint = cameraMain.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = -10f;

            #region 방향(회전) 조정
            angle = Mathf.Atan2(transform.position.y - currentPoint.y,
                                transform.position.x - currentPoint.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            #endregion

            #region 드로우 라인 on
            Vector3 linePoint = (startPoint - currentPoint);
            drawLine.RenderLine(linePoint, linePoint * -1);
            #endregion

            #region 파티클, 카메라 이펙트 및 이미지 로드
            currentChargeTime += Time.deltaTime;
            if (FULL_CHARGE_TIME - 0.15f <= currentChargeTime)
            {
                chargingEffect.Stop();
            }

            if (FULL_CHARGE_TIME <= currentChargeTime)
            {
                // 한 번만 실행
                if (currentState == EPlayerState.Charged)
                {
                    goto Charged;
                }

                chargedEffect.Play();
                Camera.main.orthographicSize -= 1f;
                spriteRenderer.sprite = playerSprite[1];
            Charged: 
                currentState = EPlayerState.Charged;
            }
            #endregion
        }

        if (Input.GetMouseButtonUp(0) && currentState != EPlayerState.Battle)
        {
            currentState = EPlayerState.Moving;
            
            endPoint = currentPoint;

            // 방향 설정 및 이동 거리 설정
            direction = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, -10f, 10f),
                                    Mathf.Clamp(startPoint.y - endPoint.y, -10f, 10f));

            movePosition = direction;           // 이동 해야 할 거리
            direction = direction.normalized;   // 방향 벡터로 설정

            if (FULL_CHARGE_TIME <= currentChargeTime)
            {
                spriteRenderer.sprite = playerSprite[0];
                m_rigidbody2D.velocity = direction * SPEED;
                currentChargeTime = 0f;
            }
            // Charged 전에 Charging을 그만두었을 때
            else
            {
                if (currentState != EPlayerState.Battle)
                {
                    currentState = EPlayerState.Idle;
                }
                chargingEffect.Stop();
                currentChargeTime = 0f;
            }

            #region 드로우 라인 off
            drawLine.EndLine();
            #endregion
        }

        currentPosition = transform.position;
        // 이동해야 할 거리와 실재 이동 거리 비교 연산
        if (currentState == EPlayerState.Moving && movePosition.magnitude <= (currentPosition - startPosition).magnitude)
        {
            movePosition = Vector3.zero;
            startPosition = Vector3.zero;
            m_rigidbody2D.velocity = new Vector2(0, 0);

            currentState = EPlayerState.Idle;
        }

        // 대쉬
        if (currentState == EPlayerState.Dash)
        {
            if (0 < dashCount)
            {
                dashTarget = Physics2D.OverlapCircle(transform.position, 5f, whatIsLayer);
                if(isColide == true)
                {
                    m_rigidbody2D.velocity = Vector2.zero;
                }
                else if (dashTarget != null)
                {
                    #region 방향 전환
                    angle = Mathf.Atan2(dashTarget.transform.position.y - transform.position.y,
                                        dashTarget.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
                    #endregion

                    #region 방향 이동
                    direction = new Vector2(dashTarget.transform.position.x - transform.position.x,
                                            dashTarget.transform.position.y - transform.position.y).normalized;
                    m_rigidbody2D.velocity = direction * SPEED;
                    #endregion

                    // 적과 충돌 시 카운트 다운
                }
                // 주변에 적이 없을 경우
                else
                {
                    dashCount = 3;
                    currentState = EPlayerState.Idle;
                }
            }
            // 대쉬 카운트가 0일 경우
            else
            {
                dashCount = 3;
                currentState = EPlayerState.Idle;
            }
        }
    
    }

    public IEnumerator PlayerKnockback()
    {
        float backSpeed = 100f;
        float duration = 0f;
        direction = new Vector2(transform.position.x - targetTransform.position.x,
                                transform.position.y - targetTransform.position.y).normalized;

        m_rigidbody2D.velocity = direction * backSpeed;
        while (true)
        {
            duration += Time.deltaTime;
            if (0.12f <= duration)
            {
                m_rigidbody2D.velocity = Vector2.zero;
                yield break;
            }
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        isColide = true;
        chargingEffect.Stop();
        drawLine.EndLine();

        if (other.gameObject.CompareTag("Enemy"))
        {
            CameraManager.Instance.isZoom = true;
            m_rigidbody2D.velocity = new Vector2(0, 0);

            switch (currentState)
            {
                case EPlayerState.Idle:
                    // Game Over
                    GameManager.Instance.currentGameState = EGameState.Defeat;
                    break;
                default:
                    currentState = EPlayerState.Battle;
                    BattleManager.Instance.enemy = other.gameObject;
                    BattleManager.Instance.EnterBattleMode(1, 3);
                    dashCount--;
                    break;
            }
        }
        else if (other.gameObject.CompareTag("Boss"))
        {
            CameraManager.Instance.isZoom = true;
            m_rigidbody2D.velocity = new Vector2(0, 0);

            switch (currentState)
            {
                case EPlayerState.Idle:
                    // Game Over
                    GameManager.Instance.currentGameState = EGameState.Defeat;
                    break;
                default:
                    currentState = EPlayerState.Battle;
                    BattleManager.Instance.enemy = other.gameObject;
                    BattleManager.Instance.EnterBattleMode(4, 6);
                    dashCount--;
                    break;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isColide = false;
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Boss"))
        {
            CameraManager.Instance.isZoom = false;
        }
    }
}