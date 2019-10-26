using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path {

	public readonly Vector3[] lookPoints;
	public readonly Line[] turnBoundaries;
	public readonly int finishLineIndex;

	public Path(Vector3[] waypoints, Vector3 startPos, float turnDst) {
		lookPoints = waypoints;
		turnBoundaries = new Line[lookPoints.Length];
		finishLineIndex = turnBoundaries.Length - 1;

		Vector2 previous = V3ToV2(startPos);
		for (int i = 0; i < lookPoints.Length; i++) {
			Vector2 curr = V3ToV2(lookPoints[i]);
			Vector2 dirToCurr = (curr - previous).normalized;
			Vector2 turnBoundary = (i == finishLineIndex) ? curr : curr - dirToCurr * turnDst;
			turnBoundaries[i] = new Line(turnBoundary, previous - dirToCurr * turnDst);
			previous = turnBoundary;
		}
	}

	Vector2 V3ToV2(Vector3 v) {
		// Check this?
		return new Vector2(v.x, v.y);
	}

	public void DrawWithGizmos() {

		Gizmos.color = Color.black;

		foreach (Vector3 p in lookPoints) {
			Gizmos.DrawCube(p + Vector3.up, Vector3.one);
		}

	}

}
