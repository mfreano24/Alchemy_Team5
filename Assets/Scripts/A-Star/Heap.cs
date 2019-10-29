using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T> {

	T[] items;
	int size;

	public Heap(int _size) {
		items = new T[_size];
	}

	public void Add(T item) {
		item.HeapIndex = size;
		items[size] = item;
		SortUp(item);
		size++;
	}

	public T RemoveFirst() {
		T returning = items[0];
		size--;
		items[0] = items[size];
		items[0].HeapIndex = 0;
		SortDown(items[0]);
		return returning;
	}

	void SortDown(T item) {
		while (true) {
			int leftIndex = item.HeapIndex * 2 + 1;
			int rightIndex = item.HeapIndex * 2 + 2;
			int swapIndex = 0;

			if (leftIndex < size) {
				swapIndex = leftIndex;

				if (rightIndex < size) {
					if (items[leftIndex].CompareTo(items[rightIndex]) < 0) {
						swapIndex = rightIndex;
					}
				}

				if (item.CompareTo(items[swapIndex]) < 0) {
					Swap(item, items[swapIndex]);
				} else {
					return;
				}

			} else {
				return;
			}
		}
	}

	void SortUp(T item) {
		int parentIndex = (item.HeapIndex - 1) / 2;
		while (true) {
			T parent = items[parentIndex];
			if (item.CompareTo(parent) > 0) {
				Swap(item, parent);
			} else {
				break;
			}

			parentIndex = (item.HeapIndex - 1) / 2;
		}
	}

	void Swap(T itemA, T itemB) {
		items[itemA.HeapIndex] = itemB;
		items[itemB.HeapIndex] = itemA;
		int temp = itemA.HeapIndex;
		itemA.HeapIndex = itemB.HeapIndex;
		itemB.HeapIndex = temp;
	}

	public int Count {
		get {
			return size;
		}
	}

	public void UpdateItem(T item) {
		SortUp(item);
	}

	public bool Contains(T item) {
		return Equals(items[item.HeapIndex], item);
	}

}

public interface IHeapItem<T> : IComparable {
	int HeapIndex { get; set; }
}
