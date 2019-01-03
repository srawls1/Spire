using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : StayAtHomeAIController
{
	protected override int navLayerMask
	{
		get
		{
			return (int)(NavTerrainTypes.Floor | NavTerrainTypes.Pit);
		}
	}
}
