using UnityEngine;
using System.Collections.Generic;

public class BezierPath {
	
    private const float MINIMUM_SQR_DISTANCE = 0.01f;
	// This corresponds to about 172 degrees, 8 degrees from a straight line
    private const float DIVISION_THRESHOLD = -0.99f; 

    private List<Vector3> controlPoints;

    public BezierPath() {
        controlPoints = new List<Vector3>();
    }

    public void SetControlPoints(List<Vector3> newControlPoints) {
        controlPoints.Clear();
        controlPoints.AddRange(newControlPoints);
    }

	public List<Vector3> GetControlPoints() { return controlPoints; }

    public Vector3 CalculateBezierPoint(float t) {
        Vector3 p0 = controlPoints[0];
        Vector3 p1 = controlPoints[1];
        Vector3 p2 = controlPoints[2];
        Vector3 p3 = controlPoints[3];
        return CalculateBezierPoint(t, p0, p1, p2, p3);
    }
		
	public List<Vector3> GetDrawingPoints() {
        List<Vector3> drawingPoints = new List<Vector3>();
		List<Vector3> bezierCurveDrawingPoints = FindDrawingPoints();
        drawingPoints.AddRange(bezierCurveDrawingPoints);
        return drawingPoints;
    }

    List<Vector3> FindDrawingPoints() {
        List<Vector3> pointList = new List<Vector3>();
        Vector3 left = CalculateBezierPoint(0);
        Vector3 right = CalculateBezierPoint(1);
        pointList.Add(left);
        pointList.Add(right);
        FindDrawingPoints(0, 1, pointList, 1);
        return pointList;
    }
    
    int FindDrawingPoints(float t0, float t1, List<Vector3> pointList, int insertionIndex) {
        Vector3 left = CalculateBezierPoint(t0);
        Vector3 right = CalculateBezierPoint(t1);

        if ((left - right).sqrMagnitude < MINIMUM_SQR_DISTANCE) { return 0; }

        float tMid = (t0 + t1) / 2;
        Vector3 mid = CalculateBezierPoint(tMid);
        Vector3 leftDirection = (left - mid).normalized;
        Vector3 rightDirection = (right - mid).normalized;

        if (Vector3.Dot(leftDirection, rightDirection) > DIVISION_THRESHOLD || Mathf.Abs(tMid - 0.5f) < 0.0001f) {
            int pointsAddedCount = 0;
            pointsAddedCount += FindDrawingPoints(t0, tMid, pointList, insertionIndex);
            pointList.Insert(insertionIndex + pointsAddedCount, mid);
            pointsAddedCount++;
            pointsAddedCount += FindDrawingPoints(tMid, t1, pointList, insertionIndex + pointsAddedCount);
            return pointsAddedCount;
        }

        return 0;
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }

}