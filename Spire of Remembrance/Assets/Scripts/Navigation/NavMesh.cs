using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NavTerrainTypes : int
{
	Floor = 1,
	Pit = 2,
	ThinWall = 4,
	ThickWall = 8,
	RatTunnel = 16,
	Door = 32
}

public class NavMesh : MonoBehaviour
{
	private static NavMesh m_instance;
	public static NavMesh instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = FindObjectOfType<NavMesh>();
			}

			return m_instance;
		}
	}

	void Awake()
	{
		if (m_instance == null)
		{
			m_instance = this;
		}
		else if (m_instance != this)
		{
			Destroy(this);
		}
	}

	[SerializeField] private float minX;
	[SerializeField] private float maxX;
	[SerializeField] private float minY;
	[SerializeField] private float maxY;
	[SerializeField] private int maxRectsPerCell;
	[SerializeField] private NavRectangle[] rects;

	public IEnumerable<NavRectangle> rectangles
	{
		get { return rects; } 
	}

	private BspNode root;

	void Start()
	{
		BuildMesh();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		for (int i = 0; i < rects.Length; ++i)
		{
			Gizmos.DrawWireCube(new Vector2(rects[i].centerX, rects[i].centerY),
								new Vector2(rects[i].width, rects[i].height));
		}
	}

	private void BuildMesh()
	{
		root = new BspNode(minX, minY, maxX, maxY, maxRectsPerCell);
		for (int i = 0; i < rects.Length; ++i)
		{
			rects[i].DeserializeAdjacents(rects);
		}
		for (int i = 0; i < rects.Length; ++i)
		{
			root.AddRect(rects[i]);
		}
	}

	public List<Vector2> GetClosestPath(Vector2 start, Vector2 end, int linkTypeMask)
	{
		NavRectangle startingRect = root.GetRectangleContainingPoint(start);
		NavRectangle endRect = root.GetRectangleContainingPoint(end);
		if (startingRect == null || endRect == null)
		{
			return null;
		}
		List<NavRectangle> rectSequence = AStarPath(startingRect, endRect, linkTypeMask);

		List<Vector2> points = new List<Vector2>();
		points.Add(start);
		for (int i = 0; i < rectSequence.Count - 1; ++i)
		{
			Vector2 dest = i < rectSequence.Count - 2 ?
				new Vector2(rectSequence[i + 2].centerX, rectSequence[i + 2].centerY) :
				end;
			points.Add(GetBestEdgePoint(rectSequence[i], rectSequence[i + 1], points[i], dest));
		}
		points.Add(end);
		return points;
	}

	private Vector2 GetBestEdgePoint(NavRectangle rect1, NavRectangle rect2, Vector2 currentPos, Vector2 destination)
	{
		Pair<Vector2, Vector2> connectingEdge = rect1.GetAdjoiningEdge(rect2);
		float edgeDx = connectingEdge.second.x -  connectingEdge.first.x;
		float edgeDy = connectingEdge.second.y - connectingEdge.first.y;
		float pathDx = destination.x - currentPos.x;
		float pathDy = destination.y - currentPos.y;

		// This is kinda messy, but it's based on setting up parametric equations
		// for the edge and line from current to destination, setting up a 2x2 system of
		// equations, and solving for the first variable (the edge parameter) using Cramer's
		// rule. If this is between 0 and 1, the line from current to destination intersects
		// the shared edge
		float detA1 = pathDx * (currentPos.y - connectingEdge.first.y) - pathDy * (currentPos.x - connectingEdge.first.x);
		float detA = pathDx * edgeDy - pathDy * edgeDx;
		float t = detA1 / detA;
		t = Mathf.Clamp(t, 0f, 1f);
		Vector2 edgePoint = Vector2.Lerp(connectingEdge.first, connectingEdge.second, t);

		return edgePoint;
	}

	private List<NavRectangle> AStarPath(NavRectangle start, NavRectangle end, int linkTypeMask)
	{
		if (start == end)
		{
			return new List<NavRectangle>() { start };
		}
		PriorityQueue<float, NavRectangle> openRects = new PriorityQueue<float, NavRectangle>();
		openRects.Add(Heuristic(start, end), start);
		Dictionary<NavRectangle, float> closedRects = new Dictionary<NavRectangle, float>();
		Dictionary<NavRectangle, NavRectangle> parentsInPath = new Dictionary<NavRectangle, NavRectangle>();

		while (!parentsInPath.ContainsKey(end))
		{
			float priority = openRects.PeekPriority();
			NavRectangle rect = openRects.Pop();
			closedRects.Add(rect, priority);

			foreach (NavRectangle adjoining in rect.GetAdjacentRectangles())
			{
				if (adjoining.Equals(end))
				{
					parentsInPath[end] = rect;
					break;
				}
				if ((linkTypeMask & adjoining.navTerrainType) == 0)
				{
					continue;
				}

				float cost = Heuristic(rect, adjoining) + Heuristic(adjoining, end);
				if (closedRects.ContainsKey(adjoining) && cost < closedRects[adjoining])
				{
					closedRects.Remove(adjoining);
				}
				if (!closedRects.ContainsKey(adjoining))
				{
					if (!openRects.Contains(adjoining))
					{
						openRects.Add(cost, adjoining);
						parentsInPath[adjoining] = rect;
					}
					else if (openRects.LowerPriority(adjoining, cost))
					{
						parentsInPath[adjoining] = rect;
					}
				}
			}
		}

		List<NavRectangle> ret = new List<NavRectangle>();
		AddParentsFirst(parentsInPath, ret, end);
		return ret;
	}

	private void AddParentsFirst(Dictionary<NavRectangle, NavRectangle> parentsInPath, List<NavRectangle> ret, NavRectangle child)
	{
		NavRectangle parent;
		if (parentsInPath.TryGetValue(child, out parent) && parent != null)
		{
			AddParentsFirst(parentsInPath, ret, parent);
		}
		ret.Add(child);
	}

	private float Heuristic(NavRectangle current, NavRectangle dest)
	{
		float dx = current.centerX - dest.centerX;
		float dy = current.centerY - dest.centerY;
		return Mathf.Sqrt(dx * dx + dy * dy);
	}
}
