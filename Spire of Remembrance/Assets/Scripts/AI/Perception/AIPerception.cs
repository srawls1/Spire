using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIPerception : MonoBehaviour
{
	#region Static Variables

	private static bool m_visualize;
	private static event Action<bool> OnVisualizeChanged;

	#endregion // Static Variables

	#region Editor Fields

	[SerializeField] private float coneAngularWidth;
	[SerializeField] private AnimationCurve sightRangeByLightLevel;
	[SerializeField] private UnityEvent<AITarget> m_onTargetEnteredSight;
	[SerializeField] private UnityEvent<AITarget> m_onTargetStayInSight;
	[SerializeField] private UnityEvent<AITarget> m_onTargetOutOfSight;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private MeshRenderer visionCone;
	private MeshFilter coneFilter;
	private List<AITarget> inRangeTargets;
	private HashSet<AITarget> inViewTargets;
	private OrderedGeometryList obstructionGeometry;
	private UpdatePolicy m_updatePolicy;
	private bool dirty;
	private Vector2 lastUpdatePosition;
	private float sightRange;
	private List<Vector2> viewPolygon;

	#endregion // Non-Editor Fields

	#region Static Properties

	private static bool visualize
	{
		get
		{
			return m_visualize;
		}
		set
		{
			m_visualize = value;
			if (OnVisualizeChanged != null)
			{
				OnVisualizeChanged(value);
			}
		}
	}

	#endregion // Static Properties

	#region Properties

	public UnityEvent<AITarget> onTargetEnteredSight { get; }
	public UnityEvent<AITarget> onTargetStayInSight { get; }
	public UnityEvent<AITarget> onTargetOutOfSight { get; }

	public UpdatePolicy updatePolicy
	{
		private get { return m_updatePolicy; }
		set { m_updatePolicy = value; }
	}

	public Vector2 facing
	{
		get; set;
	}

	#endregion // Properties

	#region Unity API

	private void Awake()
	{
		OnVisualizeChanged += VisualizeChanged;
		inRangeTargets = new List<AITarget>();
		inViewTargets = new HashSet<AITarget>();
		visionCone = GetComponent<MeshRenderer>();
		coneFilter = GetComponent<MeshFilter>();
		obstructionGeometry = new OrderedGeometryList(32);
		lastUpdatePosition = transform.position;
		viewPolygon = new List<Vector2>();
		UpdateSightPoly();
	}

	private void Update()
	{
		updatePolicy.Tick(Time.deltaTime);
		if (updatePolicy.ShouldUpdate())
		{
			float lightLevel = LightLevel.GetLightLevel(transform.position);
			sightRange = sightRangeByLightLevel.Evaluate(lightLevel);
			transform.localScale = new Vector2(sightRange, sightRange);
			CheckDidSomethingMove();
			if (dirty)
			{
				UpdateSightPoly();
			}

			CheckTargetsInSight();
		}
	}

	private void CheckTargetsInSight()
	{
		for (int i = 0; i < inRangeTargets.Count; ++i)
		{
			if (IsInSight(inRangeTargets[i]))
			{
				if (inViewTargets.Contains(inRangeTargets[i]))
				{
					onTargetStayInSight.Invoke(inRangeTargets[i]);
				}
				else
				{
					onTargetEnteredSight.Invoke(inRangeTargets[i]);
					inViewTargets.Add(inRangeTargets[i]);
				}
			}
			else if (inViewTargets.Contains(inRangeTargets[i]))
			{
				onTargetOutOfSight.Invoke(inRangeTargets[i]);
				inViewTargets.Remove(inRangeTargets[i]);
			}
		}
	}

	private bool IsInSight(AITarget target)
	{
		Vector2 disp = target.transform.position - transform.position;
		float angle = Mathf.Atan2(disp.y, disp.x);

		int start = 0;
		int end = viewPolygon.Count;

		float minAngle = Mathf.Atan2(viewPolygon[start].y, viewPolygon[start].x);
		float maxAngle = Mathf.Atan2(viewPolygon[end].y, viewPolygon[end].x);
		if (angle < minAngle || angle > maxAngle)
		{
			return false;
		}

		while (start < end)
		{
			int mid = (start + end) / 2;
			float vertAngle = Mathf.Atan2(viewPolygon[mid].y, viewPolygon[mid].x);
			if (angle < vertAngle)
			{
				end = mid;
			}
			else
			{
				start = mid + 1;
			}
		}

		Vector3 betweenVerts = viewPolygon[start] - viewPolygon[start - 1];
		Vector3 vertToTarget = (Vector2)target.transform.position - viewPolygon[start];
		return Vector3.Cross(betweenVerts, vertToTarget).z < 0;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		AITarget target = other.GetComponent<AITarget>();
		if (target != null)
		{
			inRangeTargets.Add(target);
			dirty = true;
		}

		PerceptionObstacle obstruction = other.GetComponent<PerceptionObstacle>();
		if (obstruction != null)
		{
			obstructionGeometry.AddGeometry(obstruction.Bounds);
			dirty = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		AITarget target = other.GetComponent<AITarget>();
		if (target != null)
		{
			int index = inRangeTargets.IndexOf(target);
			if (index >= 0)
			{
				inRangeTargets.RemoveAt(index);
				dirty = true;
			}
		}

		PerceptionObstacle obstruction = other.GetComponent<PerceptionObstacle>();
		if (obstruction != null)
		{
			obstructionGeometry.RemoveGeometry(obstruction.Bounds);
			dirty = true;
		}
	}

	#endregion // Unity API

	private void CheckDidSomethingMove()
	{
		if (!Mathf.Approximately(0f, Vector2.Distance(transform.position, lastUpdatePosition)))
		{
			dirty = true;
			lastUpdatePosition = transform.position;
		}

		if (obstructionGeometry.isDirty)
		{
			dirty = true;
		}
	}

	private void VisualizeChanged(bool visualize)
	{
		visionCone.enabled = visualize;
	}

	private void UpdateSightPoly()
	{
		viewPolygon.Clear();
		obstructionGeometry.AngularSort(transform.position);

		float angle = Mathf.Atan2(facing.y, facing.x) - coneAngularWidth / 2;
		float endAngle = angle + coneAngularWidth;
		int i = obstructionGeometry.GetVertexIndex(angle);

		const int NUM_ARC_SEGMENTS = 10;
		float angleStep = coneAngularWidth / NUM_ARC_SEGMENTS;

		while (angle < endAngle)
		{
			viewPolygon.Add(obstructionGeometry.GetClosestPointBefore(transform.position, i, angle, sightRange));

			angle += angleStep;
		 	while (angle > obstructionGeometry.GetAngle(i))
			{
				Vector2 before = obstructionGeometry.GetClosestPointBefore(transform.position, i, obstructionGeometry.GetAngle(i), sightRange);
				Vector2 after = obstructionGeometry.GetClosestPointAfter(transform.position, i, obstructionGeometry.GetAngle(i), sightRange);
				viewPolygon.Add(before);

				if (!Mathf.Approximately(0f, Vector2.Distance(before, after)))
				{
					viewPolygon.Add(after);
				}
				i = (i + 1) % obstructionGeometry.vertexCount;
			}
		}

		UpdateMesh();
	}

	private void UpdateMesh()
	{
		Vector3[] vertices = new Vector3[viewPolygon.Count + 1];
		int[] triangles = new int[(viewPolygon.Count - 1) * 3];
		int triCount = 0;

		vertices[0] = transform.position;
		vertices[1] = viewPolygon[0];
		for (int i = 1; i < viewPolygon.Count; ++i)
		{
			vertices[i + 1] = viewPolygon[i];
			triangles[triCount++] = 0;
			triangles[triCount++] = i + 1;
			triangles[triCount++] = i;
		}

		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		coneFilter.mesh = mesh;
	}
}
