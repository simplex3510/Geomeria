using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static bool isZoom
    {
        get;
        set;
    }

    float fullChargeTime = 1f;
    float currentChargeTime;
    float zoomIn = 10f;
    float zoomOut = 13f;
    float zoomSpeed = 0.1f;
    

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
            if (fullChargeTime <= currentChargeTime)
            {
                Debug.Log("mainCamera.orthographicSize = 12f;");
                mainCamera.orthographicSize = 12f;
            }
        }

        if(Input.GetMouseButton(0))
        {
            currentChargeTime = 0f;
        }
        #endregion

        if (isZoom)
        {
            CameraZoomEffect(zoomIn, zoomSpeed);
        }
        else
        {
            CameraZoomEffect(zoomOut, zoomSpeed);
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