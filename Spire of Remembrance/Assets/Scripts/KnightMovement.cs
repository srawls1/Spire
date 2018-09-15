using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovement : Movement
{
	#region Editor Fields

	[SerializeField] private int damage;
	[SerializeField] private float attackForce;
	private Weapon m_weapon;

	#endregion // Editor Fields

	#region Properties

	public Weapon weapon
	{
		get
		{
			return m_weapon;
		}
		set
		{
			if (m_weapon != null)
			{
				m_weapon.transform.parent = null;
				m_weapon.gameObject.SetActive(false);
			}

			m_weapon = value;

			if (m_weapon != null)
			{
				m_weapon.gameObject.SetActive(false);
				m_weapon.transform.parent = transform;
				m_weapon.transform.localPosition = Vector3.zero;
				m_weapon.damage = damage;
				m_weapon.attackForce = attackForce;
				m_weapon.gameObject.name = "Weapon";
			}
		}
	}

	#endregion // Properties

	#region Unity Functions

	private void Start()
	{
		weapon = GetComponentInChildren<Weapon>();
	}

	public void OnStartAttackAnimation()
	{
		weapon.gameObject.SetActive(true);
	}

	public void OnFinishAttackAnimation()
	{
		weapon.gameObject.SetActive(false);
	}

	#endregion // Unity Functions

	#region Override Functions

	public override void Attack()
	{
		animator.SetTrigger("Attack");
	}

	protected override bool canInteract(GameObject obj)
	{
		return obj.GetComponent<Interactable>() != null;
	}

	protected override IEnumerator interact(GameObject obj)
	{
		yield return obj.GetComponent<Interactable>().Interact(gameObject);
	}

	#endregion // Override Functions
}
