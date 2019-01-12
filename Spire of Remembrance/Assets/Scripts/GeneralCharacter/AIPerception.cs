using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIPerception
{
	[SerializeField] private float sightRange;
	[SerializeField] private float coneAngularWidth;

	private static List<AITarget> targets;

	static AIPerception()
	{
		targets = new List<AITarget>();
	}
	
	public static void RegisterTarget(AITarget target)
	{
		targets.Add(target);
	}

	public static void UnregisterTarget(AITarget target)
	{
		targets.Remove(target);
	}

	public List<AITarget> GetSeenTargets(Vector3 position, Vector2 forward)
	{
		float effectiveSightRange = sightRange * LightLevel.GetLightLevel(position);
		List<AITarget> seenTargets = new List<AITarget>();
		for (int i = 0; i < targets.Count; ++i)
		{
			// TODO - eventually change this to use effective sight range instead
			Vector2 selfToTarget = targets[i].transform.position - position;
			if (selfToTarget.magnitude < sightRange &&
				Mathf.Abs(Vector2.Angle(forward, selfToTarget)) < 90 &&
				Physics2D.Raycast(position, selfToTarget).transform == targets[i].transform)
			{
				seenTargets.Add(targets[i]);
			}
		}

		return seenTargets;
	}
}
