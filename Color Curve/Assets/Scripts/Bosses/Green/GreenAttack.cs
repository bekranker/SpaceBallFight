using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenAttack : MonoBehaviour
{
    [SerializeField] private SpinBoss _SpinBoss;
    [SerializeField] private GameObject _BulletPrefab;
    [SerializeField] private BossFightCreateEnemy _BossFightCreateEnemy;
    [SerializeField] private float _BulletSpeed, _BulletCountForEachPoint;
    [SerializeField] private BossPlayerFollow _BossPlayerFollow;
    [SerializeField] private Transform _SpawnPoint;
    [SerializeField] private List<Transform> _SpawnPointBullet;
    [SerializeField] private BossAttackManager _BossAttackManager;
    private WaitForSeconds _attackDelay = new WaitForSeconds(0.1f);
    private WaitForSeconds _attackDelay2 = new WaitForSeconds(3);
    private Transform _player;
    private bool _can;
    private float _startSpinSpeed;



    private void Start()
    {
        _startSpinSpeed = _SpinBoss._SpinSpeed;
        _can = true;
        _player = FindObjectOfType<PlayerController>().transform;
        InvokeRepeating("repeate", 0, .5f);
        _BossFightCreateEnemy.SpawnRandomEnemy(5, .35f, _SpawnPointBullet);
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
        _BossPlayerFollow.CanFollow = false;
        Audio.PlayAudio($"BossShootBGNoise", .1f);
        _SpinBoss._SpinSpeed *= 2;
        for (int i = 0; i < _BulletCountForEachPoint; i++)
        {
            yield return _attackDelay;
            Rigidbody2D rb = Instantiate(_BulletPrefab, _SpawnPoint.position, _SpawnPoint.rotation).GetComponent<Rigidbody2D>();
            PushBulet(rb);
        }
        yield return _attackDelay2;
        _SpinBoss._SpinSpeed *= 1.2f;
        _BossFightCreateEnemy.SpawnRandomEnemy(6, .3f, _SpawnPointBullet);
        yield return _attackDelay2;
        _SpinBoss._SpinSpeed = _startSpinSpeed;
        _BossPlayerFollow.CanFollow = true;
        _BossAttackManager.CanFight = true;
        repeate();
        _can = true;
    }
    private void PushBulet(Rigidbody2D rb)
    {
        rb.velocity = _BulletSpeed * rb.transform.right;
    }
}