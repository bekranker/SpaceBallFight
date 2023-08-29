using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlayerFollow : MonoBehaviour
{

    public bool CanFollow;

    [SerializeField] private float _Speed;

    private Transform _player;
    private Transform _t;


    private void Start()
    {
        _t = transform;
        CanFollow = true;
        _player = FindAnyObjectByType<PlayerController>().transform;
    }

    private void LateUpdate()
    {
        FollowThePlayer();
    }
    private void FollowThePlayer()
    {
        if (!CanFollow) return;
        _t.position = Vector2.MoveTowards(_t.position, _player.position, _Speed * Time.deltaTime);
    }
}
