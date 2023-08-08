using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenAttack : MonoBehaviour
{
    [SerializeField] private SpinBoss _SpinBoss;
    [SerializeField] private GameObject _BulletPrefab;
    [SerializeField] private BossFightCreateEnemy _BossFightCreateEnemy;
    [SerializeField] private float _BulletSpeed, _BulletCountForEachPoint;
    [SerializeField] private BossRandomMovement _BossRandomMovement;
    [SerializeField] private Transform _SpawnPoint;
    [SerializeField] private BossAttackManager _BossAttackManager;
    private WaitForSeconds _attackDelay = new WaitForSeconds(0.1f);
    private WaitForSeconds _attackDelay2 = new WaitForSeconds(3);
    private WaitForSeconds _attackDelay3 = new WaitForSeconds(3);
    private Transform _player;
    private bool _can;

    private void Start()
    {
        _can = true;
        _player = FindObjectOfType<PlayerController>().transform;
        InvokeRepeating("repeate", 0, .5f);
    }
    
    private void repeate()
    {
        if (!_BossAttackManager.CanFight) return;
        if (!_can) return;
        StartCoroutine(ShootIE());
        _can = false;
    }
    private IEnumerator ShootIE()
    {
        _BossAttackManager.CanFight = false;
        yield return _attackDelay2;
        for (int i = 0; i < _BulletCountForEachPoint; i++)
        {
            yield return _attackDelay;
            Rigidbody2D rb = Instantiate(_BulletPrefab, _SpawnPoint.position, _SpawnPoint.rotation).GetComponent<Rigidbody2D>();
            PushBulet(rb);
        }
        yield return _attackDelay2;
        _BossFightCreateEnemy.SpawnRandomEnemy(Random.Range(5, 10), .5f, _SpawnPoint.position);
        yield return _attackDelay3;
        _BossRandomMovement.CanMove = true;
        repeate();
        _can = true;
    }
    private void PushBulet(Rigidbody2D rb)
    {
        rb.velocity = _BulletSpeed * rb.transform.right;
    }
}