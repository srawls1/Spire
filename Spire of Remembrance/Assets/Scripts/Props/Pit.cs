using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
	[SerializeField] private float fallMoveSmoothing;

	new private Collider2D collider;

	protected void Awake()
	{
		collider = GetComponent<Collider2D>();
	}

	protected void OnTriggerEnter2D(Collider2D collision)
	{
		Movement movement = collision.GetComponent<Movement>();
		if (movement == null)
		{
			return;
		}

		Controller controller = movement.CurrentController;
		if (controller is CharacterController && !(movement is CharacterMovement))
		{
			CharacterController character = controller as CharacterController;
			character.Deposess();
		}

		EntityAnimations animations = collision.GetComponent<EntityAnimations>();
		animations.FallInPit();

		//Damageable damageable = collision.GetComponent<Damageable>();
		//if (damageable != null)
		//{
		//	damageable.TakeDamage(int.MaxValue, transform.position, 0f);
		//}

		StartCoroutine(FallTowardCenter(collision.transform, collision));
	}

	private IEnumerator FallTowardCenter(Transform trans, Collider2D other)
	{
		while (collider.IsTouching(other))
		{
			trans.position = Vector3.Lerp(trans.position, transform.position, Time.deltaTime * fallMoveSmoothing);
			yield return null;
		}
	}
}
