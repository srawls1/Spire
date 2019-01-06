using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZoneActivationMode
{
	Permanent,
	Stay,
	Timed
}

public class EnterZoneActivator : MonoBehaviour
{
	[SerializeField] private ZoneActivationMode mode;
	[SerializeField] private float timeLimit;

	private Activator activator;
	private bool activated;

	private void Awake()
	{
		activator = GetComponent<Activator>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!activated)
		{
			activated = true;
			activator.Activate();
			if (mode == ZoneActivationMode.Timed)
			{
				StartCoroutine(DeactivateAfterTimeLimit());
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (mode == ZoneActivationMode.Stay && activated)
		{
			activated = false;
			activator.Deactivate();
		}
	}

	private IEnumerator DeactivateAfterTimeLimit()
	{
		yield return new WaitForSeconds(timeLimit);
		activator.Deactivate();
		activated = false;
	}
}
