using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
	List<Vector2> points;

	public void AddPoint(Vector2 point)
	{
		points.Add(point);
	}

	public void ClearPathPastPoint(int index)
	{
		points.RemoveRange(index, points.Count - index);
	}

	public void SetPoint(int index, Vector2 point)
	{
		points[index] = point;
	}

	public Vector2 this[int n]
	{
		get
		{
			return points[n];
		}
	}
}
