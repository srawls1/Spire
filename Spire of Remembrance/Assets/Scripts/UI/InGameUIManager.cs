using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
	private static InGameUIManager m_instance;
	public static InGameUIManager instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = FindObjectOfType<InGameUIManager>();
			}
			return m_instance;
		}
	}

	private void Awake()
	{
		if (m_instance == null)
		{
			m_instance = this;
		}
		else if (m_instance != this)
		{
			Destroy(this);
		}
	}

	[SerializeField] private CounterUI keyCounter;
	[SerializeField] private CounterUI spiritOrbCounter;
	[SerializeField] private RectTransform pickupNotificationBox;
	[SerializeField] private PickupNotification pickupNotifPrefab;
	[SerializeField] private float notificationDuration;

	public void SetNumKeys(int numKeys)
	{
		keyCounter.SetCount(numKeys);
	}

	public void SetNumSpiritOrbs(int numOrbs)
	{
		spiritOrbCounter.SetCount(numOrbs);
	}

	public void ShowPickup(string message)
	{
		StartCoroutine(ShowPickupRoutine(message));
	}

	private IEnumerator ShowPickupRoutine(string message)
	{
		int i = 0;
		for (; i < pickupNotificationBox.childCount &&
			pickupNotificationBox.GetChild(i).gameObject.activeSelf; ++i) ;

		PickupNotification notification;
		if (i == pickupNotificationBox.childCount)
		{
			notification = Instantiate(pickupNotifPrefab);
			notification.transform.SetParent(pickupNotificationBox);
		}
		else
		{
			notification = pickupNotificationBox.GetChild(i).GetComponent<PickupNotification>();
			notification.gameObject.SetActive(true);
		}

		notification.message = message;
		yield return new WaitForSeconds(notificationDuration);
		notification.gameObject.SetActive(false);
		notification.transform.SetAsLastSibling();
	}
}
