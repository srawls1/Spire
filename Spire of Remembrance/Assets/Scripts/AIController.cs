using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
	[SerializeField] protected float attackRange;

	private Damageable m_target;
	private List<Vector2> path;
	private Coroutine recalcPath;

	public Damageable target
	{
		get
		{
			return m_target;
		}
		set
		{
			if (value != m_target)
			{
				m_target = value;
				if (m_target != null)
				{
					path = NavMesh.instance.GetClosestPath(transform.position, target.transform.position, GetNavTerrainMask());
					recalcPath = StartCoroutine(PeriodicallyRecalculatePath());
				}
				else
				{
					path = null;
					StopCoroutine(recalcPath);
					recalcPath = null;
				}
			}
		}
	}

	private IEnumerator PeriodicallyRecalculatePath()
	{
		while (true)
		{
			Debug.Log(gameObject.name + ": " + path.Count + " points");
			float waitTime = Random.Range(0.3f, 0.5f) * path.Count;
			yield return new WaitForSeconds(waitTime);
			path = NavMesh.instance.GetClosestPath(transform.position, target.transform.position, GetNavTerrainMask());
		}
	}

	protected void Update()
	{
		if (path != null)
		{
			Vector2 location = transform.position;
			for (int i = 0; i < path.Count; ++i)
			{
				Debug.DrawLine(location, path[i]);
				location = path[i];
			}
		}
		if (target == null)
		{
			controlledMovement.Walk(Vector2.zero);
		}
		else
		{
			if (path.Count == 1)
			{
				if (Vector3.Distance(target.transform.position, transform.position) < attackRange)
				{
					controlledMovement.Attack();
				}
				else
				{
					controlledMovement.Walk((target.transform.position - transform.position).normalized);
				}
			}
			else
			{
				if (Vector2.Distance(transform.position, path[0]) < 0.1f)
				{
					path.RemoveAt(0);
				}
				Vector2 input = path[0] - new Vector2(transform.position.x, transform.position.y);
				input.Normalize();
				controlledMovement.Walk(input);
			}
		}
	}

	protected virtual int GetNavTerrainMask()
	{
		return (int)(NavTerrainTypes.Floor | NavTerrainTypes.Door);
	}
}
