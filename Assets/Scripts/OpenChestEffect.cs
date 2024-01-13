using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenChestEffect : BaseController
{
	public Image body;

	public Image lid;

	public Image open;

	public Image lightEff;

	public UIButtonController chest;

	public TMP_Text ChestCountText;

	[HideInInspector]
	public ChestType ChestType;

	public OpenChest OpenChestDialog => BaseController.GameController.DialogController.OpenChest;

	public override void Start()
	{
		lightEff.transform.DOLocalRotate(Vector3.forward * 360f, 6f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
		BaseController.GameController.OnChestChangeCallback.Add(UpdateChestText);
	}

	public void UpdateChestText()
	{
		int num = DataManager.Instance.SavegameData.ChestSavegames.ContainsKey((int)ChestType) ? DataManager.Instance.SavegameData.ChestSavegames[(int)ChestType] : 0;
		ChestCountText.transform.parent.gameObject.SetActive(num > 0);
		ChestCountText.text = num.ToString();
	}

	public void ChangeChestState(OpenChestState state)
	{
		switch (state)
		{
		case OpenChestState.EndAnimation:
			break;
		case OpenChestState.Idle:
			SetOpen(isOpen: false);
			StopAllTween();
			break;
		case OpenChestState.OpeningChest:
			StartOpen();
			break;
		case OpenChestState.ShowingCards:
			EndOpen();
			chest.transform.DOScale(Vector3.one * 1.2f, 0.1f);
			chest.transform.DOScale(Vector3.one, 0.1f).SetDelay(0.1f);
			break;
		}
	}

	public void SetOpen(bool isOpen)
	{
		body.gameObject.SetActive(!isOpen);
		lid.gameObject.SetActive(!isOpen);
		open.gameObject.SetActive(isOpen);
	}

	public void StopAllTween()
	{
		chest.transform.localScale = Vector3.one;
		DOTween.Kill(body);
		DOTween.Kill(lid);
		DOTween.Kill(chest);
	}

	public void StartOpen()
	{
		StartCoroutine(_StartOpen());
	}

	private IEnumerator _StartOpen()
	{
		StopAllTween();
		SetOpen(isOpen: false);
		chest.transform.localScale = Vector3.one * 3f;
		chest.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBounce);
		yield return new WaitForSeconds(0.5f);
		lid.transform.localEulerAngles = new Vector3(0f, 0f, -5f);
		lid.transform.DORotate(new Vector3(0f, 0f, 5f), 0.1f).SetLoops(10, LoopType.Yoyo);
		lid.transform.localEulerAngles = new Vector3(0f, 0f, -2f);
		chest.transform.DORotate(new Vector3(0f, 0f, 2f), 0.2f).SetLoops(5, LoopType.Yoyo);
		yield return new WaitForSeconds(1f);
		lid.transform.DORotate(new Vector3(0f, 0f, 0f), 0.1f);
		chest.transform.DORotate(new Vector3(0f, 0f, 0f), 0.1f);
		OpenChestDialog.ChangeChestState(OpenChestState.ShowingCards);
	}

	public void EndOpen()
	{
		StopAllCoroutines();
		StopAllTween();
		SetOpen(isOpen: true);
	}
}
