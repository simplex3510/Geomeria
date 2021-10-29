using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    bool isZoom = false;
    bool isCharge = false;
    float fullChargeTime = 1f;
    float currentChargeTime;
    float zoomIn = 10f;
    float zoomOut = 13f;
    float zoomPower = 0.1f;
    

    Transform playerPosition;
    Camera mainCamera;

    private void Start()
    {
        isZoom = false;
        mainCamera = Camera.main;
        mainCamera.orthographicSize = 5f;
        playerPosition = GetComponent<Transform>();
    }

    private void Update()
    {
        #region camera view
        mainCamera.transform.position = new Vector3(playerPosition.position.x, playerPosition.position.y, -10f);
        #endregion

        #region charge effect
        if (Input.GetMouseButton(0))
        {
            currentChargeTime += Time.deltaTime;
            if (fullChargeTime <= currentChargeTime && isCharge == false)
            {
                mainCamera.orthographicSize = 12f;
                isCharge = true;
            }
        }

        if(isCharge)
        {
            CameraZoomEffect(zoomOut, 0.01f);
        }

        if(Input.GetMouseButtonDown(0))
        {
            currentChargeTime = 0f;
            isCharge = false;
        }
        #endregion

        if (isZoom)
        {
            CameraZoomEffect(zoomIn, zoomPower);
        }
        else
        {
            CameraZoomEffect(zoomOut, zoomPower);
        }

    }

    void CameraZoomEffect(float _zoom, float _zoomSpeed)
    {
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, _zoom, _zoomSpeed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            isZoom = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            isZoom = false;
        }
    }
}