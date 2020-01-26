using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisjointSet<T> : ICollection<T>
{
	#region Inner Class

	private class SetNode
	{
		public bool isRepresentative;
		public SetNode representative;
		public T value;

		public override bool Equals(object obj)
		{
			SetNode other = obj as SetNode;
			if (other == null)
			{
				return false;
			}

			return value.Equals(other.value);
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
	}

	#endregion // Inner Class

	#region Private Variables

	private IDictionary<T, SetNode> elements;

	#endregion // Private Variables

	#region Public Properties

	public int Count
	{
		get
		{
			return elements.Count;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	#endregion // Public Properties

	#region Constructor

	public DisjointSet()
	{
		elements = new Dictionary<T, SetNode>();
	}

	#endregion // Constructor

	#region Public Functions

	public void Add(T item)
	{
		SetNode node = new SetNode();
		node.isRepresentative = true;
		node.representative = node;
		node.value = item;
		elements.Add(item, node);
	}

	public void Union(T item1, T item2)
	{
		Debug.Assert(item1 != null);
		Debug.Assert(item2 != null);
		Debug.Assert(elements.ContainsKey(item1));
		Debug.Assert(elements.ContainsKey(item2));

		SetNode rep1 = GetRepresentative(elements[item1]);
		SetNode rep2 = GetRepresentative(elements[item2]);
		SetNode tail2 = GetTail(rep2);

		SetRepresentative(rep2, rep1);
		SetTail(rep1, tail2);
	}

	public void Clear()
	{
		elements.Clear();
	}

	public bool Contains(T item)
	{
		return elements.ContainsKey(item);
	}

	public bool AreUnited(T item1, T item2)
	{
		Debug.Assert(item1 != null);
		Debug.Assert(item2 != null);
		Debug.Assert(elements.ContainsKey(item1));
		Debug.Assert(elements.ContainsKey(item2));

		SetNode rep1 = GetRepresentative(elements[item1]);
		SetNode rep2 = GetRepresentative(elements[item2]);
		return rep1 == rep2;
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		foreach (KeyValuePair<T, SetNode> entry in elements)
		{
			if (arrayIndex >= array.Length)
			{
				break;
			}

			array[arrayIndex++] = entry.Key;
		}
	}

	public IEnumerator<T> GetEnumerator()
	{
		return elements.Keys.GetEnumerator();
	}

	public bool Remove(T item)
	{
		throw new NotImplementedException();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	#endregion // Public Functions

	#region Private Functions

	private SetNode GetRepresentative(SetNode node)
	{
		SetNode iter;
		for (iter = node; !iter.isRepresentative; iter = iter.representative);
		return iter;
	}

	private void SetRepresentative(SetNode node, SetNode rep)
	{
		SetNode intermediateRep = GetRepresentative(node);
		intermediateRep.isRepresentative = false;
		intermediateRep.representative = rep;
	}

	private SetNode GetTail(SetNode node)
	{
		return GetRepresentative(node).representative;
	}

	private void SetTail(SetNode node, SetNode tail)
	{
		SetNode rep = GetRepresentative(node);
		rep.representative = tail;
	}

	#endregion // Private Functions
}
