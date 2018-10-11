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

	#endregion // Editor Fields

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
		set
		{
			m_lit = value;

			if (m_lit)
			{
				animator.Play(litAnimName);
				if (OnLit != null)
				{
					OnLit();
				}
			}
			else
			{
				animator.Play(idleAnimName);
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

	#endregion // Unity Functions
}
