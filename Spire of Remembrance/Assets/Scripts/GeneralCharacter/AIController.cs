using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType
{
	Walk,
	Wait,
	Turn
}

[System.Serializable]
public class AIAction
{
	public ActionType type;
	public Vector3 destination;
	public float waitTime;
	public Facing direction;
}

public class AIController : Controller
{
	#region Editor Fields

	[SerializeField] protected float attackRange;
	[SerializeField] protected AIAction[] actions;
	[SerializeField] protected AIPerception perception;
	[SerializeField] protected int perceptionCheckFrequency = 15;
	[SerializeField] protected int pathRecalcFrequency = 60;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private AITarget selfTarget;
	private AITarget m_target;
	private int perceptionCheckFrame;
	private int currentPerceptionCount;
	private int pathRecalcFrame;
	private int currentPathCount;
	private Vector2 homePosition;

	#endregion // Non-Editor Fields

	#region Properties

	public AITarget target
	{
		get
		{
			return m_target;
		}
		set
		{
			if (m_target != null)
			{
				m_target.RemoveAttackingEnemy(gameObject);
			}

			m_target = value;

			if (m_target != null)
			{
				m_target.AddAttackingEnemy(gameObject);
			}
		}
	}

	#endregion // Properties

	#region Unity Functions

	protected void Awake()
	{
		homePosition = transform.position;
		selfTarget = GetComponent<AITarget>();
		selfTarget.alignment = Alignment.Enemy;
		perceptionCheckFrame = Random.Range(0, perceptionCheckFrequency);
		pathRecalcFrame = Random.Range(0, pathRecalcFrequency);
	}

	protected void OnEnable()
	{
		StartCoroutine(FollowRoute());
	}

	protected void OnDisable()
	{
		StopAllCoroutines();
	}

	private void OnDrawGizmosSelected()
	{
		Vector2 currentPos = transform.position;
		for (int i = 0; i < actions.Length; ++i)
		{
			if (actions[i].type == ActionType.Walk)
			{
				Gizmos.DrawLine(currentPos, actions[i].destination);
				currentPos = actions[i].destination;
			}
		}
	}

	#endregion // Unity Functions

	#region Private Functions

	private IEnumerator FollowRoute()
	{
		yield return null;
		yield return NavigateToPoint(homePosition);
		while (true)
		{
			yield return null;
			for (int i = 0; i < actions.Length; ++i)
			{
				yield return StartCoroutine(MakeRequisiteChecks());
				switch (actions[i].type)
				{
					case ActionType.Walk:
						yield return StartCoroutine(NavigateToPoint(actions[i].destination));
						break;
					case ActionType.Turn:
						yield return StartCoroutine(Turn(actions[i].direction));
						break;
					case ActionType.Wait:
						yield return StartCoroutine(WaitForSeconds(actions[i].waitTime));
						break;
				}
			}
		}
	}

	private IEnumerator NavigateToPoint(Vector2 destination)
	{
		List<Vector2> route = null;
		if (NavMesh.instance != null)
		{
			route = NavMesh.instance.GetClosestPath(transform.position, destination, GetNavTerrainMask());
		}

		//for (int i = 1; i < route.Count; ++i)
		//{
		//	Debug.DrawLine(route[i - 1], route[i], Color.white, .5f);
		//}

		if (route == null)
		{
			yield return StartCoroutine(WalkToPoint(destination));
		}
		else
		{
			for (int i = 0; i < route.Count; ++i)
			{
				yield return StartCoroutine(WalkToPoint(route[i]));
			}
		}
	}

	private IEnumerator WalkToPoint(Vector2 destination)
	{
		while (Vector2.Distance(transform.position, destination) > 0.2f)
		{
			yield return StartCoroutine(MakeRequisiteChecks());
			Vector2 pos = transform.position;
			Vector2 disp = destination - pos;
			controlledMovement.Walk(disp.normalized);
			yield return null;
		}
	}

	private IEnumerator Turn(Facing direction)
	{
		yield return StartCoroutine(MakeRequisiteChecks());
		controlledMovement.Walk(GetVectorForDirection(direction) * 0.2f);
		yield break;
	}

	private Vector2 GetVectorForDirection(Facing direction)
	{
		switch (direction)
		{
			case Facing.right: return Vector2.right;
			case Facing.up: return Vector2.up;
			case Facing.left: return Vector2.left;
			case Facing.down: return Vector2.down;
			default: return Vector2.zero;
		}
	}

	private IEnumerator WaitForSeconds(float waitTime)
	{
		float timePassed = 0f;
		while (timePassed < waitTime)
		{
			yield return StartCoroutine(MakeRequisiteChecks());
			timePassed += Time.deltaTime;
			controlledMovement.Walk(Vector2.zero);
			yield return null;
		}
	}


	private IEnumerator MakeRequisiteChecks()
	{
		if (target == null && currentPerceptionCount++ == perceptionCheckFrame)
		{
			List<AITarget> targetsInView = perception.GetSeenTargets(transform.position,
				GetVectorForDirection(controlledMovement.Facing));

			float minDistance = Mathf.Infinity;
			int minIndex = -1;
			for (int i = 0; i < targetsInView.Count; ++i)
			{
				float distance;
				if (ShouldAttack(targetsInView[i]) &&
					(distance = Vector2.Distance(targetsInView[i].transform.position, transform.position)) < minDistance)
				{
					minDistance = distance;
					minIndex = i;
				}
			}

			if (minIndex >= 0)
			{
				target = targetsInView[minIndex];
			}
		}
		currentPerceptionCount %= perceptionCheckFrequency;

		if (target != null)
		{
			yield return StartCoroutine(PursueTarget());
		}
	}

	private IEnumerator PursueTarget()
	{
		while (target != null)
		{
			currentPathCount %= pathRecalcFrequency;
			float targettingAngle = target.GetTargetingAngle(this);
			Vector2 destination = target.transform.position;
			Vector2 disp = new Vector2(Mathf.Cos(targettingAngle) * (attackRange - 0.5f),
				Mathf.Sin(targettingAngle) * (attackRange - 0.5f));
			RaycastHit2D hit = Physics2D.Raycast(destination, disp, attackRange - 0.5f,
				Physics2D.GetLayerCollisionMask(gameObject.layer));
			if (hit.transform != null)
			{
				disp *= hit.distance / disp.magnitude;
			}
			destination += disp;
			List<Vector2> route = null;
			if (NavMesh.instance != null)
			{
				route = NavMesh.instance.GetClosestPath(transform.position, destination, GetNavTerrainMask());
			}

			if (route == null || route.Count == 0)
			{
				//target = null;
				while (Vector2.Distance(transform.position, destination) > 0.5f)
				{
					++currentPathCount;
					currentPathCount %= pathRecalcFrequency;
					if (currentPathCount == pathRecalcFrame)
					{
						yield break;
					}

					Vector2 position = transform.position;
					Vector2 dir = destination - position;
					controlledMovement.Walk(dir.normalized);
					yield return null;
				}
			}
			else
			{
				for (int i = 0; i < route.Count; ++i)
				{
					while (Vector2.Distance(transform.position, route[i]) > 0.5f)
					{
						++currentPathCount;
						currentPathCount %= pathRecalcFrequency;
						if (currentPathCount == pathRecalcFrequency)
						{
							yield break;
						}

						Vector2 position = transform.position;
						Vector2 dir = route[i] - position;
						controlledMovement.Walk(dir.normalized);
						yield return null;
					}
				}
			}

			if (target != null && Vector2.Distance(transform.position, target.transform.position) <= attackRange + 0.5f)
			{
				Attack();
			}
			yield return null;
		}
	}

	private void Attack()
	{
		// First face the target
		Vector2 dir = target.transform.position - transform.position;
		controlledMovement.Walk(dir.normalized * 0.2f);
		// Then attack
		controlledMovement.Attack();
	}

	protected virtual bool ShouldAttack(AITarget other)
	{
		return AITarget.FactionsHostile(selfTarget.alignment, other.alignment) &&
			(selfTarget.alignment != Alignment.Wildcard ||
			selfTarget.attackingEnemies.Contains(other.gameObject));
	}

	protected virtual NavTerrainTypes GetNavTerrainMask()
	{
		return NavTerrainTypes.Floor | NavTerrainTypes.Door;
	}

	#endregion // Private Functions
}
