using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SquareManager : EnemyMovementAbstract
{
    [HideInInspector] public PlayerController _PlayerController;
    [SerializeField] private Transform BodyOne, BodyTwo;
    [SerializeField] private Rigidbody2D _Rb;
    [SerializeField] EnemyManager _EnemyManager;

    private Transform _t, _playerT;
    private bool _canDash, _canFollow, _canD;
    private Vector3 _to;
    private WaitForSeconds _delay = new WaitForSeconds(.5f);


    public override void OnUpdate(AbstractmovementManager abstractmovementManager)
    {
        LookToPlayer();
    }

    public override void OnStart(AbstractmovementManager abstractmovementManager)
    {
        _t = transform;
        _canD = true;
        _canDash = true;
        _canFollow = true;
        _PlayerController = FindObjectOfType<PlayerController>();
        _playerT = _PlayerController.transform;
    }

    public override void OnLateUpdate(AbstractmovementManager abstractmovementManager)
    {
        FollowCurrentPlayer();
    }

    private void LookToPlayer() => _t.up = _playerT.position - _t.position;
    private void FollowCurrentPlayer()
    {
        if (Vector2.Distance(_t.position, _playerT.position) > 5 && _canFollow)
        {
            TurnBody();
            _t.position = Vector3.MoveTowards(_t.position, _playerT.position, _EnemyManager.Speed * Time.deltaTime);
        }
        else if(!_canFollow)
        {
           if (_canDash)
           {
                _canDash = false;
                StartCoroutine(dashToPlayer());
           }
        }
        if (Vector2.Distance(_t.position, _playerT.position) > 5 && _canFollow)
        {
            _canDash = false;
            _canFollow = true;
            _canD = true;
        }
        else
        {
            _canDash = true;
            _canFollow = false;
        }
    }

    private IEnumerator dashToPlayer()
    {
        yield return _delay;
        if (_canD)
        {
            _to = _playerT.position;
            _canD = false;
        }
        if(_t == null)
        {
            yield break;
        }
        _t.DOMove(_to, 1f).OnComplete(()=> _canFollow = true);
    }

    public override void TriggerEnter2D(AbstractmovementManager abstractmovementManager, Collider2D collision)
    {
    }
    private void TurnBody()
    {
        if (BodyOne != null)
            BodyOne.transform.Rotate(0, 0, 90 * Time.deltaTime * -2.5f);
        if(BodyTwo != null)
            BodyTwo.transform.Rotate(0, 0, 90 * Time.deltaTime * 2.5f);
    }
}
