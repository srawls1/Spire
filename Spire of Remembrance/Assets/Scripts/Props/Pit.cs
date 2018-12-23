using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
	[SerializeField] private float fallMoveSmoothing;

	protected void OnTriggerEnter2D(Collider2D collision)
	{
		Movement movement = collision.GetComponent<Movement>();
		if (movement == null)
		{
			return;
		}

		Controller controller = movement.CurrentController;
		if (controller is CharacterController)
		{
			CharacterController character = controller as CharacterController;
			character.Deposess();
			// TODO - do some damage to the spirit health, reposition outside of the pit
		}

		EntityAnimations animations = collision.GetComponent<EntityAnimations>();
		animations.FallInPit();

		Damageable damageable = collision.GetComponent<Damageable>();
		if (damageable != null)
		{
			damageable.TakeDamage(int.MaxValue, transform.position, 0f);
		}

		StartCoroutine(FallTowardCenter(collision.transform));
	}

	private IEnumerator FallTowardCenter(Transform trans)
	{
		while (trans)
		{
			trans.position = Vector3.Lerp(trans.position, transform.position, Time.deltaTime * fallMoveSmoothing);
			yield return null;
		}
	}
}
