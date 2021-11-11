using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public ParticleSystem chargingEffect;
    public ParticleSystem chargedEffect;
    public float speed;

    bool isCharged;
    bool isCharging;
    float currentChargeTime;
    float fullChargeTime = 1f;
    Camera cameraMain;
    Rigidbody2D m_rigidbody2D;
    SpriteRenderer spriteRenderer;
    DrawLine drawLine;

    Vector3 startPosition;
    Vector3 movePosition;
    Vector3 currentPosition;

    Vector3 direction;
    Vector3 startPoint;
    Vector3 currentPoint;
    Vector3 endPoint;

    void Start()
    {
        cameraMain = Camera.main;
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        drawLine = GetComponentInChildren<DrawLine>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isCharging = true;
            isCharged = false;

            startPosition = transform.position;

            startPoint = transform.position;
            startPoint.z = -10f;

            chargingEffect.Play();
        }

        if (Input.GetMouseButton(0))
        {
            currentPoint = cameraMain.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = -10f;

            #region 드로우 라인 on
            Vector3 linePoint = (startPoint - currentPoint);
            drawLine.RenderLine(linePoint, linePoint * -1);

            //Vector3 dirNor = (this.transform.position - currentPoint);
            //drawLine.RenderLine(dirNor, dirNor * -1);
            #endregion

            #region 이펙트 및 이미지 로드
            currentChargeTime += Time.deltaTime;
            if (fullChargeTime - 0.15f <= currentChargeTime)
            {
                chargingEffect.Stop();
            }

            if (fullChargeTime <= currentChargeTime)
            {
                // 한 번만 실행
                if (isCharged)
                {
                    goto charged;
                }

                chargedEffect.Play();
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player_Charged");
            charged: 
                isCharged = true;
            }
            #endregion
        }

        if (Input.GetMouseButtonUp(0))
        {
            isCharging = false;
            isCharged = false;

            endPoint = currentPoint;

            #region 드로우 라인 off
            drawLine.EndLine();
            #endregion

            // 방향 설정 및 이동 거리 설정
            direction = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, -10f, 10f),
                                    Mathf.Clamp(startPoint.y - endPoint.y, -10f, 10f));

            movePosition = direction;           // 이동 해야 할 거리
            direction = direction.normalized;   // 방향 벡터로 설정

            if (fullChargeTime <= currentChargeTime)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player");
                m_rigidbody2D.velocity = direction * speed;
                currentChargeTime = 0f;
            }
            else
            {
                chargingEffect.Stop();
                currentChargeTime = 0f;
            }
        }

        currentPosition = transform.position;
        // 이동해야 할 거리와 실재 이동 거리 비교 연산
        if (movePosition.magnitude <= (currentPosition - startPosition).magnitude)
        {
            m_rigidbody2D.velocity = new Vector2(0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            CameraManager.Instance.isZoom = true;
            BattleManager.Instance.EnterBattleMode();
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            CameraManager.Instance.isZoom = false;
        }
    }
}