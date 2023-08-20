using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HexagonManager : EnemyMovementAbstract
{
    [HideInInspector] public PlayerController _PlayerController;
    [SerializeField] private Transform BodyOne, BodyTwo;
    [SerializeField] EnemyManager _EnemyManager;

    private Transform _t;
    public override void OnStart(AbstractmovementManager abstractmovementManager)
    {
        _t = transform;
        _PlayerController = FindObjectOfType<PlayerController>();
        LookToPlayer();
    }
    public override void OnUpdate(AbstractmovementManager abstractmovementManager) 
    {
        _t.position += _EnemyManager.Speed * Time.deltaTime * _t.up;
        TurnBody();
    }
    public override void OnLateUpdate(AbstractmovementManager abstractmovementManager) { }
    private void LookToPlayer() => transform.up = new Vector3(_PlayerController.transform.position.x, _PlayerController.transform.position.y, 0) - new Vector3(_t.position.x, _t.position.y, 0);
    public override void TriggerEnter2D(AbstractmovementManager abstractmovementManager, Collider2D collision)
    {
        BounceFromEdge();
    }
    private void BounceFromEdge()
    {
        LookToPlayer();
    }
    private void TurnBody()
    {
        if (BodyOne != null)
            BodyOne.Rotate(0, 0, 90 * Time.deltaTime * -2.5f);
        if (BodyTwo != null)
            BodyTwo.Rotate(0, 0, 90 * Time.deltaTime * 2.5f);
    }

}