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
    [SerializeField] private List<Transform> _SpawnPoint;

    private WaitForSeconds _changeAttackDelay = new WaitForSeconds(5);
    private WaitForSeconds _sleepTimeFirst = new WaitForSeconds(2);
    private WaitForSeconds _shootDelay = new WaitForSeconds(.01f);
    private float _firstSpeed;
    private Transform _t;
    private void Start()
    {
        _firstSpeed = _SpinBoss._SpinSpeed;
        Attack();
        _t = transform;
    }
    private void Attack()
    {
        StartCoroutine(AttackIE());
    }
    private IEnumerator AttackIE()
    {
        yield return _sleepTimeFirst;
        _SpinBoss._SpinSpeed += 1.2f;
        _BossPlayerFollow.CanFollow = false;
        //---------------------------------------Cut---------------------------------------
        StartCoroutine(ShootIE());
        Audio.PlayAudio($"BossShootBGNoise", .1f, "General", "Sound");
        yield return _changeAttackDelay;
        _SpinBoss._SpinSpeed += 1.3f;
        createEnemys();
        yield return _changeAttackDelay;
        createEnemys();
        _SpinBoss._SpinSpeed = _firstSpeed;
        _BossPlayerFollow.CanFollow = true;
        Attack();
    }
    private IEnumerator ShootIE()
    {
        for (int i = 0; i < _BulletCountForEachPoint; i++)
        {
            yield return _shootDelay;
            float angle = i * 45;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Rigidbody2D rb = Instantiate(_BulletPrefab, _t.position, rotation).GetComponent<Rigidbody2D>();
            PushBulet(rb);
        }
    }
    private void createEnemys()
    {
        _BossFightCreateEnemy.SpawnRandomEnemy(7, .35f, _SpawnPoint);
    }

    private void PushBulet(Rigidbody2D rb)
    {
        rb.velocity = _BulletSpeed * rb.transform.right;
    }
}