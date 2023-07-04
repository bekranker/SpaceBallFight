using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonClickManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    //the delegate methods
    #region Delegate
    [HideInInspector]
    public delegate void delegateMethod();
    [HideInInspector]
    public delegateMethod DoSomething;
    #endregion
    [Space(10)]
    [Header("---Dotween---")]
    [SerializeField] private Image _ButtonIcon;
    [SerializeField] private Vector3 _ToScale;
    private Vector3 _startScale;
    [Space(10)]
    [Header("---Managers---")]
    [HideInInspector] public bool CanClick;
    private bool _didEnter;
    private bool _didClick;
    private void Start()
    {
        CanClick = true;
        _didClick = false;
        _startScale = transform.localScale;
    }
    //This function is only working when mouse is hovering the UI buttons. Also Its working only When press or hold
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CanClick) return;
        transform.DOScale(_ToScale, .05f);
        _ButtonIcon.transform.DOPunchRotation(Vector3.forward * 10, .1f);
        _didClick = true;
        CanClick = false;
    }
    public void OnPointerEnter(PointerEventData eventData) => _didEnter = true;
    public void OnPointerExit(PointerEventData eventData) => _didEnter = false;
    //This function is only working when mouse is hovering the UI buttons. Also Its working only When remove the click
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(_startScale, .05f);
        if (!_didClick) return;
        if (_didEnter)
        {
            if (DoSomething != null)
                DoSomething();
        }
        else
        {
            _didClick = false;
            CanClick = true;
            return;
        }
        _didClick = false;
    }
}