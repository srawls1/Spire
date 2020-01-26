using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public enum Alignment
{
	Player,
	Enemy,
	Wildcard
}

public class AITarget : MonoBehaviour
{
	private Alignment prevAlignment;
	private Alignment m_alignment;
	public Alignment alignment
	{
		get
		{
			return m_alignment;
		}
		set
		{
			prevAlignment = m_alignment;
			m_alignment = value;
		}
	}

	private List<GameObject> m_attackingEnemies;
	public ReadOnlyCollection<GameObject> attackingEnemies
	{
		get
		{
			return m_attackingEnemies.AsReadOnly();
		}
	}

	public static bool FactionsHostile(Alignment alignment1, Alignment alignment2)
	{
		return alignment1 != alignment2 || alignment1 == Alignment.Wildcard;
	}

	private void Awake()
	{
		m_attackingEnemies = new List<GameObject>();
	}

	public void AddAttackingEnemy(GameObject attacker)
	{
		m_attackingEnemies.Add(attacker);
	}

	public void RemoveAttackingEnemy(GameObject attacker)
	{
		m_attackingEnemies.Remove(attacker);
	}

	public float GetTargetingAngle(AIController attacker)
	{
		AngularSort(m_attackingEnemies);

		int index = m_attackingEnemies.IndexOf(attacker.gameObject);
		if (index < 0)
		{
			return CalculateAngle(attacker.transform);
		}

		float actualAngle = CalculateAngle(m_attackingEnemies[0].transform) +
			2 * index * Mathf.PI / m_attackingEnemies.Count;
		return Mathf.Round(actualAngle / (Mathf.PI / 2)) * Mathf.PI / 2;
	}

	private float CalculateAngle(Transform otherTrans)
	{
		Vector2 disp = otherTrans.position - transform.position;
		return Mathf.Atan2(disp.y, disp.x);
	}

	private void AngularSort(List<GameObject> enemies)
	{
		List<float> angles = new List<float>(enemies.Count);
		float minDistance = Mathf.Infinity;
		int minIndex = -1;
		for (int i = 0; i < enemies.Count; ++i)
		{
			if (enemies[i] == null)
			{
				enemies.RemoveAt(i);
				--i;
			}
			angles.Add(CalculateAngle(enemies[i].transform));
			float distance = Vector2.Distance(enemies[i].transform.position, transform.position);
			if (distance < minDistance)
			{
				minDistance = distance;
				minIndex = i;
			}
		}

		if (minIndex == -1)
		{
			return;
		}

		float closestAngle = angles[minIndex];
		for (int i = 0; i < angles.Count; ++i)
		{
			angles[i] = angles[i] - closestAngle;
			angles[i] += angles[i] < 0f ? 360f : 0f;
		}

		for (int i = 1; i < enemies.Count; ++i)
		{
			float tempAngle = angles[i];
			GameObject tempEnemy = enemies[i];
			int j;
			for (j = i; j > 0; --j)
			{
				if (tempAngle < angles[j - 1])
				{
					angles[j] = angles[j - 1];
					enemies[j] = enemies[j - 1];
				}
				else
				{
					break;
				}
			}
			angles[j] = tempAngle;
			enemies[j] = tempEnemy;
		}
	}

	public void ResetAlignmentFrom(Alignment a)
	{
		if (a == alignment)
		{
			alignment = prevAlignment;
		}
	}
}
