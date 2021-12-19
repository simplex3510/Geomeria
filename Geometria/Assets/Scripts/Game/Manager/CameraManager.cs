using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;

    public float currentZoomSize;
    public readonly float ZOOM_IN = 10f;
    public readonly float ZOOM_OUT = 13f;
    public readonly float ZOOM_POWER = 0.1f;

    public bool isZoom
    {
        get;
        set;
    }

    Camera cameraMain;
    bool isCharge = false;
    float FULL_CHARGE_TIME = 1f;
    float currentChargeTime;

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

    private void Start()
    {
        isZoom = false;
        cameraMain = Camera.main;
        cameraMain.orthographicSize = 5f;
        StartCoroutine(Update_FSM());
    }

    void Update()
    {
        if(Input.anyKeyDown &&
           Player.Instance.currentState == EState.Battle &&
           !Input.GetMouseButtonDown(0) &&
           !Input.GetMouseButtonDown(1) &&
           !Input.GetMouseButtonDown(2))
        {
            CameraZoomEffect(currentZoomSize - 1, ZOOM_POWER);
        }    
    }

    IEnumerator Update_FSM()
    {
        while (true)
        {
            switch (Player.Instance.currentState)
            {
                default:
                    yield return StartCoroutine(NormalState());
                    break;
            }

            yield return null;
        }
    }

    IEnumerator NormalState()
    //void Update()
    {
        #region Player Camera View
        cameraMain.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
        #endregion

        currentZoomSize = cameraMain.orthographicSize;

        #region Charge Camera Effect
        if (Input.GetMouseButtonUp(0))
        {
            currentChargeTime = 0f;
            isCharge = false;
        }

        if (Input.GetMouseButton(0))
        {
            currentChargeTime += Time.deltaTime;
            if (FULL_CHARGE_TIME <= currentChargeTime && isCharge == false)
            {
                cameraMain.orthographicSize = currentZoomSize - 1;
                isCharge = true;
            }
        }
        #endregion

        if (isZoom)
        {
            CameraZoomEffect(ZOOM_IN, ZOOM_POWER);
        }
        else
        {
            CameraZoomEffect(ZOOM_OUT, ZOOM_POWER);
        }

        yield return null;
    }

    public IEnumerator CameraShakeEffect(float duration, float magnitude)
    {
        Vector3 originalPosition = cameraMain.transform.position;

        float elapsed = 0.0f;

        while (elapsed <= duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cameraMain.transform.position = originalPosition + new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        cameraMain.transform.position = originalPosition;
    }

    public void CameraZoomEffect(float _zoom, float _zoomSpeed)
    {
        cameraMain.orthographicSize = Mathf.Lerp(cameraMain.orthographicSize, _zoom, _zoomSpeed);
    }
}