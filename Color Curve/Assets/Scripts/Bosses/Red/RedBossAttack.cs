using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBossAttack : MonoBehaviour
{
    [SerializeField] private SpinBoss _SpinBoss;
    [SerializeField] private GameObject _BulletPrefab;
    [SerializeField] private BossFightCreateEnemy _BossFightCreateEnemy;
    [SerializeField] private float _BulletSpeed, _BulletCountForEachPoint;
    [SerializeField] private BossPlayerFollow _BossPlayerFollow;
    [SerializeField] private Transform _SpawnPoint;

    private WaitForSeconds _sleepTime = new WaitForSeconds(5);
    private WaitForSeconds _sleepTimeFirst = new WaitForSeconds(2);
    private WaitForSeconds _attackDelay = new WaitForSeconds(.1f);
    private WaitForSeconds _attackDelay2 = new WaitForSeconds(.01f);
    private float _firstSpeed;
    private Transform _t;
    private Vector3 _p;
    private void Start()
    {
        _firstSpeed = _SpinBoss._SpinSpeed;
        Attack();
        _t = transform;
    }
    private void Update()
    {
        _p = _SpawnPoint.position;
    }
    private void Attack()
    {
        StartCoroutine(AttackIE());
    }
    private IEnumerator AttackIE()
    {
        yield return _sleepTimeFirst;
        _SpinBoss._SpinSpeed *= 1.5f;
        _BossPlayerFollow.CanFollow = false;
        //---------------------------------------Cut---------------------------------------
        StartCoroutine(ShootIE());
        yield return _sleepTime;
        _SpinBoss._SpinSpeed *= 1.2f;
        createEnemys();
        yield return _sleepTime;
        _SpinBoss._SpinSpeed = _firstSpeed;
        _BossPlayerFollow.CanFollow = true;
        Attack();
    }
    private IEnumerator ShootIE()
    {
        for (int i = 0; i < _BulletCountForEachPoint; i++)
        {
            yield return _attackDelay2;
            float angle = i * 22.5f;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Rigidbody2D rb = Instantiate(_BulletPrefab, _t.position, rotation).GetComponent<Rigidbody2D>();
            PushBulet(rb);
        }
    }
    private void createEnemys()
    {
        _BossFightCreateEnemy.SpawnRandomEnemy(Random.Range(5, 10), .25f, _p);
    }

    private void PushBulet(Rigidbody2D rb)
    {
        rb.velocity = _BulletSpeed * rb.transform.right;
    }
}