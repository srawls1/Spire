using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatController : StayAtHomeAIController
{
	protected override int navLayerMask
	{
		get
		{
			return (int)(NavTerrainTypes.Floor | NavTerrainTypes.RatTunnel);
		}
	}
}
