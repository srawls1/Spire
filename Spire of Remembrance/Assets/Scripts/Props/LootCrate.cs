using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnFrequencyEntry
{
	public GameObject obj;
	public float frequency;
}

public class LootCrate : Damageable
{
	[SerializeField] private SpawnFrequencyEntry[] lootTable;
	[SerializeField] private float durability;
	[SerializeField] private float destroyAnimDuration;
	[SerializeField] private float courseCorrection;

	public override void TakeDamage(float damage, Vector3 damagerPosition, float force)
	{
		durability -= damage;
		if (durability <= 0)
		{
			// TODO - trigger destruction animation
			StartCoroutine(destructionRoutine());
		}
	}

	private IEnumerator destructionRoutine()
	{
		yield return new WaitForSeconds(destroyAnimDuration);

		float min = 0f;
		float max = 1f;
		for (int i = 0; i < lootTable.Length; ++i)
		{
			if (Random.Range(min, max) <= lootTable[i].frequency)
			{
				Instantiate(lootTable[i].obj, transform.position, transform.rotation);
				min += courseCorrection;
			}
			else
			{
				max -= courseCorrection;
			}
		}

		Destroy(gameObject);
	}
}
