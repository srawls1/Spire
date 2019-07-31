using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum InvItemType
{
	None = 0,
	Bottle,
	Sword,
	Bow,
	Staff,
	Talisman
}

[System.Serializable]
public enum PotionType
{
	Empty = 0,
	Body,
	Spirit,
	Stamina,
	InfiniteStamina,
	LightResist,
	Transmutation
}

[System.Serializable]
public class SerializedInventoryItem
{
	[SerializeField] InvItemType type;
	[SerializeField] PotionType potionType;
	[SerializeField] WeaponData swordData;
	[SerializeField] ProjectileShooterData shooterData;
	[SerializeField] float fillAmount;
	[SerializeField] float potionScale;
	[SerializeField] float duration;
	[SerializeField] Sprite sprite;
	[SerializeField] Sprite fullSprite;
	[SerializeField] Sprite partialSprite;
	[SerializeField] InventoryMenu inventoryMenu;

	public InventoryItem ToItem()
	{
		switch (type)
		{
			case InvItemType.Bottle:
				Bottle bottle = new Bottle(sprite);
				switch (potionType)
				{
					case PotionType.Empty:
						break;
					case PotionType.Body:
					{
						BodyPotion potion = new BodyPotion(partialSprite, fullSprite, potionScale);
						potion.Fill(fillAmount);
						bottle.containedPotion = potion;
						break;
					}
					case PotionType.Spirit:
					{
						SpiritPotion potion = new SpiritPotion(partialSprite, fullSprite, potionScale);
						potion.Fill(fillAmount);
						bottle.containedPotion = potion;
						break;
					}
					case PotionType.Stamina:
					{
						StaminaPotion potion = new StaminaPotion(partialSprite, fullSprite, potionScale);
						potion.Fill(fillAmount);
						bottle.containedPotion = potion;
						break;
					}
					case PotionType.InfiniteStamina:
					{
						bottle.containedPotion = new InfiniteStaminaPotion(fullSprite, duration);
						break;
					}
					case PotionType.LightResist:
						// TODO
						break;
					case PotionType.Transmutation:
						bottle.containedPotion = new TransmutationPotion(fullSprite, inventoryMenu);
						break;
				}
				return bottle;
			case InvItemType.Sword:
				return new SwordItem(sprite, swordData);
			case InvItemType.Bow:
				return new ShooterItem(sprite, shooterData, false);
			case InvItemType.Staff:
				return new ShooterItem(sprite, shooterData, true);
			case InvItemType.Talisman:
				// TODO
			default:
				return null;
		}
	}
}
