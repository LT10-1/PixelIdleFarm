using UnityEngine;

public class DataUtils
{
	public enum ManageEffectSprite
	{
		Capacity,
		Cost,
		Load,
		Mine,
		Speed
	}

	public static Sprite GetSpriteEffect(int effectID, bool active = true)
	{
		int managerEffect = (int)GetManagerEffect(effectID);
		string resources = (!active) ? DATA_RESOURCES.IMAGE.DIALOG_MANAGER_SKILL_INACTIVE[managerEffect] : DATA_RESOURCES.IMAGE.BUTTON_SKILL[managerEffect];
		return BaseController.LoadSprite(resources);
	}

	public static ManageEffectSprite GetManagerEffect(int effectID)
	{
		switch (effectID)
		{
		case 1:
		case 9:
		case 11:
			return ManageEffectSprite.Speed;
		case 3:
		case 10:
		case 16:
			return ManageEffectSprite.Cost;
		case 8:
			return ManageEffectSprite.Mine;
		case 5:
		case 12:
			return ManageEffectSprite.Capacity;
		case 4:
		case 13:
			return ManageEffectSprite.Load;
		default:
			return ManageEffectSprite.Capacity;
		}
	}

	public static ManagerArea GetManagerArea(int effectID)
	{
		switch (effectID)
		{
		case 8:
		case 9:
		case 10:
			return ManagerArea.Corridor;
		case 11:
		case 12:
		case 13:
		case 16:
			return ManagerArea.Elevator;
		case 1:
		case 3:
		case 4:
		case 5:
			return ManagerArea.Ground;
		default:
			return (ManagerArea)0;
		}
	}

	public static string ManagerAreaText(ManagerArea area)
	{
		switch (area)
		{
		case ManagerArea.Corridor:
			return "Mine Shaft";
		case ManagerArea.Ground:
			return "Warehouse";
		case ManagerArea.Elevator:
			return "Elevator";
		default:
			return string.Empty;
		}
	}

	public static Sprite GetAvatarSprite(int areaID, int rarityID)
	{
		string resources = string.Empty;
		if (rarityID == -1)
		{
			resources = "Images/UI/avatar/unavailable";
		}
		else
		{
			switch (areaID)
			{
			case 1:
				resources = DATA_RESOURCES.IMAGE.AVATAR_CORRIDOR[rarityID - 1];
				break;
			case 3:
				resources = DATA_RESOURCES.IMAGE.AVATAR_ELEVATOR[rarityID - 1];
				break;
			case 2:
				resources = DATA_RESOURCES.IMAGE.AVATAR_GROUND[rarityID - 1];
				break;
			}
		}
		return BaseController.LoadSprite(resources);
	}

	public static Sprite GetItemBoostImage(ItemBoostMultiple multiple, ItemBoostDuration duration)
	{
		string text = "x" + (double)multiple + "_";
		switch (duration)
		{
		case ItemBoostDuration.Minutex5:
		case ItemBoostDuration.Hour:
			text += 1;
			break;
		case ItemBoostDuration.Hourx12:
		case ItemBoostDuration.Day:
			text += 2;
			break;
		case ItemBoostDuration.Week:
		case ItemBoostDuration.Weekx2:
			text += 3;
			break;
		}
		return BaseController.LoadSprite("Images/UI/Shop/Boost/" + text);
	}
}
