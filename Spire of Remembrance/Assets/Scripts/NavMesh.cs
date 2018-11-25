using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
				if (m_instance == null)
				{
					GameObject obj = new GameObject();
					m_instance = obj.AddComponent<NavMesh>();
				}
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

	private BspNode root;

	void Start()
	{
		BuildMesh();
	}

	private void BuildMesh()
	{
		root = new BspNode(minX, minY, maxX, maxY, maxRectsPerCell);
		for (int i = 0; i < rects.Length; ++i)
		{
			root.AddRect(rects[i]);
		}
	}

	public void GetClosestPath(Vector2 start, Vector2 end, int linkTypeMask)
	{
		throw new NotImplementedException();
	}
}
