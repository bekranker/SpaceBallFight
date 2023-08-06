using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class StupidBossAttack : MonoBehaviour
{
    [SerializeField] private Animator _Animator;
    [SerializeField] private BossAttackManager _BossAttackManager;

    private void OnEnable()
    {
        BossAttackManager.AttackEvent += AttackToPlayer;
    }
    private void OnDisable()
    {
        BossAttackManager.AttackEvent -= AttackToPlayer;
    }
    private bool _canMove;
    private WaitForSecondsRealtime _sleep = new WaitForSecondsRealtime(2);
    private List<Vector2> _Positions = new List<Vector2>
    {
        new Vector2(0, 6),
        new Vector2(0, 0),
        new Vector2(0, -6)
    };
    private int _positionIndex;
    [SerializeField] Transform _t;

    public void AttackToPlayer()
    {
        if (!_BossAttackManager.CanFight) return;
        StartCoroutine(AttackIE());
    }
    private IEnumerator AttackIE()
    {
        _t.DOMove(_Positions[_positionIndex], 1);
        yield return _sleep;
        _Animator.SetTrigger("Attack");
        _positionIndex = (_positionIndex + 1 < _Positions.Count) ? _positionIndex + 1 : 0;
    }
}
