using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundExpedition : MonoBehaviour
{
	public Image road1;

	public Image road2;

	public Image roadLeft1;

	public Image roadLeft2;

	public Image roadRight1;

	public Image roadRight2;

	public Image treeRight1;

	public Image treeRight2;

	public Image treeLeft1;

	public Image treeLeft2;

	public Image sceneTreeLeft1;

	public Image sceneTreeLeft2;

	public Image sceneTreeRight1;

	public Image sceneTreeRight2;

	public Image cloud1;

	public Image cloud2;

	public GameObject center;

	[HideInInspector]
	public bool IsMoving;

	private void Awake()
	{
		moveCloud(cloud1, 15f);
		moveCloud(cloud2, 10f);
		InitTween();
	}

	public void InitTween()
	{
		Image image = road1;
		Image image2 = road2;
		Vector2 anchoredPosition = road1.GetComponent<RectTransform>().anchoredPosition;
		InitMoveTween(image, image2, anchoredPosition.y);
		Image image3 = roadLeft1;
		Image image4 = roadLeft2;
		Vector2 anchoredPosition2 = roadLeft1.GetComponent<RectTransform>().anchoredPosition;
		InitMoveTween(image3, image4, anchoredPosition2.y);
		Image image5 = roadRight1;
		Image image6 = roadRight2;
		Vector2 anchoredPosition3 = roadRight1.GetComponent<RectTransform>().anchoredPosition;
		InitMoveTween(image5, image6, anchoredPosition3.y);
		Image image7 = treeLeft1;
		Image image8 = treeLeft2;
		Vector2 anchoredPosition4 = treeLeft1.GetComponent<RectTransform>().anchoredPosition;
		InitMoveTween(image7, image8, anchoredPosition4.y);
		Image image9 = treeRight1;
		Image image10 = treeRight2;
		Vector2 anchoredPosition5 = treeRight1.GetComponent<RectTransform>().anchoredPosition;
		InitMoveTween(image9, image10, anchoredPosition5.y);
		Image image11 = sceneTreeLeft1;
		Image image12 = sceneTreeLeft2;
		Vector2 anchoredPosition6 = sceneTreeLeft1.GetComponent<RectTransform>().anchoredPosition;
		float x = anchoredPosition6.x;
		Vector2 anchoredPosition7 = sceneTreeLeft1.GetComponent<RectTransform>().anchoredPosition;
		InitMoveSceneTween(image11, image12, x, anchoredPosition7.y, sceneTreeLeft1.GetComponent<RectTransform>().rect.width);
		Image image13 = sceneTreeRight1;
		Image image14 = sceneTreeRight2;
		Vector2 anchoredPosition8 = sceneTreeRight1.GetComponent<RectTransform>().anchoredPosition;
		float x2 = anchoredPosition8.x;
		Vector2 anchoredPosition9 = sceneTreeRight1.GetComponent<RectTransform>().anchoredPosition;
		InitMoveSceneTween(image13, image14, x2, anchoredPosition9.y, 0f - sceneTreeRight1.GetComponent<RectTransform>().rect.width);
		if (!IsMoving)
		{
			StopMoving();
		}
	}

	public void StartMoving()
	{
		IsMoving = true;
		DotweenPlay(road1, road2, roadLeft1, roadLeft2, roadRight1, roadRight2, treeLeft1, treeLeft2, treeRight1, treeRight2, sceneTreeLeft1, sceneTreeLeft2, sceneTreeRight1, sceneTreeRight2);
	}

	public void StopMoving()
	{
		IsMoving = false;
		DotweenPause(road1, road2, roadLeft1, roadLeft2, roadRight1, roadRight2, treeLeft1, treeLeft2, treeRight1, treeRight2, sceneTreeLeft1, sceneTreeLeft2, sceneTreeRight1, sceneTreeRight2);
	}

	public void DotweenPlay(params Image[] images)
	{
		foreach (Image image in images)
		{
			DOTween.Play(image.GetComponent<RectTransform>());
		}
	}

	public void DotweenPause(params Image[] images)
	{
		foreach (Image image in images)
		{
			DOTween.Pause(image.GetComponent<RectTransform>());
		}
	}

	private void moveCloud(Image cloud, float dur)
	{
		cloud.transform.DOMove(center.transform.position, dur).SetEase(Ease.Linear).SetLoops(-1);
		cloud.DOFade(0f, dur).SetLoops(-1).SetEase(Ease.Linear);
		cloud.transform.DOScale(0.5f, dur).SetLoops(-1);
	}

	public void InitMoveSceneTween(Image image1, Image image2, float xBegin, float yBegin, float _W)
	{
		image1.GetComponent<RectTransform>().anchoredPosition = new Vector2(xBegin - _W / 6f, yBegin - Mathf.Abs(_W) / 10f);
		image2.GetComponent<RectTransform>().anchoredPosition = new Vector2(xBegin - _W, yBegin);
		image1.GetComponent<RectTransform>().DOAnchorPos(new Vector2(xBegin + _W / 3f, yBegin - sceneTreeLeft1.GetComponent<RectTransform>().rect.height), 20f).SetEase(Ease.Linear);
		image2.GetComponent<RectTransform>().DOAnchorPos(new Vector2(xBegin - _W / 6f, yBegin - Mathf.Abs(_W) / 10f), 7f).SetDelay(13f)
			.SetEase(Ease.Linear)
			.OnComplete(delegate
			{
				InitMoveSceneTween(image2, image1, xBegin, yBegin, _W);
			});
	}

	public void InitMoveTween(Image image1, Image image2, float yBegin)
	{
		float height = image1.GetComponent<RectTransform>().rect.height;
		float duration = height * 0.015f;
		RectTransform component = image1.GetComponent<RectTransform>();
		Vector2 anchoredPosition = image1.GetComponent<RectTransform>().anchoredPosition;
		component.anchoredPosition = new Vector2(anchoredPosition.x, yBegin);
		RectTransform component2 = image2.GetComponent<RectTransform>();
		Vector2 anchoredPosition2 = image2.GetComponent<RectTransform>().anchoredPosition;
		component2.anchoredPosition = new Vector2(anchoredPosition2.x, yBegin - height);
		image1.GetComponent<RectTransform>().DOAnchorPosY(yBegin + height, duration).SetEase(Ease.Linear);
		image2.GetComponent<RectTransform>().DOAnchorPosY(yBegin, duration).SetEase(Ease.Linear)
			.OnComplete(delegate
			{
				InitMoveTween(image2, image1, yBegin);
			});
	}
}
