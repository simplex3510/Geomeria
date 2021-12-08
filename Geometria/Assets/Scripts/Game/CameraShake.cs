using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        Camera cameraMain = Camera.main;
        Vector3 originalPosition = cameraMain.transform.position;

        float elapsed = 0.0f;

        while(elapsed <= duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cameraMain.transform.position = originalPosition + new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        cameraMain.transform.position = originalPosition;
    }
}
