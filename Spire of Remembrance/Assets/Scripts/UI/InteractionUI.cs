using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
	[SerializeField] private GameObject interactionPane;
	[SerializeField] private GameObject switchActionRow;
	[SerializeField] private Text interactionText;
	[SerializeField] private Color disabledColor;

	private void Start()
	{
		CharacterController character = FindObjectOfType<CharacterController>();
		character.OnInteractableChanged += ShowInteractable;
	}
	
	private void ShowInteractable(Interaction interactable, bool moreThanOne)
	{
		if (interactable == null)
		{
			interactionPane.SetActive(false);
		}
		else
		{
			interactionPane.SetActive(true);
			interactionText.text = interactable.interactionText;
			interactionText.color = interactable.enabled ? Color.white : disabledColor;
			interactionText.gameObject.SetActive(false);
			interactionText.gameObject.SetActive(true);

			switchActionRow.SetActive(moreThanOne);
		}
	}
}
