using System;
using UnityEngine;

public class Stamina : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private float m_totalStamina; // In seconds of possession
	[SerializeField] private float passiveRecoveryRate;

	#endregion // Editor Fields

	#region Non-Editor Fields

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
				if (isPossessing)
				{
					controller.Deposess();
				}
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
		controller = GetComponent<CharacterController>();
		movement = GetComponent<CharacterMovement>();
		currentStamina = totalStamina;
	}

	private void Update()
	{
		if (isPossessing)
		{
			currentStamina -= Time.deltaTime;
		}
		else
		{
			currentStamina += Time.deltaTime * passiveRecoveryRate;
		}
	}

	#endregion // Unity Functions

	#region Public Functions

	public void Restore(float amount)
	{
		currentStamina += Time.deltaTime;
	}

	#endregion // Public Functions
}
