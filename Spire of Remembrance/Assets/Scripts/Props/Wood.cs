using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
	[SerializeField] private float destroyAfterBurningTime;
	[SerializeField] private float burningSpreadPeriod;
	[SerializeField] private float burningSpreadRadius;

	private float spreadingBurnDuration;
	private float spreadingBurnDps;

	public void OnFireDamage(FireDamageArgs args)
	{
		spreadingBurnDuration = args.duration;
		spreadingBurnDps = args.dps;
		StartCoroutine(BurnRoutine());
	}

	private IEnumerator BurnRoutine()
	{
		// TODO - fire particles
		FireDamageArgs args = new FireDamageArgs(spreadingBurnDuration, spreadingBurnDps);

		float timePassed = 0f;
		while (true)
		{
			yield return new WaitForSeconds(burningSpreadPeriod);
			timePassed += burningSpreadPeriod;
			if (timePassed >= destroyAfterBurningTime)
			{
				break;
			}

			Collider2D[] spreadTargets = Physics2D.OverlapCircleAll(transform.position,
				burningSpreadRadius, Physics2D.GetLayerCollisionMask(gameObject.layer));
			for (int i = 0; i < spreadTargets.Length; ++i)
			{
				spreadTargets[i].gameObject.SendMessage("OnFireDamage", args, SendMessageOptions.DontRequireReceiver);
			}
		}

		Destroy(gameObject);
	}
}
