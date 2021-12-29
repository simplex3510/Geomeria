using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform playerTransform;
    public readonly float ZOOM_IN = 10f;
    public readonly float ZOOM_OUT = 13f;
    public readonly float ZOOM_POWER = 0.1f;

    public bool isZoom
    {
        get;
        set;
    }

    Camera cameraMain;
    float currentChargeTime;
    bool hasCameraShake;

    #region CameraManager Singleton
    private static CameraManager _instance;
    public  static CameraManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CameraManager>();
                if(_instance == null)
                {
                    Debug.Log("No CameraManager Singleton Object");
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    void Start()
    {
        isZoom = false;
        hasCameraShake = false;
        cameraMain = Camera.main;
        cameraMain.orthographicSize = 5f;
    }

    void Update()
    {
        if (isZoom)
        {
            CameraZoomEffect(ZOOM_IN, ZOOM_POWER);
        }
        else
        {
            CameraZoomEffect(ZOOM_OUT, ZOOM_POWER);
        }

        if(hasCameraShake == true)
        {
            return;
        }
        else
        {
            cameraMain.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -10f);
        }
    }

    public IEnumerator CameraShakeEffect(float duration, float magnitude)
    {
        hasCameraShake = true;
        Vector3 originalPosition = cameraMain.transform.position;

        float elapsed = 0.0f;

        while (elapsed <= duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cameraMain.transform.position = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        cameraMain.transform.position = originalPosition;
        hasCameraShake = false;
    }

    public void CameraZoomEffect(float _zoom, float _zoomSpeed)
    {
        cameraMain.orthographicSize = Mathf.Lerp(cameraMain.orthographicSize, _zoom, _zoomSpeed);
    }
}