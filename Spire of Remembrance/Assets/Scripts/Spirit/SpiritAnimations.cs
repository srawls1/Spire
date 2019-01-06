using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritAnimations : EntityAnimations
{
	[SerializeField] private float timeToRewindOnFall;
	[SerializeField] private int damageOnFall;

	private Queue<Pair<float, Vector2>> previousPositions;
	private EnemyHealth health;

	new protected void Awake()
	{
		base.Awake();
		previousPositions = new Queue<Pair<float, Vector2>>();
		health = GetComponent<EnemyHealth>();
	}

	private void Update()
	{
		previousPositions.Enqueue(new Pair<float, Vector2>
		{
			first = Time.time,
			second = transform.position
		});

		float headTime = previousPositions.Peek().first;
		if (headTime < Time.time - timeToRewindOnFall)
		{
			previousPositions.Dequeue();
		}
	}

	public override void FallInPit()
	{
		StartCoroutine(FallInPitRoutine());
	}

	private IEnumerator FallInPitRoutine()
	{
		movement.enabled = false;
		Vector2 resetPotion = previousPositions.Peek().second;
		PlayAnimation(Animations.FallInPit);
		yield return new WaitForSeconds(fallAnimDuration);

		health.TakeDamage(damageOnFall, transform.position, 0f);
		currentState = Animations.IdleDown;
		transform.position = resetPotion;
		movement.enabled = true;
	}
}
