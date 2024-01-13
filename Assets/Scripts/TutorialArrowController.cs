using DG.Tweening;
using UnityEngine;

public class TutorialArrowController : MonoBehaviour
{
	protected virtual bool IsUI => false;

	public virtual void Start()
	{
		if (IsUI)
		{
			Transform transform = base.transform;
			Vector3 localPosition = base.transform.localPosition;
			transform.DOLocalMoveY(localPosition.y + 20f, 0.65f).SetLoops(-1, LoopType.Yoyo);
		}
		else
		{
			Transform transform2 = base.transform;
			Vector3 localPosition2 = base.transform.localPosition;
			transform2.DOLocalMoveY(localPosition2.y + 0.2f, 0.65f).SetLoops(-1, LoopType.Yoyo);
		}
	}
}
