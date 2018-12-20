using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	private static InventoryManager m_instance;
	public static InventoryManager instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = FindObjectOfType<InventoryManager>();
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


	public Bottle GetAvailableBottle(Type potionType)
	{
		throw new NotImplementedException();
	}
}
