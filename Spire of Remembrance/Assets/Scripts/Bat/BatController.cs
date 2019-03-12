using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : StayAtHomeAIController
{
	protected override NavTerrainTypes navLayerMask
	{
		get
		{
			return NavTerrainTypes.Floor | NavTerrainTypes.Pit;
		}
	}
}
