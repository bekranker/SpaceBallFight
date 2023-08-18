using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectableXPMovement : MonoBehaviour
{
    [SerializeField] private float _FromSpeed, _ToSpeed;
    [SerializeField] private Transform _Target;
    private bool _canFollow;
    private float randForFirstSplitting, _randSpeed;
    private Transform _t;
    private void Start()
    {
        _t = transform;
        _Target = FindObjectOfType<PlayerController>().transform;
        GoAway();
    }
    void Update()
    {
        FollowPlayer();
    }
    private void FollowPlayer()
    {
        if (!_canFollow) return;
        _randSpeed = Random.Range(_FromSpeed, _ToSpeed);
        _t.position = Vector3.MoveTowards(_t.position, _Target.position, _randSpeed * Time.deltaTime);        
    }
    private void GoAway()
    {
        randForFirstSplitting = Random.Range(2f, 3f);
        _t.DOMove(1.2f * _t.up + _t.position, randForFirstSplitting).SetEase(Ease.OutSine).OnComplete(() => _canFollow = true);
    }
}