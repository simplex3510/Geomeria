using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawArrow : MonoBehaviour
{
    LineRenderer lineRenderer;
    Vector3[] points;

    private void Awake()
    {
        points = new Vector3[2];
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void RenderLine(Vector3 _startPoint, Vector3 _endPoint)
    {
        lineRenderer.positionCount = 2;

        points[0] = _startPoint.normalized * Mathf.Clamp(_startPoint.magnitude, -10, 10);
        points[1] = _endPoint.normalized * Mathf.Clamp(_endPoint.magnitude, -10, 10);

        lineRenderer.SetPositions(points);
    }

    public void EndLine()
    {
        lineRenderer.positionCount = 0;
    }
}
