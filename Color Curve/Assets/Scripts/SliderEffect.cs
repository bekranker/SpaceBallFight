using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class SliderEffect : MonoBehaviour
{
    [SerializeField] private Transform _Transform;
    [SerializeField] private float _PunchValue, _Speed;
    private Vector2 _startScale;
    private bool _canPunch;


    private void Start()
    {
        _canPunch = true;
        _startScale = _Transform.localScale;
    }

    public void DoEffect()
    {
        if (!_canPunch) return;
        _Transform.DOPunchScale(_PunchValue * Vector3.one, _Speed).OnComplete(()=> 
        {
            _Transform.localScale = _startScale;
            _canPunch = true;
        });
        _canPunch = false;
    }
}
