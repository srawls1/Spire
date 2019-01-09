using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializedControlLabel
{
	public string inputKey;
	public ButtonLabel label;
}

public class ControlsHUD : MonoBehaviour
{
	[SerializeField] private SerializedControlLabel[] buttonLabels;

	private Dictionary<string, ButtonLabel> labels;

	public string this[string input]
	{
		get
		{
			return labels[input].action;
		}
		set
		{
			labels[input].action = value;
		}
	}

	private void Awake()
	{
		labels = new Dictionary<string, ButtonLabel>();
		for (int i = 0; i < buttonLabels.Length; ++i)
		{
			labels.Add(buttonLabels[i].inputKey, buttonLabels[i].label);
		}
	}

	private void Start()
	{
		CharacterController.instance.OnInteractableChanged += OnInteractableChanged;
	}

	private void Update()
	{
		if (!(CharacterController.instance.controlledMovement is CharacterMovement))
		{
			this["Attack"] = "Attack";
			this["Possess"] = "Deposess";
		}
		else
		{
			this["Attack"] = string.Empty;
			this["Possess"] = string.Empty;
		}
	}

	private void OnInteractableChanged(Interaction interaction, bool moreThanOne)
	{
		if (interaction == null)
		{
			this["Interact"] = string.Empty;
			this["RotateInteractable"] = string.Empty;
		}
		else
		{
			this["Interact"] = interaction.interactionText;
			if (moreThanOne)
			{
				this["RotateInteractable"] = "Switch Actions";
			}
			else
			{
				this["RotateInteractable"] = string.Empty;
			}
		}
	}
}
