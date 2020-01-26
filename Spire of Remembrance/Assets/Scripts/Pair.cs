using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pair<T, U>
{
	public Pair(T first, U second)
	{
		First = first;
		Second = second;
	}

	public T First;
	public U Second;
}

public class ComparablePair<T, U> : Pair<T, U>, IComparable<ComparablePair<T, U>> where T : IComparable<T>
{
	public ComparablePair(T first, U second)
		: base(first, second)
	{ }

	public int CompareTo(ComparablePair<T, U> other)
	{
		return First.CompareTo(other.First);
	}
}
