using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatController : AIController {

	protected override int GetNavTerrainMask()
	{
		return (int)(NavTerrainTypes.Floor | NavTerrainTypes.RatTunnel);
	}
}
