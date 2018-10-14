using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
	[SerializeField] protected float attackRange;

	private Damageable m_target;

	public Damageable target
	{
		get
		{
			return m_target;
		}
		set
		{
			m_target = value;
			// TODO calculate path
		}
	}


	protected void Update()
	{
		if (target == null)
		{
			controlledMovement.Walk(Vector2.zero);
		}
		else
		{
			// TODO follow path toward target
			controlledMovement.Walk((target.transform.position - transform.position).normalized);

			if (Vector3.Distance(target.transform.position, transform.position) < attackRange)
			{
				controlledMovement.Attack();
			}
		}
	}
}
