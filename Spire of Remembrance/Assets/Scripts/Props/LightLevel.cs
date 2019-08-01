using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightSource
{
	public Vector2 center { get; set; }
	public float intensity { get; set; }
	public AnimationCurve falloff { get; set; }
	public float radius { get; set; }
}

public static class LightLevel
{
	private static List<LightSource> sources;
	private static HashSet<int> holes;

	public static float ambientLightLevel { get; set; }

	static LightLevel()
	{
		sources = new List<LightSource>();
		holes = new HashSet<int>();
		ambientLightLevel = 0.5f;
	}

	public static float GetLightLevel(Vector2 position)
	{
		float lightLevel = ambientLightLevel;

		for (int i = 0; i < sources.Count; ++i)
		{
			if (holes.Contains(i))
			{
				continue;
			}

			float distance = Vector2.Distance(position, sources[i].center) / sources[i].radius;
			if (distance < 1)
			{
				float light = sources[i].intensity * sources[i].falloff.Evaluate(distance);
				lightLevel += light;
			}
		}

		return lightLevel;
	}

	public static int RegisterLightSource(Vector2 position, float intensity, AnimationCurve distanceFalloff, float maxDistance)
	{
		LightSource source = new LightSource()
		{
			center = position,
			intensity = intensity,
			falloff = distanceFalloff,
			radius = maxDistance
		};

		if (holes.Count > 0)
		{
			int index = holes.First();
			holes.Remove(index);
			sources[index] = source;
			return index;
		}

		sources.Add(source);
		return holes.Count - 1;
	}

	public static void UnregisterLightSource(int id)
	{
		sources[id] = null;
		holes.Add(id);
	}
}
