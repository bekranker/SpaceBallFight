using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HexagonManager : EnemyMovementAbstract
{
    [HideInInspector] public PlayerController _PlayerController;
    [SerializeField] private Transform BodyOne, BodyTwo;
    [SerializeField] EnemyManager _EnemyManager;

    private Transform _t, _playerT;
    public override void OnStart(AbstractmovementManager abstractmovementManager)
    {
        _t = transform;
        _PlayerController = FindObjectOfType<PlayerController>();
        if (_PlayerController != null)
        {
            _playerT = _PlayerController.transform;
        }
        LookToPlayer();
    }
    public override void OnUpdate(AbstractmovementManager abstractmovementManager) 
    {
        _t.position += _EnemyManager.Speed * Time.deltaTime * _t.up;
        TurnBody();
    }
    public override void OnLateUpdate(AbstractmovementManager abstractmovementManager) { }
    private void LookToPlayer() => _t.up = new Vector3(_playerT.position.x, _playerT.position.y, 0) - new Vector3(_t.position.x, _t.position.y, 0);
    public override void TriggerEnter2D(AbstractmovementManager abstractmovementManager, Collider2D collision)
    {
        BounceFromEdge();
    }
    private void BounceFromEdge()
    {
        if(_playerT != null)
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