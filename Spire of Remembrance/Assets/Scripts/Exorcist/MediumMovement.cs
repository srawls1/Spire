using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumMovement : Movement
{
	[SerializeField] TeleportToInteractable teleporterPrefab;
	[SerializeField] private float teleportWindow = 2f;

	protected MediumAnimations animations
	{
		get { return animator as MediumAnimations; }
	}

	protected override int getInteractionLayermask()
	{
		Debug.Log(LayerMask.NameToLayer("Teleporter"));
		return base.getInteractionLayermask() | (1 << LayerMask.NameToLayer("Teleporter"));
	}

	public void TeleportTo(Vector2 destination)
	{
		animations.AnimateTeleport(destination);
	}

	public void MakeTeleportInteractable(AITarget target)
	{
		TeleportToInteractable teleporter = Instantiate(teleporterPrefab, transform.position, Quaternion.identity);
		teleporter.movement = this;
		teleporter.target = target;
		//NewInteractable(teleporter);
		Destroy(teleporter.gameObject, teleportWindow);
	}
}
