using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator _animator;
    private static readonly int _hoverId = Animator.StringToHash("Hover");

    #region ### ------------------------------ MonoBehaviour ------------------------------ ###

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    #endregion ### ------------------------------ MonoBehaviour ------------------------------ ###

    #region ### ------------------------------ Public methods ------------------------------ ###

    public void OnPointerEnter(PointerEventData eventData)
    {
        _animator.SetBool(_hoverId, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animator.SetBool(_hoverId, false);
    }

    #endregion ### ------------------------------ Public methods ------------------------------ ###
}