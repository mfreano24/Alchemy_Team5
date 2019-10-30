using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

	public Transform player;

	public LayerMask unwalkableMask;
	public Vector2 worldSize;
	public float nodeRadius;
	Node[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	public int BoxSize {
		get {
			return gridSizeX * gridSizeY;
		}
	}

	void Awake() {
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(worldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(worldSize.y / nodeDiameter);
		CreateGrid();
	}

	void CreateGrid() {
		grid = new Node[gridSizeX,gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * worldSize.x/2 - Vector3.up * worldSize.y/2;

		for (int x = 0; x < gridSizeX; x ++) {
			for (int y = 0; y < gridSizeY; y ++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
				grid[x,y] = new Node(walkable,worldPoint, x,y);
			}
		}
	}


	public Node NodeFromWorldPoint(Vector3 worldPos) {
		float perX = (worldPos.x + worldSize.x / 2) / worldSize.x;
		float perY = (worldPos.y + worldSize.y / 2) / worldSize.y;

		perX = Mathf.Clamp01(perX);
		perY = Mathf.Clamp01(perY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * perX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * perY);

		return grid[x, y];
	}

	public List<Node> GetNeighbors(Node item) {
		List<Node> neighbors = new List<Node>();

		for (int x = -1; x < 2; x++) {
			for (int y = -1; y < 2; y++) {
				// Skip current node (item)

				int checkX = item.GridX + x;
				int checkY = item.GridY + y;

				if (x != 0 && y != 0 && (checkX >= 0 && checkY >= 0 && checkX < gridSizeX && checkY < gridSizeY)) {
					neighbors.Add(grid[checkX, checkY]);
				}
			}
		}

		return neighbors;

	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position, new Vector3(worldSize.x, worldSize.y, 1));
		if (grid != null) {
			foreach (Node n in grid) {
				Gizmos.color = (n.isWalkable) ? Color.white : Color.red;
				Gizmos.DrawCube(n.pos, Vector3.one * (nodeDiameter - .1f));
			}
		}
	}
}
