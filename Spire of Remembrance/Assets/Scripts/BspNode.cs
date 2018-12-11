using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BspNode
{
	private int maxNumRects;

	public bool root
	{
		get; private set;
	}

	private List<NavRectangle> rects;
	public ReadOnlyCollection<NavRectangle> rectangles
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
		if (root)
		{
			rects.Add(rect);
			if (rects.Count > maxNumRects)
			{
				Split();
			}
		}
		else
		{
			float midX = (minX + maxX) / 2;
			float midY = (minY + maxY) / 2;
			if (rect.minX < midX && rect.maxX > midX)
			{
				Pair<NavRectangle, NavRectangle> newRects = rect.SplitVertically(midX);
				AddRect(newRects.first);
				AddRect(newRects.second);
				return;
			}
			if (rect.minY < midY && rect.maxY > midY)
			{
				Pair<NavRectangle, NavRectangle> newRects = rect.SplitHorizontally(midY);
				AddRect(newRects.first);
				AddRect(newRects.second);
				return;
			}

			if (rect.centerX < midX)
			{
				if (rect.centerY < midY)
				{
					llChild.AddRect(rect);
				}
				else
				{
					ulChild.AddRect(rect);
				}
			}
			else
			{
				if (rect.centerY < midY)
				{
					lrChild.AddRect(rect);
				}
				else
				{
					urChild.AddRect(rect);
				}
			}
		}
	}

	private void Split()
	{
		float midX = (minX + maxX) / 2;
		float midY = (minY + maxY) / 2;
		llChild = new BspNode(minX, minY, midX, midY, maxNumRects);
		ulChild = new BspNode(minX, midY, midX, maxY, maxNumRects);
		lrChild = new BspNode(midX, minY, maxX, midY, maxNumRects);
		urChild = new BspNode(midX, midY, maxX, maxY, maxNumRects);
		root = false;

		for (int i = 0; i < rects.Count; ++i)
		{
			AddRect(rects[i]);
		}
		
		rects.Clear();

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
			return null;
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
