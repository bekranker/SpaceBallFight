using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleManager : EnemyMovementAbstract
{
    [HideInInspector] public PlayerController _PlayerController;
    [SerializeField] private float _Speed;
    public override void OnUpdate(AbstractmovementManager abstractmovementManager)
    {
        LookToPlayer();
    }

    public override void OnStart(AbstractmovementManager abstractmovementManager)
    {
        _PlayerController = FindObjectOfType<PlayerController>();
    }

    public override void OnLateUpdate(AbstractmovementManager abstractmovementManager)
    {
        FollowCurrentPlayer();
    }

    private void LookToPlayer() => transform.up = _PlayerController.gameObject.transform.position - transform.position;
    private void FollowCurrentPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, _PlayerController.gameObject.transform.position, _Speed * Time.deltaTime);
    }

    public override void TriggerEnter2D(AbstractmovementManager abstractmovementManager, Collider2D collision)
    {
    }
}
