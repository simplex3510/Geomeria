using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public ParticleSystem chargingEffect;
    public ParticleSystem chargedEffect;
    public float speed;
    public float offsetSpeed;

    bool isCharge;
    float currentChargeTime;
    float fullChargeTime = 1f;
    Camera m_camera;
    Rigidbody2D m_rigidbody2D;
    SpriteRenderer spriteRenderer;
    DrawArrow drawArrow;

    Vector3 startPosition;
    Vector3 movePosition;
    Vector3 currentPosition;

    Vector3 direction;
    Vector3 startPoint;
    Vector3 currentPoint;
    Vector3 endPoint;

    void Start()
    {
        m_camera = Camera.main;
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        drawArrow = GetComponentInChildren<DrawArrow>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // isCharge = true;
            // startPoint = m_camera.ScreenToWorldPoint(Input.mousePosition);
            // startPoint.z = 0f;

            startPosition = transform.position;

            chargingEffect.Play();
        }

        if (Input.GetMouseButton(0))
        {
            currentPoint = m_camera.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = 0f;

            #region 드로우 라인 on
            drawArrow.RenderLine(currentPoint * -1, currentPoint);
            #endregion

            #region 이펙트 및 이미지 렌더러
            currentChargeTime += Time.deltaTime;
            if (fullChargeTime - 0.15f <= currentChargeTime)
            {
                chargingEffect.Stop();
            }
            
            if (fullChargeTime <= currentChargeTime && currentChargeTime < currentChargeTime + 1f)
            {
                // chargedEffect.Play();
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player_Charged");
            }
            #endregion
        }

        if (Input.GetMouseButtonUp(0))
        {
            isCharge = false;
            // endPoint = m_camera.ScreenToWorldPoint(Input.mousePosition);
            endPoint = currentPoint;
            startPoint = endPoint * -1;
            endPoint.z = 0f;

            #region 드로우 라인 off
            drawArrow.EndLine();
            #endregion

            direction = new Vector2(startPoint.x - endPoint.x, startPoint.y - endPoint.y);
            movePosition = direction;   // 이동 해야 할 거리
            direction = direction.normalized;

            if (fullChargeTime <= currentChargeTime)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player");
                m_rigidbody2D.velocity = direction * speed * offsetSpeed;
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
        if(movePosition.magnitude <= (currentPosition - startPosition).magnitude)
        {
            Debug.Log($"CP-SP: {(currentPosition - startPosition).magnitude}");
            Debug.Log($"MP: {movePosition.magnitude}");
            Debug.Log("Move Stop");
            m_rigidbody2D.velocity = new Vector2(0, 0);
        }
    }
}