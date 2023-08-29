using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueAttack : MonoBehaviour
{
    [SerializeField] private SpinBoss _SpinBoss;
    [SerializeField] private GameObject _BulletPrefabForBlue;
    [SerializeField] private BossFightCreateEnemy _BossFightCreateEnemy;
    [SerializeField] private float _BulletSpeed, _BulletCountForEachPoint;
    [SerializeField] private BossPlayerFollow _BossPlayerFollow;
    [SerializeField] private Transform _SpawnPoint;
    [SerializeField] private BossAttackManager _BossAttackManager;
    private WaitForSeconds _shootDelayForBlue = new WaitForSeconds(0.1f);
    private WaitForSeconds _blueWait = new WaitForSeconds(3);
    private Transform _player;
    private float _startSpinSpeed;
    private void Start()
    {
        _startSpinSpeed = _SpinBoss._SpinSpeed;
        _player = FindObjectOfType<PlayerController>().transform;
        repeate();
    }

    private void repeate()
    {
        if (!_BossAttackManager.CanFight) return;
        StartCoroutine(ShootIE());
    }
    private IEnumerator ShootIE()
    {
        _BossPlayerFollow.CanFollow = false;
        yield return _blueWait;
        Audio.PlayAudio($"BossShootBGNoise", .1f);
        _SpinBoss._SpinSpeed *= 2;
        for (int i = 0; i < _BulletCountForEachPoint; i++)
        {
            yield return _shootDelayForBlue;
            Rigidbody2D rb = Instantiate(_BulletPrefabForBlue, _SpawnPoint.position, _SpawnPoint.rotation).GetComponent<Rigidbody2D>();
            PushBulet(rb);
        }
        yield return _blueWait;
        _SpinBoss._SpinSpeed *= 1.2f;
        _BossFightCreateEnemy.SpawnRandomEnemy(Random.Range(5, 10), .5f, _SpawnPoint.position);
        yield return _blueWait;
        _SpinBoss._SpinSpeed = _startSpinSpeed;
        _BossPlayerFollow.CanFollow = true;
        repeate();
    }
    private void PushBulet(Rigidbody2D rb)
    {
        rb.velocity = _BulletSpeed * rb.transform.up;
    }
}
