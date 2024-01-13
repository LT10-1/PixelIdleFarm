using System.Collections;
using UnityEngine;

public class MineEffectController : BaseMineController
{
	public GameObject NewShaftEffect;

	public CharacterControllerSekeleton[] NewShaftEffectAnimation;

	public GameObject NewMilestoneEffect;

	public CharacterControllerSekeleton[] NewMilestoneAnimation;

	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
		base.Update();
	}

	public void StartNewShaftEffect()
	{
		NewShaftEffect.gameObject.SetActive(value: true);
		for (int i = 0; i < NewShaftEffectAnimation.Length; i++)
		{
			NewShaftEffectAnimation[i].spineAnimationState.SetAnimation(0, "explode", loop: false);
		}
	}

	public void HideNewShaftEffect()
	{
		NewShaftEffect.gameObject.SetActive(value: false);
	}

	public void StartMilestoneEffect()
	{
		for (int i = 0; i < NewMilestoneAnimation.Length; i++)
		{
			NewMilestoneAnimation[i].gameObject.SetActive(value: false);
		}
		NewMilestoneEffect.gameObject.SetActive(value: true);
		StopAllCoroutines();
		StartCoroutine(MileStoneEffect());
	}

	private IEnumerator MileStoneEffect()
	{
		for (int i = 0; i < NewMilestoneAnimation.Length; i++)
		{
			NewMilestoneAnimation[i].gameObject.SetActive(value: true);
			NewMilestoneAnimation[i].spineAnimationState.SetAnimation(0, "animation", loop: false);
			yield return new WaitForSeconds(0.2f);
		}
	}

	public void HideMilestoneEffect()
	{
		NewMilestoneEffect.gameObject.SetActive(value: false);
	}
}
