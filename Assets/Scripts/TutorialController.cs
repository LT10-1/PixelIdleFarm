using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : BaseController
{
	public Transform TutorialUI;

	public SpotlightController SpotlightController;

	public TutorialDialogController TutorialDialogController;

	[HideInInspector]
	public Dictionary<int, TutorialArrowController> TutorialArrowControllers = new Dictionary<int, TutorialArrowController>();

	[HideInInspector]
	public List<TutorialType> CompletedTutorial = new List<TutorialType>();

	public void RemoveTutorial(TutorialType tutorialType)
	{
		if (TutorialArrowControllers.ContainsKey((int)tutorialType))
		{
			UnityEngine.Object.Destroy(TutorialArrowControllers[(int)tutorialType].gameObject);
			TutorialArrowControllers.Remove((int)tutorialType);
			CompletedTutorial.Add(tutorialType);
			SpotlightController.Hide();
		}
	}

	public void RemoveAllTutorial()
	{
		foreach (KeyValuePair<int, TutorialArrowController> tutorialArrowController in TutorialArrowControllers)
		{
			UnityEngine.Object.Destroy(tutorialArrowController.Value.gameObject);
		}
		TutorialArrowControllers = new Dictionary<int, TutorialArrowController>();
		SpotlightController.Hide();
	}

	public void CreateTutorial(TutorialType tutorialType, GameObject gameObject, bool isUI = false, float distance = 1f, bool useSpotlight = false, float spotlightScale = 1f, Vector3 spotlightDistance = default(Vector3))
	{
		if (CompletedTutorial.Contains(tutorialType) || TutorialArrowControllers.ContainsKey((int)tutorialType))
		{
			return;
		}
		if (!useSpotlight || DataManager.Instance.SavegameData.CurrentMineUnlocked != 0 || (!isUI && BaseController.GameController.DialogController.GetNumberActiveDialog() != 0))
		{
			CreateTutorialArrow(tutorialType, gameObject, isUI, distance);
			return;
		}
		Vector3 spotlightPosition = gameObject.transform.position;
		if (!isUI)
		{
			spotlightPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		}
		spotlightPosition += spotlightDistance;
		SpotlightController.transform.localScale = Vector3.one * spotlightScale * CONST.GRAPHIC_SCREEN_RATIO;
		List<string> list = new List<string>();
		switch (tutorialType)
		{
		case TutorialType.NewShaft:
			list.Add("Hello tycoon, we mining dwarfs are coming to work for you!");
			list.Add("Let's start with your first mine now!");
			break;
		case TutorialType.WorkerCorridor:
			list.Add("Click on your miner to start digging!");
			break;
		case TutorialType.WorkerElevator:
			list.Add("Click on the Elevator to tranfer Resources to the ground!");
			break;
		case TutorialType.WorkerGround:
			list.Add("Click on the Transporter to finally move the resources to your warehouse!");
			break;
		case TutorialType.ButtonLevelUp:
			list.Add("Upgrade your mine to produce more resources!");
			break;
		case TutorialType.ClickWorkerAgain:
			list.Add("Now continue clicking on us to make even more money!");
			list.Add("It won't be too long until you can automate the workflow.");
			break;
		case TutorialType.HireManager:
			list.Add("Wow, you have the money now. Hire a Manager to automate the work for you!");
			break;
		}
		TutorialDialogController.Show(list, delegate
		{
			TutorialDialogController.Hide();
			if (tutorialType != TutorialType.ClickWorkerAgain)
			{
				CreateTutorialArrow(tutorialType, gameObject, isUI, distance);
				SpotlightController.Show(spotlightPosition);
			}
		});
	}

	public void CreateTutorialArrow(TutorialType tutorialType, GameObject gameObject, bool isUI, float distance)
	{
		TutorialArrowController tutorialArrowController = CreateTutorial(tutorialType, isUI);
		tutorialArrowController.transform.SetParent((!isUI) ? base.transform : TutorialUI);
		tutorialArrowController.transform.position = gameObject.transform.position + Vector3.up * distance * ((!isUI) ? 1f : CONST.PIXEL_PER_UNIT);
	}

	public TutorialArrowController CreateTutorial(TutorialType tutorialType, bool isUI)
	{
		TutorialArrowController component = InstantiatePrefab((!isUI) ? "Prefabs/TutorialArrow" : "Prefabs/TutorialArrowUI").GetComponent<TutorialArrowController>();
		TutorialArrowControllers.Add((int)tutorialType, component);
		return component;
	}

	public void StartFlashEff(SpriteRenderer spriteRenderer)
	{
		if (spriteRenderer.GetComponentInChildren<FlashSpriteEffect>() == null)
		{
			FlashSpriteEffect component = InstantiatePrefab("Prefabs/Effects/FlashSpriteEff").GetComponent<FlashSpriteEffect>();
			component.InitEffect(spriteRenderer);
		}
	}

	public void StopFlashEff(SpriteRenderer spriteRenderer)
	{
		FlashSpriteEffect componentInChildren = spriteRenderer.GetComponentInChildren<FlashSpriteEffect>();
		if (componentInChildren != null)
		{
			componentInChildren.stopEffect();
		}
	}

	public FlashEfect StartFlashEff(Button button)
	{
		if (button.GetComponentInChildren<FlashEfect>() == null)
		{
			FlashEfect component = InstantiatePrefab("Prefabs/Effects/FlashEffect").GetComponent<FlashEfect>();
			component.InitEffect(button.GetComponent<Image>());
			return component;
		}
		return button.GetComponentInChildren<FlashEfect>();
	}

	public void StopFlashEff(Button button)
	{
		FlashEfect componentInChildren = button.GetComponentInChildren<FlashEfect>();
		if (componentInChildren != null)
		{
			componentInChildren.stopEffect();
		}
	}
}
