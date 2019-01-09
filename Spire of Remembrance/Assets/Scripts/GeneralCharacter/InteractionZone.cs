using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class InteractionZone : MonoBehaviour
//{
//	private Movement movement;
//	List<Interactable> interactables;

//	private void Awake()
//	{
//		movement = GetComponentInParent<Movement>();
//		interactables = new List<Interactable>();
//	}

//	private void OnTriggerEnter2D(Collider2D collision)
//	{
//		Interactable interactable = collision.GetComponent<Interactable>();
//		if (interactable != null)
//		{
//			interactables.Add(interactable);
//			movement.NewInteractables(interactables);
//		}
//	}

//	private void OnTriggerExit2D(Collider2D collision)
//	{
//		Interactable interactable = collision.GetComponent<Interactable>();
//		if (interactable != null)
//		{
//			interactables.Remove(interactable);
//			movement.NewInteractables(interactables);
//		}
//	}
//}
