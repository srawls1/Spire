using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PricedItemDescriptionField : ItemDescriptionField
{
	[SerializeField] private Text priceText;
	[SerializeField] private Color cantAffordColor;
	[SerializeField] private Color normalColor;

	private int m_price;
	public int price
	{
		get
		{
			return m_price;
		}
		set
		{
			m_price = value;
			if (m_price > CharacterController.instance.numSpiritOrbs)
			{
				priceText.color = cantAffordColor;
			}
			else
			{
				priceText.color = normalColor;
			}

			priceText.text = "" + m_price;
		}
	}
}
