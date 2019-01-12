using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : Movement
{
	#region Editor Fields

	[SerializeField] private float possessFadeDuration;
	[SerializeField] private HealthBar possessedEnemyHealthBar;
	[SerializeField] private int physicalLayer;
	[SerializeField] private Color  physicalColor;
	[SerializeField] private float physicalDuration;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private SpiritHealth health;
	private Color baseColor;
	private Bounds baseScale;
	private int baseLayer;

	#endregion // Non-Editor Fields

	#region Public Functions

	public Coroutine Possess(GameObject interactable)
	{
		Movement movement = interactable != null ? interactable.GetComponentInParent<Movement>() : null;
		if (movement == null)
		{
			return StartCoroutine(dummyCoroutine());
		}

		EnemyHealth bodyHealth = movement.GetComponent<EnemyHealth>();
		possessedEnemyHealthBar.target = bodyHealth;
		health.possessedBody = bodyHealth;
		return StartCoroutine(possessRoutine(movement));
	}

	public Coroutine Depossess()
	{
		if (transform.parent != null)
		{
			if (transform.parent.GetComponent<Movement>() != null)
			{
				GameObject possessed = transform.parent.gameObject;
				possessedEnemyHealthBar.target = null;
				health.possessedBody = null;
				return StartCoroutine(FadeOutOf(possessed));
			}
		}

		return StartCoroutine(dummyCoroutine());
	}

	public void TurnPhysical()
	{
		if (gameObject.layer == physicalLayer)
		{
			return;
		}

		baseLayer = gameObject.layer;
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		baseColor = renderer.color;

		gameObject.layer = physicalLayer;
		renderer.color = physicalColor;
		StartCoroutine(TurnBackToSpiritTimer());
	}

	#endregion // Public Functions

	#region Unity Functions

	protected new void Awake()
	{
		base.Awake();
		collider = GetComponent<Collider2D>();
		health = GetComponent<SpiritHealth>();
	}

	#endregion // Unity Functions

	#region Override Functions

	protected override int getInteractionLayermask()
	{
		if (gameObject.layer == physicalLayer)
		{
			return base.getInteractionLayermask();
		}
		else
		{
			return LayerMask.GetMask(new string[]
			{
				"Spirit"
			});
		}
	}

	#endregion // Override Functions

	#region Helper Functions

	private IEnumerator dummyCoroutine()
	{
		yield break;
	}

	private IEnumerator TurnBackToSpiritTimer()
	{
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		yield return new WaitForSeconds(physicalDuration);
		renderer.color = baseColor;
		gameObject.layer = baseLayer;
	}

	private IEnumerator possessRoutine(Movement movement)
	{
		if (movement == null)
		{
			yield break;
		}

		rigidBody.velocity = Vector2.zero;
		CurrentController.enabled = false;
		transform.parent = movement.transform;

		yield return FadeInto(movement.gameObject);

		CurrentController.Possess(movement);
		CurrentController.enabled = true;
	}

	private Coroutine FadeInto(GameObject obj)
	{
		rigidBody.bodyType = RigidbodyType2D.Kinematic;
		collider.enabled = false;
		return StartCoroutine(FadeIntoRoutine(obj));
	}

	private IEnumerator FadeOutOf(GameObject obj)
	{
		transform.parent = null;
		yield return StartCoroutine(FadeOutOfRoutine(obj));
		collider.enabled = true;
		rigidBody.bodyType = RigidbodyType2D.Dynamic;
	}

	private IEnumerator FadeIntoRoutine(GameObject obj)
	{
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();

		Vector3 startPosition = transform.localPosition;
		Vector3 startScale = transform.localScale;
		Color startColor = renderer.color;
		
		Vector3 endPosition = Vector3.zero;
		Bounds objBounds = obj.GetComponent<SpriteRenderer>().bounds;
		Bounds myBounds = renderer.bounds;
		Vector3 endScale = new Vector3(transform.localScale.x * objBounds.extents.x / myBounds.extents.x,
									transform.localScale.y * objBounds.extents.y / myBounds.extents.y,
									transform.localScale.z);
		Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0.0f);

		baseColor = startColor;
		baseScale = myBounds;

		float timePassed = 0f;
		while (timePassed < 1f)
		{
			timePassed += Time.deltaTime / possessFadeDuration;
			transform.localPosition = Vector3.Lerp(startPosition, endPosition, timePassed);
			transform.localScale = Vector3.Lerp(startScale, endScale, timePassed);
			renderer.color = Color.Lerp(startColor, endColor, timePassed);
			yield return null;
		}

		transform.localPosition = endPosition;
		transform.localScale = endScale;
		renderer.color = endColor;
		yield break;
	}

	private IEnumerator FadeOutOfRoutine(GameObject obj)
	{
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();

		Vector3 startScale = transform.localScale;
		Color startColor = renderer.color;

		Bounds myBounds = renderer.bounds;
		Vector3 endScale = new Vector3(transform.localScale.x * baseScale.extents.x / myBounds.extents.x,
									transform.localScale.y * baseScale.extents.y / myBounds.extents.y,
									transform.localScale.z);
		Color endColor = baseColor;

		float timePassed = 0f;
		while (timePassed < 1f)
		{
			timePassed += Time.deltaTime / possessFadeDuration;
			transform.localScale = Vector3.Lerp(startScale, endScale, timePassed);
			renderer.color = Color.Lerp(startColor, endColor, timePassed);
			yield return null;
		}

		transform.localScale = endScale;
		renderer.color = endColor;
		yield break;
	}

	#endregion // Helper Functions
}
