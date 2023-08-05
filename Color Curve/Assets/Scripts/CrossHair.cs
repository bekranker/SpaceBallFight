using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CrossHair : MonoBehaviour
{
    [SerializeField] private PlayerController _PlayerController;
    [SerializeField] private SpriteRenderer _SpriteRenderer;
    [SerializeField] private Color _Blue, _Red, _Green;

    private Vector3 _mousePos;
    private Camera _camera;

    private void Start()
    {
        Cursor.visible = false;
        ChangeColorCross();
        _camera = Camera.main;
    }
    void Update()
    {
        _mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        _mousePos.z = 0;
        transform.position = _mousePos;
    }
    public void ShootEffect()
    {
        transform.DOScale(1.5f, .05f).OnComplete(() =>
        {
            transform.localScale = Vector3.one;
        });
    }
    public void ChangeColorCross()
    {
        switch (_PlayerController._PlayerStates)
        {
            case PlayerState.Red:
                _SpriteRenderer.color = _Red;
                break;
            case PlayerState.Green:
                _SpriteRenderer.color = _Green;
                break;
            case PlayerState.Blue:
                _SpriteRenderer.color = _Blue;
                break;
            default:
                break;
        }
    }
}