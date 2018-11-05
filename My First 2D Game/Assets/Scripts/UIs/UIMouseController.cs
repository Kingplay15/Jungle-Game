using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIMouseController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent OnLeft;
    public UnityEvent OnRight;
    public UnityEvent OnMiddle;
    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeft.Invoke();
            SoundController.Instance.PlayMouseClick();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRight.Invoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            OnMiddle.Invoke();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnExit.Invoke();
    }
}