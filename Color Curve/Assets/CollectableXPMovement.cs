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
    private void Start()
    {
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
        transform.position = Vector3.MoveTowards(transform.position, _Target.position, _randSpeed * Time.deltaTime);        
    }
    private void GoAway()
    {
        randForFirstSplitting = Random.Range(0.5f, 1f);
        transform.DOMove(1.2f * transform.up + transform.position, randForFirstSplitting).SetEase(Ease.OutSine).OnComplete(() => _canFollow = true);
    }
}
