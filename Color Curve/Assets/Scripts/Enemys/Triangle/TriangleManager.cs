using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleManager : EnemyMovementAbstract
{
    [HideInInspector] public PlayerController _PlayerController;
    [SerializeField] EnemyManager _EnemyManager;
    private Transform _t;
    public override void OnUpdate(AbstractmovementManager abstractmovementManager)
    {
        LookToPlayer();
    }

    public override void OnStart(AbstractmovementManager abstractmovementManager)
    {
        _t = transform;
        _PlayerController = FindObjectOfType<PlayerController>();
    }

    public override void OnLateUpdate(AbstractmovementManager abstractmovementManager)
    {
        FollowCurrentPlayer();
    }

    private void LookToPlayer() => _t.up = _PlayerController.transform.position - _t.position;
    private void FollowCurrentPlayer()
    {
        _t.position = Vector3.MoveTowards(_t.position, _PlayerController.transform.position, _EnemyManager.Speed * Time.deltaTime);
    }

    public override void TriggerEnter2D(AbstractmovementManager abstractmovementManager, Collider2D collision)
    {
    }
}
