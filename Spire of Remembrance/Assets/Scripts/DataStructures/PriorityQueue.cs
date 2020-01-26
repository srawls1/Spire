using System;
using System.Diagnostics;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public class PriorityQueue<T> : ICollection<T> where T : IComparable<T>
{
	#region Private Variables

	private T[] data;
	private Dictionary<T, int> indices;

	#endregion // Private Variables

	#region Public Properties

	public int Count
    {
		get;
		private set;
    }

    public bool IsReadOnly
    {
        get { return false; }
    }

	#endregion // Public Properties

	#region Constructor

	public PriorityQueue(int initialCapacity = 7)
    {
        data = new T[initialCapacity];
		indices = new Dictionary<T, int>();
        Count = 0;
    }

	#endregion // Constructor

	#region Public Functions

	public T Peek()
	{
		if (Count == 0)
		{
			return default(T);
		}

		return data[0];
	}

	public T Pop()
	{
		if (Count == 0)
		{
			return default(T);
		}

		T ret = data[0];
		indices.Remove(ret);
		data[0] = data[--Count];
		WalkDown(0);
		return ret;
	}

	public void UpdatePosition(T currentItem, T newItem)
	{
		int index;
		if (indices.TryGetValue(currentItem, out index) &&
			index >= 0 && index < Count)
		{
			data[index] = newItem;
			// Just do both of them. Only one will have an effect,
			// the other will immediately return
			WalkUp(index);
			WalkDown(index);
		}
	}

    public void Add(T item)
    {
		EnsureCapacity(Count + 1);
		data[Count] = item;
		WalkUp(Count++);
    }

    public void Clear()
    {
		for (int i = 0; i < Count; ++i)
		{
			data[i] = default(T);
		}
		Count = 0;
		indices.Clear();
    }

    public bool Contains(T item)
    {
		int index;
		return indices.TryGetValue(item, out index) &&
			index >= 0 && index < Count;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
		data.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
		int index;
		bool present = indices.TryGetValue(item, out index) &&
			index >= 0 && index < Count;
		if (!present)
		{
			return false;
		}

		data[index] = data[--Count];
		WalkUp(index);
		WalkDown(index);
		return true;
	}

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Count; ++i)
		{
			yield return data[i];
		}
    }

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
    }

	#endregion // Public Functions

	#region Private Functions

	private void EnsureCapacity(int newCapacity)
	{
		if (newCapacity > data.Length)
		{
			T[] newData = new T[data.Length * 2 + 1];
			data.CopyTo(newData, 0);
			data = newData;
		}
	}

	private void WalkDown(int startIndex)
	{
		int index = startIndex;
		T walkingItem = data[index];

		while (index < Count)
		{
			int leftChild = index * 2 + 1;
			int rightChild = index * 2 + 2;

			if (leftChild >= Count)
			{
				break;
			}

			int child = leftChild;
			if (rightChild < Count &&
				data[rightChild].CompareTo(data[leftChild]) < 0)
			{
				child = rightChild;
			}

			if (data[child].CompareTo(walkingItem) < 0)
			{
				data[index] = data[child];
				indices[data[index]] = index;
				index = child;
			}
			else
			{
				break;
			}
		}

		data[index] = walkingItem;
		indices[data[index]] = index;
	}

	private void WalkUp(int startIndex)
	{
		int index = startIndex;
		T walkingItem = data[startIndex];

		while (index > 0)
		{
			int parent = (index - 1) / 2;

			if (walkingItem.CompareTo(data[parent]) < 0)
			{
				data[index] = data[parent];
				indices[data[index]] = index;
				index = parent;
			}
			else
			{
				break;
			}
		}

		data[index] = walkingItem;
		indices[data[index]] = index;
	}

	public override string ToString()
	{
		StringBuilder s = new StringBuilder('[');
		bool commaNeeded = false;
		for (int i = 0; i < Count; ++i)
		{
			if (commaNeeded)
			{
				s.Append(',');
			}
			s.Append(data[i].ToString());
			commaNeeded = true;
		}

		s.Append(']');
		return s.ToString();
	}

	#endregion // Private Functions
}
