using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAtHomeAIController : Controller
{
	#region Editor Fields

	[SerializeField] private float maxWanderDistance;
	[SerializeField] private float outsideWaitTime;
	[SerializeField] private float travelDistancePerBout;
	[SerializeField] private float idleTimeBetweenBouts;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private Vector2 homePosition;

	#endregion // Non-Editor Fields

	#region Properties

	protected virtual int navLayerMask
	{
		get
		{
			return (int)(NavTerrainTypes.Floor | NavTerrainTypes.Door);
		}
	}

	#endregion // Properties

	#region Unity Functions

	protected void Start()
	{
		homePosition = transform.position;
	}

	private void OnEnable()
	{
		StartCoroutine(Wander());
	}

	#endregion // Unity Functions

	#region Private Functions

	private IEnumerator Wander()
	{
		while (enabled)
		{
			yield return null;
			if (Vector2.Distance(transform.position, homePosition) > maxWanderDistance)
			{
				yield return StartCoroutine(Idle(outsideWaitTime));
				Vector2 homeDest = homePosition + Random.insideUnitCircle * maxWanderDistance;
				yield return StartCoroutine(NavigateToPoint(homeDest));
			}
			Vector2 currentPosition = transform.position;
			float theta = Random.Range(0, 2 * Mathf.PI);
			Vector2 displacement = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)) * travelDistancePerBout;
			Vector2 dest = currentPosition + displacement;
			float distFromHome = Vector2.Distance(dest, homePosition);
			if (distFromHome > maxWanderDistance)
			{
				dest += (homePosition - dest).normalized * (distFromHome - maxWanderDistance);
			}
			yield return StartCoroutine(MoveToPoint(dest));

			yield return StartCoroutine(Idle(idleTimeBetweenBouts));
		}
	}

	private IEnumerator NavigateToPoint(Vector2 homeDest)
	{
		List<Vector2> path = null;
		if (NavMesh.instance != null)
		{
			path = NavMesh.instance.GetClosestPath(transform.position, homeDest, navLayerMask);
		}

		if (path == null)
		{
			yield return StartCoroutine(MoveToPoint(homeDest));
			yield break;
		}

		for (int i = 0; i < path.Count; ++i)
		{
			if (controlledMovement.CurrentController != this)
			{
				break;
			}
			yield return StartCoroutine(MoveToPoint(path[i], Mathf.Infinity));
		}
	}

	private IEnumerator Idle(float idleTime)
	{
		for (float timePassed = 0f; timePassed < idleTime; timePassed += Time.deltaTime)
		{
			if (controlledMovement.CurrentController != this)
			{
				break;
			}
			controlledMovement.Walk(Vector2.zero);
			yield return null;
		}
	}

	private IEnumerator MoveToPoint(Vector2 dest, float timeout = 10f)
	{
		float timePassed = 0f;
		while (timePassed < timeout && Vector2.Distance(transform.position, dest) > .5f)
		{
			if (controlledMovement.CurrentController != this)
			{
				break;
			}
			timePassed += Time.deltaTime;
			Vector2 currentPos = transform.position;
			controlledMovement.Walk((dest - currentPos).normalized);
			yield return null;
		}
	}

	#endregion // Helper Functions
}
