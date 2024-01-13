using DG.Tweening;
using TMPro;
using UnityEngine;

public class LevelTextController : SpriteButtonController
{
	public Transform ArrowUpGroup;

	private TMP_Text _textMeshPro;

	private TMP_Text TextMeshPro => _textMeshPro ?? (_textMeshPro = GetComponentInChildren<TMP_Text>(includeInactive: true));

	private void Start()
	{
		Transform arrowUpGroup = ArrowUpGroup;
		Vector3 localPosition = ArrowUpGroup.localPosition;
		arrowUpGroup.DOLocalMoveY(localPosition.y + 0.07f, 0.65f).SetLoops(-1, LoopType.Yoyo);
	}

	public void SetLevel(int level)
	{
		TextMeshPro.text = $"Level\n{level}";
	}

	public void SetUpgradableByLevel(int level)
	{
		if (level >= 50)
		{
			SetUpgradableArrow(3);
		}
		else if (level >= 10)
		{
			SetUpgradableArrow(2);
		}
		else if (level >= 1)
		{
			SetUpgradableArrow(1);
		}
		else
		{
			SetUpgradableArrow(0);
		}
	}

	public void SetUpgradableArrow(int upgradable)
	{
		for (int i = 0; i < ArrowUpGroup.childCount; i++)
		{
			int num = ArrowUpGroup.childCount - i - 1;
			ArrowUpGroup.GetChild(i).gameObject.SetActive(num < upgradable);
		}
	}
}
