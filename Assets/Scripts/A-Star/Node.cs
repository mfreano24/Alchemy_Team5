using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {

	public bool isWalkable;
	public Vector2 pos;
	public int GridX;
	public int GridY;
	
	public int GCost;
	public int HCost;
	public int FCost { get { return GCost + HCost; } }
	
	int heapIndex;
	
	public Node parent;
	
	public Node(bool _isWalkable, Vector2 _pos, int _x, int _y) {
		this.isWalkable = _isWalkable;
		this.pos = _pos;
		this.GridX = _x;
		this.GridY = _y;
	}
	
	public int HeapIndex {
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}
	
	public int CompareTo(object obj) {
		int compare = FCost.CompareTo((obj as Node).FCost);
	
		if (compare == 0) {
			compare = HCost.CompareTo((obj as Node).HCost);
		}
	
		return -compare;
	}
}
