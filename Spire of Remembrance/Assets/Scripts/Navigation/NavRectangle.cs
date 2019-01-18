using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NavRectangle
{
	[SerializeField] private float m_centerX;
	[SerializeField] private float m_centerY;
	[SerializeField] private float m_width;
	[SerializeField] private float m_height;
	[SerializeField] private NavTerrainTypes m_navTerrainType;
	[SerializeField] private int[] adjoiningIDs;

	private List<NavRectangle> adjoiningRectangles = new List<NavRectangle>();

	public float centerX
	{
		get { return m_centerX; }
		private set { m_centerX = value; }
	}
	public float centerY
	{
		get { return m_centerY; }
		private set { m_centerY = value; }
	}
	public float width
	{
		get { return m_width; }
		private set { m_width = value; }
	}
	public float height
	{
		get { return m_height; }
		private set { m_height = value; }
	}
	public int navTerrainType {
		get { return (int)m_navTerrainType; }
		private set { m_navTerrainType = (NavTerrainTypes)value; }
	}

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

	public int ID;
	private static int nextID;

	static NavRectangle()
	{
		nextID = 0;
	}

	public NavRectangle()
	{
		ID = nextID++;
		adjoiningRectangles = new List<NavRectangle>();
	}

	public NavRectangle(float cx, float cy, float w, float h, int navType)
		: this()
	{
		centerX = cx;
		centerY = cy;
		width = w;
		height = h;
		navTerrainType = navType;
	}

	public NavRectangle(Vector2 center, Vector2 extents, int navType)
		: this(center.x, center.y, extents.x * 2, extents.y * 2, navType)
	{ }

	public NavRectangle(Vector2 center, int navType)
		: this(center.x, center.y, 1.0f, 1.0f, navType)
	{ }

	public void SerializeAdjacents(List<NavRectangle> list)
	{
		adjoiningIDs = new int[adjoiningRectangles.Count];
		for (int i = 0; i < adjoiningIDs.Length; ++i)
		{
			adjoiningIDs[i] = list.IndexOf(adjoiningRectangles[i]);
		}
	}

	public void DeserializeAdjacents(NavRectangle[] list)
	{
		nextID = list.Length;
		adjoiningRectangles = new List<NavRectangle>();
		foreach (int index in adjoiningIDs)
		{
			adjoiningRectangles.Add(list[index]);
		}
	}

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

		adjoiningRectangles.Add(other);
		other.adjoiningRectangles.Add(this);
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

	public void ClearAdjoinments()
	{
		adjoiningRectangles.Clear();
	}

	public IEnumerable<NavRectangle> GetAdjacentRectangles()
	{
		return adjoiningRectangles;
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
			if (maxEdgeY <= minEdgeY)
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
			if (maxEdgeX <= minEdgeX)
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
		float lowerCenter = (minY + y) / 2;
		float lowerHeight = y - minY;
		float upperCenter = (y + maxY) / 2;
		float upperHeight = maxY - y;
		NavRectangle lowerRect = new NavRectangle(centerX, lowerCenter, width, lowerHeight, navTerrainType);
		NavRectangle upperRect = new NavRectangle(centerX, upperCenter, width, upperHeight, navTerrainType);
		HandleSplitAdjoinments(lowerRect, upperRect);
		return new Pair<NavRectangle, NavRectangle>()
		{
			first = lowerRect,
			second = upperRect
		};
	}

	private void HandleSplitAdjoinments(NavRectangle rect1, NavRectangle rect2)
	{
		rect1.adjoiningRectangles.Add(rect2);
		rect2.adjoiningRectangles.Add(rect1);

		for (int i = 0; i < adjoiningRectangles.Count; ++i)
		{
			NavRectangle adjoining = adjoiningRectangles[i];
			adjoining.adjoiningRectangles.Remove(this);
			if (rect1.GetAdjoiningEdge(adjoining) != null)
			{
				rect1.adjoiningRectangles.Add(adjoining);
				adjoining.adjoiningRectangles.Add(rect1);
			}
			if (rect2.GetAdjoiningEdge(adjoining) != null)
			{
				rect2.adjoiningRectangles.Add(adjoining);
				adjoining.adjoiningRectangles.Add(rect2);
			}
		}
	}

	public Pair<NavRectangle, NavRectangle> SplitVertically(float x)
	{
		float leftCenter = (minX + x) / 2;
		float leftWidth = x - minX;
		float rightCenter = (x + maxX) / 2;
		float rightWidth = maxX - x;
		NavRectangle leftRect = new NavRectangle(leftCenter, centerY, leftWidth, height, navTerrainType);
		NavRectangle rightRect = new NavRectangle(rightCenter, centerY, rightWidth, height, navTerrainType);
		HandleSplitAdjoinments(leftRect, rightRect);
		return new Pair<NavRectangle, NavRectangle>()
		{
			first = leftRect,
			second = rightRect
		};
	}

	public bool ContainsPoint(Vector2 point)
	{
		return Mathf.Abs(point.x - centerX) <= width / 2 &&
			Mathf.Abs(point.y - centerY) <= height / 2;
	}

	public override bool Equals(object other)
	{
		if (!(other is NavRectangle))
		{
			return false;
		}
		NavRectangle rect = other as NavRectangle;
		return Mathf.Approximately(centerX, rect.centerX) &&
			Mathf.Approximately(centerY, rect.centerY) &&
			Mathf.Approximately(width, rect.width) &&
			Mathf.Approximately(height, rect.height) &&
			navTerrainType == rect.navTerrainType;
	}

	public static bool operator==(NavRectangle rect1, NavRectangle rect2)
	{
		if (object.ReferenceEquals(rect1, null))
		{
			return object.ReferenceEquals(rect2, null);
		}
		return rect1.Equals(rect2);
	}

	public static bool operator!=(NavRectangle rect1, NavRectangle rect2)
	{
		return !(rect1 == rect2);
	}

	public override int GetHashCode()
	{
		return Mathf.RoundToInt(13 * centerX + 23 * centerY + 37 *
			width + 57 * height + navTerrainType);
	}

	public override string ToString()
	{
		return "NavRect " + ID;
	}
}
