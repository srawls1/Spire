using System;
using System.Collections.Generic;

public class PriorityQueue<PriorityType, PayloadType> where PriorityType : IComparable<PriorityType>
{
	private class EntryType : Pair<PriorityType, PayloadType>, IComparable<EntryType>
	{
		public int CompareTo(EntryType other)
		{
			return first.CompareTo(other.first);
		}
	}

	private EntryType[] minHeap;
	private Dictionary<PayloadType, int> indices;

	public PriorityQueue()
	{
		minHeap = new EntryType[1];
		indices = new Dictionary<PayloadType, int>();
		size = 0;
	}

	public int size
	{
		get; private set;
	}

	public void Add(PriorityType priority, PayloadType payload)
	{
		EntryType entry = new EntryType()
		{
			first = priority,
			second = payload
		};
		if (minHeap.Length == size)
		{
			Expand();
		}

		minHeap[size++] = entry;
		WalkUp(size - 1);
	}

	public PayloadType Peek()
	{
		return minHeap[0].second;
	}

	public PayloadType Pop()
	{
		PayloadType ret = minHeap[0].second;
		indices.Remove(ret);
		minHeap[0] = minHeap[--size];
		WalkDown(0);
		return ret;
	}

	public PriorityType PeekPriority()
	{
		return minHeap[0].first;
	}

	public bool Contains(PayloadType payload)
	{
		int index;
		return indices.TryGetValue(payload, out index) &&
			index >= 0 && index < minHeap.Length;
	}

	public PriorityType GetPriority(PayloadType payload)
	{
		int index;
		if (indices.TryGetValue(payload, out index) &&
			index >= 0 && index < minHeap.Length)
		{
			EntryType entry = minHeap[index];
			return entry.first;
		}

		throw new Exception("This payload is not contained in the queue");
	}

	public bool LowerPriority(PayloadType payload, PriorityType newPriority)
	{
		int index;
		if (indices.TryGetValue(payload, out index) &&
			index >= 0 && index < minHeap.Length)
		{
			EntryType entry = minHeap[index];
			if (newPriority.CompareTo(entry.first) < 0)
			{
				entry.first = newPriority;
				WalkUp(index);
				return true;
			}
		}

		return false;
	}

	private void WalkDown(int startingIndex)
	{
		int index = startingIndex;
		EntryType walkingEntry = minHeap[index];
		while (index < size)
		{
			int leftChild = GetLeftChildIndex(index);
			int rightChild = GetRightChildIndex(index);
			if (leftChild >= size)
			{
				break;
			}
			int child = leftChild;
			if (rightChild < size &&
				minHeap[rightChild].CompareTo(minHeap[leftChild]) < 0)
			{
				child = rightChild;
			}

			if (minHeap[child].CompareTo(walkingEntry) < 0)
			{
				minHeap[index] = minHeap[child];
				indices[minHeap[index].second] = index;
				index = child;
			}
			else
			{
				break;
			}
		}

		minHeap[index] = walkingEntry;
		indices[minHeap[index].second] = index;
	}

	private void WalkUp(int startingIndex)
	{
		int index = startingIndex;
		EntryType walkingEntry = minHeap[index];
		while (index > 0)
		{
			int parent = GetParentIndex(index);
			if (walkingEntry.CompareTo(minHeap[parent]) < 0)
			{
				minHeap[index] = minHeap[parent];
				indices[minHeap[index].second] = index;
				index = parent;
			}
			else
			{
				break;
			}
		}

		minHeap[index] = walkingEntry;
		indices[minHeap[index].second] = index;
	}

	private static int GetLeftChildIndex(int parentIndex)
	{
		return parentIndex * 2 + 1;
	}

	private static int GetRightChildIndex(int parentIndex)
	{
		return parentIndex * 2 + 2;
	}

	private static int GetParentIndex(int childIndex)
	{
		return (childIndex - 1) / 2;
	}

	private void Expand()
	{
		EntryType[] newMinHeap = new EntryType[minHeap.Length * 2 + 1];
		minHeap.CopyTo(newMinHeap, 0);
		minHeap = newMinHeap;
	}
}
