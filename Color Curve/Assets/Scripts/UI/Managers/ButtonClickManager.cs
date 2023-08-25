using UnityEngine;
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
    [SerializeField] private Vector3 _ToScale;
    private Vector3 _startScale;
    [Space(10)]
    [Header("---Managers---")]
    [HideInInspector] public bool CanClick;
    private bool _didEnter;
    private bool _didClick;
    private Transform _t;
    private void Start()
    {
        _t = transform;
        CanClick = true;
        _didClick = false;
        _startScale = _t.localScale;
    }
    //This function is only working when mouse is hovering the UI buttons. Also Its working only When press or hold
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CanClick) return;
        _t.DOScale(_ToScale, .05f).SetUpdate(true);
        _didClick = true;
        CanClick = false;
    }
    public void OnPointerEnter(PointerEventData eventData) => _didEnter = true;
    public void OnPointerExit(PointerEventData eventData) => _didEnter = false;
    //This function is only working when mouse is hovering the UI buttons. Also Its working only When remove the click
    public void OnPointerUp(PointerEventData eventData)
    {
        _t.DOScale(_startScale, .05f).SetUpdate(true);
        Audio.PlayAudio("Click", .15f);
        if (!_didClick) return;
        if (_didEnter)
        {
            DoSomething?.Invoke();
            CanClick = true;
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