using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line {

	const float verticalLineGradient = 1e5f;

	float gradient;
	float y_int;

	Vector2 p1, p2;

	float perpendicular;
	bool approachSide;

	public Line(Vector2 point, Vector2 perp) {
		float dx = point.x - perp.x;
		float dy = point.y - perp.y;

		if (dx == 0) {
			perpendicular = verticalLineGradient;
		} else {
			perpendicular = dy / dx;
		}
		
		if (perpendicular == 0) {
			gradient = verticalLineGradient;
		} else {
			gradient = -1 / perpendicular;
		}

		y_int = point.y - gradient * point.x;
		p1 = point;
		p2 = point + new Vector2(1, gradient);

		// Need a starting value
		approachSide = false;
		approachSide = GetSide(perp);
	}

	bool GetSide(Vector2 p) {
		return (p.x - p1.x) * (p2.y - p1.y) > (p.y - p1.y) * (p2.x - p1.x);
	}

	public bool CrossedLine(Vector2 p) {
		return GetSide(p) != approachSide;
	}

	public float DistanceFromPoint(Vector2 p) {
		float yIntPerp = p.y - perpendicular * p.x;
		float intX = (yIntPerp - y_int) / (gradient - perpendicular);
		float intY = gradient * intX + y_int;
		return Vector2.Distance(p, new Vector2(intX, intY));
	}

	public void DrawWithGizmos(float len) {
		Vector3 lineDir = new Vector3(1, gradient, 0).normalized;
		Vector3 lineCenter = new Vector3(p1.x, p1.y, 1);
		Gizmos.DrawLine(lineCenter - lineDir * len / 2, lineCenter + lineDir * len / 2);
	}
}
