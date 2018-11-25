using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BspNode
{
	private int maxNumRects;

	public bool root
	{
		get; private set;
	}

	private List<NavRectangle> rects;
	public IReadOnlyList<NavRectangle> rectangles
	{
		get
		{
			return rects.AsReadOnly();
		}
	}

	public BspNode ulChild
	{
		get; private set;
	}
	public BspNode urChild
	{
		get; private set;
	}
	public BspNode llChild
	{
		get; private set;
	}
	public BspNode lrChild
	{
		get; private set;
	}

	public float minX
	{
		get; private set;
	}
	public float maxX
	{
		get; private set;
	}
	public float minY
	{
		get; private set;
	}
	public float maxY
	{
		get; private set;
	}

	public BspNode(float minX, float minY, float maxX, float maxY, int maxNumRects)
	{
		root = true;
		rects = new List<NavRectangle>();
		this.minX = minX;
		this.minY = minY;
		this.maxX = maxX;
		this.maxY = maxY;
		this.maxNumRects = maxNumRects;
	}

	public void AddRect(NavRectangle rect)
	{
		rects.Add(rect);
		if (rects.Count > maxNumRects)
		{
			Split();
		}
	}

	private void Split()
	{
		throw new NotImplementedException();
	}

	public NavRectangle GetRectangleContainingPoint(Vector2 point)
	{
		if (root)
		{
			for (int i = 0; i < rects.Count; ++i)
			{
				if (rects[i].ContainsPoint(point))
				{
					return rects[i];
				}
			}
		}
		else
		{
			float midX = (minX + maxX) / 2;
			float midY = (minY + maxY) / 2;
			if (point.x < midX)
			{
				if (point.y < midY)
				{
					return llChild.GetRectangleContainingPoint(point);
				}
				else
				{
					return ulChild.GetRectangleContainingPoint(point);
				}
			}
			else
			{
				if (point.y < midY)
				{
					return lrChild.GetRectangleContainingPoint(point);
				}
				else
				{
					return urChild.GetRectangleContainingPoint(point);
				}
			}
		}
	}
}
