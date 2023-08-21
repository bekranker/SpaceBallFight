using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AdsButtonInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text Info;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Info.DOFade(1, .25f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Info.DOFade(0, .25f);
    }

}
