using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritOrbPickup : Pickup
{
	protected override void PerformPickupAction()
	{
		CharacterController.instance.gainSpiritOrb();
		base.PerformPickupAction();
	}
}
