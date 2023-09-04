using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<ButtonClickManager> _ContunieManager;
    [SerializeField] private List<GameObject> _TutorialPanels;
    [SerializeField] private Vector2 _EffectScale;
    private UIManager _uIManager;
    private int _index;
    [SerializeField] private bool _CANT;

    private void Start()
    {
        _uIManager = GameObject.FindWithTag("CanvasManager").GetComponent<UIManager>();
        if (_CANT)
        {
            if (PlayerPrefs.HasKey("FirstTutorial"))
            {
                _uIManager.CanClick = true;
                Destroy(gameObject);
            }
            else
            {
                Time.timeScale = 0;
                _uIManager.CanClick = false;
                _ContunieManager.ForEach((button) => { button.DoSomething += ContunieButtonf; });
                _TutorialPanels[_index].transform.DOPunchScale(_EffectScale, .1f).SetUpdate(true);
            }
        }
        else
        {
            Time.timeScale = 0;
            _uIManager.CanClick = false;
            _ContunieManager.ForEach((button) => { button.DoSomething += ContunieButtonf; });
            _TutorialPanels[_index].transform.DOPunchScale(_EffectScale, .1f).SetUpdate(true);
        }
    }
    private void ContunieButtonf()
    {
        _TutorialPanels[_index].SetActive(false);
        if (_index < _TutorialPanels.Count - 1)
        {
            _index++;
            _TutorialPanels[_index].SetActive(true);
            _TutorialPanels[_index].transform.DOPunchScale(_EffectScale, .1f).SetUpdate(true);
        }
        else
        {
            SetTimeScaleToOne();
        }
    }
    private void SetTimeScaleToOne()
    {
        _TutorialPanels[_index].SetActive(false);
        Time.timeScale = 1;
        _uIManager.CanClick = true;
        if (_CANT)
        {
            PlayerPrefs.SetInt("FirstTutorial", 1);
        }
        Destroy(gameObject, 0.05f);
    }
}
