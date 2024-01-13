using UnityEngine;
using UnityEngine.EventSystems;

public class GestureController : BaseDialog, IEndDragHandler, IEventSystemHandler
{
	public enum DraggedDirection
	{
		Up,
		Down,
		Right,
		Left
	}

	public virtual void OnEndDragCallback(DraggedDirection direction)
	{
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		Vector3 dragVector = (eventData.position - eventData.pressPosition).normalized;
		OnEndDragCallback(GetDragDirection(dragVector));
	}

	public DraggedDirection GetDragDirection(Vector3 dragVector)
	{
		float num = Mathf.Abs(dragVector.x);
		float num2 = Mathf.Abs(dragVector.y);
		if (num > num2)
		{
			return (!(dragVector.x > 0f)) ? DraggedDirection.Left : DraggedDirection.Right;
		}
		return (!(dragVector.y > 0f)) ? DraggedDirection.Down : DraggedDirection.Up;
	}
}
