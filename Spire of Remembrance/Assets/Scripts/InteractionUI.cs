using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
	[SerializeField] private GameObject interactionPane;
	[SerializeField] private GameObject switchActionRow;
	[SerializeField] private Text interactionText;

	private void Start()
	{
		CharacterController character = FindObjectOfType<CharacterController>();
		character.OnInteractableChanged += ShowInteractable;
	}
	
	private void ShowInteractable(GameObject interactable, bool moreThanOne)
	{
		if (interactable == null)
		{
			interactionPane.SetActive(false);
		}
		else
		{
			Interactable inter = interactable.GetComponent<Interactable>();

			interactionPane.SetActive(true);
			interactionText.text = (inter != null) ? inter.interactionText : "Possess";

			switchActionRow.SetActive(moreThanOne);
		}
	}
}
