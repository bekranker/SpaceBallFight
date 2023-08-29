using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _W, _A, _S, _D;
    [SerializeField] private Image _Up, _Down, _Right, _Left;
    [SerializeField] private Color _PressedColor;
    [SerializeField] Image _MouseLeftClick;
    private void Start()
    {
        StartCoroutine(closeTutorial());
    }
    IEnumerator closeTutorial()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        GetComponent<CanvasGroup>().interactable = false;
        yield return new WaitForSeconds(10);
        GetComponent<CanvasGroup>().DOFade(0, 1).OnComplete(()=> 
        {
            gameObject.SetActive(false);
        });
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _MouseLeftClick.color = _PressedColor;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _MouseLeftClick.color = Color.white;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            _W.color = _PressedColor;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            _W.color = Color.white;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _S.color = _PressedColor;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            _S.color = Color.white;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _A.color = _PressedColor;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            _A.color = Color.white;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _D.color = _PressedColor;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            _D.color = Color.white;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _Up.color = _PressedColor;
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            _Up.color = Color.white;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _Down.color = _PressedColor;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            _Down.color = Color.white;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _Right.color = _PressedColor;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            _Right.color = Color.white;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _Left.color = _PressedColor;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            _Left.color = Color.white;
        }
    }
}
