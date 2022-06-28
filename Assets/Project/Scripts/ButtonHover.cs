using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator _animator;
    private static readonly int _hoverId = Animator.StringToHash("Hover");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _animator.SetBool(_hoverId, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animator.SetBool(_hoverId, false);
    }
}
