using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class Pathfinder : MonoBehaviour {

	Grid grid;

	private void Awake() {
		grid = GetComponent<Grid>();
	}

	public void FindPath(PathRequest request, Action<PathResult> callback) {
		Node startNode = grid.NodeFromWorldPoint(request.pathStart);
		Node targetNode = grid.NodeFromWorldPoint(request.pathEnd);

		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;

		startNode.parent = startNode;

		if (startNode.isWalkable && targetNode.isWalkable) {
			Heap<Node> openSet = new Heap<Node>(grid.BoxSize);
			HashSet<Node> closedSet = new HashSet<Node>();
			openSet.Add(startNode);

			while (openSet.Count > 0) {
				Node currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);

				if (currentNode == targetNode) {
					pathSuccess = true;
					break;
				}

				foreach (Node n in grid.GetNeighbors(currentNode)) {
					if (!n.isWalkable || closedSet.Contains(n)) {
						continue;
					}

					int newCost = currentNode.GCost + DistanceOfNodes(currentNode, n);

					if (newCost < n.GCost || !openSet.Contains(n)) {
						n.GCost = newCost;
						n.HCost = DistanceOfNodes(n, targetNode);
						n.parent = currentNode;

						if (!openSet.Contains(n)) {
							openSet.Add(n);
						} else {
							openSet.UpdateItem(n);
						}
					}
				}

			}
		}
		
		if (pathSuccess) {
			waypoints = RetracePath(startNode, targetNode);
			pathSuccess = waypoints.Length > 0;
		}

		callback(new PathResult(waypoints, pathSuccess, request.callback));

	}

	Vector3[] RetracePath(Node start, Node end) {
		List<Node> path = new List<Node>();
		Node curr = end;

		while (curr != start) {
			path.Add(curr);
			curr = curr.parent;
		}

		Vector3[] waypoints = simplifyPath(path);

		Array.Reverse(waypoints);
		
		return waypoints;
	}
	
	Vector3[] simplifyPath(List<Node> path) {
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;

		for (int i = 1; i < path.Count; i++) {
			Vector2 newDirection = new Vector2(path[i - 1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY);
			if (newDirection != directionOld) {
				waypoints.Add(path[i].pos);
			}
			directionOld = newDirection;
		}

		return waypoints.ToArray();

	}

	int DistanceOfNodes(Node i, Node j) {
		int x = Mathf.Abs(i.GridX - j.GridX);
		int y = Mathf.Abs(i.GridY - j.GridY);

		if (x > y) {
			return 14 * y + 10 * (x - y);
		}

		return 14 * x + 10 * (y - x);
		
	}

}
