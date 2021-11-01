using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawArrow : MonoBehaviour
{

    float minOffset = -5f;
    float maxOffset = 5f;
    LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void RenderLine(Vector3 _startPoint, Vector3 _endPoint)
    {
        lineRenderer.positionCount = 2;
        Vector3[] points = new Vector3[2];
        _startPoint = new Vector3(Mathf.Clamp(_startPoint.x, minOffset, maxOffset),
                                  Mathf.Clamp(_startPoint.y, minOffset, maxOffset),
                                  0f);
        _endPoint   = new Vector3(Mathf.Clamp(_endPoint.x, minOffset, maxOffset),
                                  Mathf.Clamp(_endPoint.y, minOffset, maxOffset),
                                  0f);

        points[0] = _startPoint;
        points[1] = _endPoint;

        lineRenderer.SetPositions(points);
    }

    public void EndLine()
    {
        lineRenderer.positionCount = 0;
    }
}
