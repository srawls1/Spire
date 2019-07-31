using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private float m_totalStamina; // In seconds of possession
	[SerializeField] private float passiveRecoveryRate;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private LinkedList<StaminaFilter> filters;
	private CharacterController controller;
	private CharacterMovement movement;
	private float m_currentStamina;

	#endregion // Non-Editor Fields

	#region Event

	public event Action<float, float> OnStaminaChanged;

	#endregion // Event

	#region Properties

	public float totalStamina
	{
		get
		{
			return m_totalStamina;
		}
	}

	public float currentStamina
	{
		get
		{
			return m_currentStamina;
		}
		private set
		{
			m_currentStamina = value;
			if (m_currentStamina <= 0f)
			{
				m_currentStamina = 0f;
			}

			if (m_currentStamina > totalStamina)
			{
				m_currentStamina = totalStamina;
			}

			if (OnStaminaChanged != null)
			{
				OnStaminaChanged(currentStamina, totalStamina);
			}
		}
	}

	private bool isPossessing
	{
		get
		{
			return controller.controlledMovement != movement;
		}
	}

	#endregion // Properties

	#region Unity Functions

	private void Awake()
	{
		filters = new LinkedList<StaminaFilter>();
		AddFilter(new DefaultStaminaFilter());
		controller = GetComponent<CharacterController>();
		movement = GetComponent<CharacterMovement>();
		currentStamina = totalStamina;
	}

	//<temp>
	private void Update()
	{
		if (isPossessing)
		{
			if (!Expend(Time.deltaTime))
			{
				if (isPossessing)
				{
					controller.Deposess();
				}
			}
		}
		else
		{
			Expend(-Time.deltaTime * passiveRecoveryRate);
		}
	}
	//</temp>

	#endregion // Unity Functions

	#region Public Functions

	public void AddFilter(StaminaFilter filter)
	{
		for (LinkedListNode<StaminaFilter> node = filters.First; node != null; node = node.Next)
		{
			if (node.Value.priority > filter.priority)
			{
				filters.AddBefore(node, filter);
				return;
			}
		}
		filters.AddLast(filter);
	}

	public void RemoveFilter(StaminaFilter filter, float delay = 0f)
	{
		if (delay > 0)
		{
			StartCoroutine(RemoveFilterAfterDelay(filter, delay));
		}
		else
		{
			filters.Remove(filter);
		}
	}

	public bool Expend(float amount)
	{
		StaminaExpense expense = new StaminaExpense();
		expense.cost = amount;

		foreach (StaminaFilter filter in filters)
		{
			filter.FilterExpense(expense, this);
		}

		if (expense.canAfford)
		{
			currentStamina -= expense.cost;
		}

		return expense.canAfford;
	}

	#endregion // Public Functions

	#region Private Functions

	private IEnumerator RemoveFilterAfterDelay(StaminaFilter filter, float delay)
	{
		yield return new WaitForSeconds(delay);
		filters.Remove(filter);
	}

	#endregion // Private Functions
}
