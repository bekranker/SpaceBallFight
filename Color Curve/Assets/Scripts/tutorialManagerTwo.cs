using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorialManagerTwo : MonoBehaviour
{
    [SerializeField] private Image _Scroll;
    private void Start()
    {
        animationOfScroll();
    }
    IEnumerator fadeToZero()
    {
        yield return new WaitForSeconds(10);
        GetComponent<CanvasGroup>().DOFade(0, 1).OnComplete(()=> gameObject.SetActive(false));
    }
    void animationOfScroll()
    {
        if (_Scroll == null) return;
        _Scroll.DOFade(0, 1).OnComplete(() => { _Scroll.DOFade(1, 1).OnComplete(() => { animationOfScroll(); }); });
    }
}
