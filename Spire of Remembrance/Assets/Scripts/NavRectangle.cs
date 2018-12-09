using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavRectangle
{
	public float centerX { get; private set; }
	public float centerY { get; private set; }
	public float width { get; private set; }
	public float height { get; private set; }
	public int navTerrainType { get; private set; }

	public float minX
	{
		get
		{
			return centerX - width / 2;
		}
	}
	public float maxX
	{
		get
		{
			return centerX + width / 2;
		}
	}
	public float minY
	{
		get
		{
			return centerY - height / 2;
		}
	}
	public float maxY
	{
		get
		{
			return centerY + height / 2;
		}
	}

	private LinkedList<NavRectangle> adjoiningRectangles;

	public NavRectangle(float cx, float cy, float w, float h, int navType)
	{
		centerX = cx;
		centerY = cy;
		width = w;
		height = h;
		navTerrainType = navType;

		adjoiningRectangles = new LinkedList<NavRectangle>();
	}

	public NavRectangle(Vector2 center, Vector2 extents, int navType)
		: this(center.x, center.y, extents.x * 2, extents.y * 2, navType)
	{ }

	public NavRectangle(Vector2 center, int navType)
		: this(center.x, center.y, 1.0f, 1.0f, navType)
	{ }

	// This will return whether or not the adjoining rectangle is separate from this one
	public bool Adjoin(NavRectangle other)
	{
		if (centerX - other.centerX > width / 2 + other.width / 2 + 0.1f ||
			centerY - other.centerY > height / 2 + other.height / 2 + 0.1f)
		{
			return true;
		}

		if (navTerrainType == other.navTerrainType && FitsWholeSide(other))
		{
			float minX = Mathf.Min(centerX - width / 2, other.centerX - other.width / 2);
			float maxX = Mathf.Max(centerX + width / 2, other.centerX + other.width / 2);
			float minY = Mathf.Min(centerY - height / 2, other.centerY - other.height / 2);
			float maxY = Mathf.Max(centerY + height / 2, other.centerY + other.height / 2);
			centerX = (minX + maxX) / 2;
			centerY = (minY + maxY) / 2;
			width = maxX - minX;
			height = maxY - minY;
			return false;
		}

		adjoiningRectangles.AddLast(other);
		return true;
	}

	private bool FitsWholeSide(NavRectangle other)
	{
		if (Mathf.Approximately(centerY, other.centerY) &&
			Mathf.Approximately(height, other.height))
		{
			return true;
		}

		if (Mathf.Approximately(centerX, other.centerX) &&
			Mathf.Approximately(width, other.width))
		{
			return true;
		}

		return false;
	}

	public IEnumerator<NavRectangle> GetAdjacentRectangles()
	{
		return adjoiningRectangles.GetEnumerator();
	}

	public Pair<Vector2, Vector2> GetAdjoiningEdge(NavRectangle other)
	{
		float halfWidth = width / 2;
		float minX = centerX - halfWidth;
		float maxX = centerX + halfWidth;
		float halfHeight = height / 2;
		float minY = centerY - halfHeight;
		float maxY = centerY + halfHeight;

		float halfWidth2 = other.width / 2;
		float minX2 = other.centerX - halfWidth2;
		float maxX2 = other.centerX + halfWidth2;
		float halfHeight2 = other.height / 2;
		float minY2 = other.centerY - halfHeight2;
		float maxY2 = other.centerY + halfHeight2;

		bool leftRightEdge = false;
		float edgeX = 0f;
		float edgeY = 0f;

		if (Mathf.Abs(minX - maxX2) < 0.1f) // other is to the left of this one
		{
			leftRightEdge = true;
			edgeX = minX;
		}
		else if (Mathf.Abs(maxX - minX2) < 0.1f) // other is to the right of this
		{
			leftRightEdge = true;
			edgeX = maxX;
		}
		else if (Mathf.Abs(minY - maxY2) < 0.1f) // other is below this
		{
			leftRightEdge = false;
			edgeY = minY;
		}
		else if (Mathf.Abs(maxY - minY2) < 0.1f)
		{
			leftRightEdge = false;
			edgeY = maxY;
		}
		else
		{
			return null;
		}

		if (leftRightEdge)
		{
			float minEdgeY = Mathf.Max(minY, minY2);
			float maxEdgeY = Mathf.Min(maxY, maxY2);
			if (maxEdgeY < minEdgeY)
			{
				return null;
			}
			return new Pair<Vector2, Vector2>()
			{
				first = new Vector2(edgeX, minEdgeY),
				second = new Vector2(edgeX, maxEdgeY)
			};
		}
		else
		{
			float minEdgeX = Mathf.Max(minX, minX2);
			float maxEdgeX = Mathf.Min(maxX, maxX2);
			if (maxEdgeX < minEdgeX)
			{
				return null;
			}
			return new Pair<Vector2, Vector2>()
			{
				first = new Vector2(minEdgeX, edgeY),
				second = new Vector2(maxEdgeX, edgeY)
			};
		}
	}

	public Pair<NavRectangle, NavRectangle> SplitHorizontally(float y)
	{
		throw new NotImplementedException();
	}

	public Pair<NavRectangle, NavRectangle> SplitVertically(float x)
	{
		throw new NotImplementedException();
	}

	public bool ContainsPoint(Vector2 point)
	{
		return Mathf.Abs(point.x - centerX) < width / 2 &&
			Mathf.Abs(point.y - centerY) < height / 2;
	}
}
