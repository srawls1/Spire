﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Pickup
{
	protected override void PerformPickupAction()
	{
		CharacterController.instance.gainKey();
		base.PerformPickupAction();
	}
}
