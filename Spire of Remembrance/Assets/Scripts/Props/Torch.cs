using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private bool m_lit;
	[SerializeField] private string idleAnimName;
	[SerializeField] private string litAnimName;
	[SerializeField] private float lightIntensity;
	[SerializeField] private float lightRadius;
	[SerializeField] private AnimationCurve lightFalloff;

	#endregion // Editor Fields

	int id = -1;

	#region Events

	public event Action OnLit;
	public event Action OnPutOut;

	#endregion // Events

	#region Properties

	public bool lit
	{
		get
		{
			return m_lit;
		}
		private set
		{
			m_lit = value;

			if (m_lit)
			{
				animator.Play(litAnimName);
				if (id == -1)
				{
					id = LightLevel.RegisterLightSource(transform.position, lightIntensity, lightFalloff, lightRadius);
				}
				if (OnLit != null)
				{
					OnLit();
				}
			}
			else
			{
				animator.Play(idleAnimName);
				if (id != -1)
				{
					LightLevel.UnregisterLightSource(id);
					id = -1;
				}
				if (OnPutOut != null)
				{
					OnPutOut();
				}
			}
		}
	}

	#endregion // Properties

	#region Non-Editor Fields

	Animator animator;

	#endregion // Non-Editor Fields

	#region Unity Functions

	void Awake()
	{
		animator = GetComponent<Animator>();
		lit = lit;
	}

	private void OnDestroy()
	{
		lit = false;
	}

	public void OnFireDamage(FireDamageArgs args)
	{
		lit = true;
	}

	public void OnIceDamage(IceDamageArgs args)
	{
		lit = false;
	}

	#endregion // Unity Functions
}
