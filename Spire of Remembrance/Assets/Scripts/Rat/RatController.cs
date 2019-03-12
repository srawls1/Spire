using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatController : StayAtHomeAIController
{
	protected override NavTerrainTypes navLayerMask
	{
		get
		{
			return NavTerrainTypes.Floor | NavTerrainTypes.RatTunnel;
		}
	}
}
