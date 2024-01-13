using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDialogController : MonoBehaviour
{
	public Button Background;

	public Image Character;

	public Image DialogBox;

	public TMP_Text DialogContent;

	private bool inAnimation;

	private int currentTutorialText;

	[HideInInspector]
	public Action OnAnimationComplete;

	[HideInInspector]
	public List<string> TutorialTextList;

	private void Start()
	{
		Background.onClick.AddListener(OnClickBackground);
	}

	private void Update()
	{
	}

	public void Show(List<string> tutorialText, Action onComplete = null)
	{
		TutorialTextList = tutorialText;
		OnAnimationComplete = onComplete;
		if (TutorialTextList.Count <= 0)
		{
			if (OnAnimationComplete != null)
			{
				OnAnimationComplete();
			}
			return;
		}
		base.gameObject.SetActive(value: true);
		DOTween.Kill(Background.GetComponent<Image>());
		DOTween.Kill(Character);
		DOTween.Kill(DialogBox);
		DOTween.Kill(DialogContent);
		Image character = Character;
		Color color = new Color(1f, 1f, 1f, 0f);
		DialogBox.color = color;
		character.color = color;
		Image component = Background.GetComponent<Image>();
		color = new Color(0f, 0f, 0f, 0f);
		DialogContent.color = color;
		component.color = color;
		inAnimation = true;
		Background.GetComponent<Image>().DOFade(0.4f, 0.2f);
		Character.DOFade(1f, 0.2f);
		DialogBox.DOFade(1f, 0.2f);
		currentTutorialText = 0;
		DialogContent.maxVisibleCharacters = 0;
		((Graphic)DialogContent).DOFade(1f, 0.2f).onComplete = StartTextAnimation;
	}

	public void Hide()
	{
		DOTween.Kill(Background.GetComponent<Image>());
		DOTween.Kill(Character);
		DOTween.Kill(DialogBox);
		DOTween.Kill(DialogContent);
		Background.GetComponent<Image>().DOFade(0f, 0.2f);
		Character.DOFade(0f, 0.2f);
		DialogBox.DOFade(0f, 0.2f);
		((Graphic)DialogContent).DOFade(0f, 0.2f).OnComplete(delegate
		{
			base.gameObject.SetActive(value: false);
		});
	}

	public void OnClickBackground()
	{
		StopAllCoroutines();
		if (inAnimation)
		{
			CompleteTextAnimation();
		}
		else if (currentTutorialText < TutorialTextList.Count)
		{
			StartTextAnimation();
		}
		else if (OnAnimationComplete != null)
		{
			OnAnimationComplete();
		}
	}

	public void StartTextAnimation()
	{
		DialogContent.text = TutorialTextList[currentTutorialText];
		currentTutorialText++;
		DialogContent.maxVisibleCharacters = 0;
		StartCoroutine(StartText());
	}

	public void CompleteTextAnimation()
	{
		inAnimation = false;
		DialogContent.maxVisibleCharacters = DialogContent.text.Length;
	}

	public IEnumerator StartText()
	{
		inAnimation = true;
		while (inAnimation)
		{
			DialogContent.maxVisibleCharacters += 1;
			if (DialogContent.maxVisibleCharacters == DialogContent.text.Length)
			{
				break;
			}
			yield return new WaitForSeconds(0.01f);
		}
		CompleteTextAnimation();
		StartCoroutine(AutoNextScene());
	}

	public IEnumerator AutoNextScene()
	{
		yield return new WaitForSeconds(7f);
		OnClickBackground();
	}
}
