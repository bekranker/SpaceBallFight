using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AdsButtonInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text Info;
    private Transform _t;
    private Vector2 _firstScale;

    private void Start()
    {
        _t = transform;
        _firstScale = _t.localScale;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _t.DOScale(_firstScale * 1.15f, .25f);
        Info.DOFade(1, .25f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _t.DOScale(_firstScale, .25f);
        Info.DOFade(0, .25f);
    }

}
