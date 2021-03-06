using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public Transform currentPosition;

    LineRenderer lineRenderer;
    Vector3[] points;

    private void Awake()
    {
        points = new Vector3[2];
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void RenderLine(Vector3 _startPoint, Vector3 _endPoint)
    {
        transform.position = currentPosition.position;

        lineRenderer.positionCount = 2;

        points[0] = _startPoint.normalized * Mathf.Clamp(_startPoint.magnitude, -10f, 10f);
        points[1] = _endPoint.normalized * Mathf.Clamp(_endPoint.magnitude, -10f, 10f);

        lineRenderer.SetPositions(points);
    }

    public void EndLine()
    {
        lineRenderer.positionCount = 0;
    }
}
