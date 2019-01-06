using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionUI : MonoBehaviour
{
	private static WeaponSelectionUI m_instance;
	public static WeaponSelectionUI instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = FindObjectOfType<WeaponSelectionUI>();
			}
			return m_instance;
		}
	}

	#region Editor Fields

	[SerializeField] private Image[] weaponImages;
	[SerializeField] private Color inactiveColor;
	[SerializeField] private Color activeColor = Color.white;
	[SerializeField] private float activeScaleUp;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private List<InventoryItem> availableWeapons;
	private Image controlImage;

	#endregion // Non-Editor Fields

	#region Properties

	private int m_selectedWeaponIndex;
	public int selectedWeaponIndex
	{
		get
		{
			return m_selectedWeaponIndex;
		}
		set
		{
			if (m_selectedWeaponIndex >= 0)
			{
				RectTransform trans = weaponImages[m_selectedWeaponIndex].rectTransform;
				trans.localScale /= activeScaleUp;
				weaponImages[m_selectedWeaponIndex].color = inactiveColor;
			}

			m_selectedWeaponIndex = value;

			if (m_selectedWeaponIndex >= 0)
			{
				RectTransform trans = weaponImages[m_selectedWeaponIndex].rectTransform;
				trans.localScale *= activeScaleUp;
				weaponImages[m_selectedWeaponIndex].color = activeColor;
			}
		}
	}

	#endregion // Properties

	#region Public Functions

	public void SetAvailableWeapons(List<InventoryItem> weapons)
	{
		availableWeapons = weapons.GetRange(0, Mathf.Min(weaponImages.Length, weapons.Count));
		int i = 0;
		for (; i < availableWeapons.Count; ++i)
		{
			weaponImages[i].gameObject.SetActive(true);
			weaponImages[i].sprite = weapons[i].sprite;
		}
		for (; i < weaponImages.Length; ++i)
		{
			weaponImages[i].gameObject.SetActive(false);
		}

		if (controlImage != null)
		{
			if (availableWeapons.Count == 0)
			{
				controlImage.color = inactiveColor;
			}
			else
			{
				controlImage.color = activeColor;
			}
		}
	}

	public void ShowSelectedWeapon(InventoryItem weapon)
	{
		int index = availableWeapons.IndexOf(weapon);
		selectedWeaponIndex = index;
	}

	#endregion // Public Functions

	#region Unity Functions

	private void Awake()
	{
		controlImage = GetComponent<Image>();
		availableWeapons = new List<InventoryItem>();
		m_selectedWeaponIndex = -1;
	}

	#endregion // Unity Functions
}
