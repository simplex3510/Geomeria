using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public ParticleSystem chargingEffect;
    public ParticleSystem chargedEffect;
    public float speed;

    float currentChargeTime;
    float fullChargeTime = 1f;
    Camera m_camera;
    Rigidbody2D m_rigidbody2D;
    SpriteRenderer spriteRenderer;

    Vector2 minDistance = new Vector2(-3, -3);
    Vector2 maxDistance = new Vector2(3, 3);
    Vector2 distance;
    Vector3 startPoint;
    Vector3 endPoint;

    void Start()
    {
        m_camera = Camera.main;
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = m_camera.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 1f;

            // var chargingEffectMain = chargingEffect.main;
            // chargingEffectMain.loop = true;
            chargingEffect.Play();
        }

        if (Input.GetMouseButton(0))
        {
            currentChargeTime += Time.deltaTime;

            if (fullChargeTime - 0.15f <= currentChargeTime)
            {
                // spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player_Charged");
                // var chargingEffectMain = chargingEffect.main;
                // chargingEffectMain.loop = false;
                chargingEffect.Stop();
            }
            
            if (fullChargeTime <= currentChargeTime)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player_Charged");
                // chargedEffect.Play();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            endPoint = m_camera.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 1f;

            distance = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minDistance.x, maxDistance.x),
                                   Mathf.Clamp(startPoint.y - endPoint.y, minDistance.y, maxDistance.y));

            if (fullChargeTime <= currentChargeTime)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player");
                m_rigidbody2D.velocity = distance * speed;
                currentChargeTime = 0f;
            }
            else
            {
                chargingEffect.Stop();
                currentChargeTime = 0f;
            }
        }
    }
}