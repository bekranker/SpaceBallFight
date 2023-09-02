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
    [SerializeField] private List<Transform> _SpawnPoints;
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
        _BossPlayerFollow.CanFollow = true;
        yield return _blueWait;
        _BossPlayerFollow.CanFollow = false;
        Audio.PlayAudio($"BossShootBGNoise", .1f);
        _SpinBoss._SpinSpeed *= 1.1f;
        for (int i = 0; i < _BulletCountForEachPoint; i++)
        {
            yield return _shootDelayForBlue;
            float angle = i * 45;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Rigidbody2D rb = Instantiate(_BulletPrefabForBlue, _SpawnPoint.position, rotation).GetComponent<Rigidbody2D>();
            PushBulet(rb);
        }
        _BossFightCreateEnemy.SpawnRandomEnemy(6, .5f, _SpawnPoints);
        _SpinBoss._SpinSpeed *= 1.3f;
        yield return _blueWait;
        _BossFightCreateEnemy.SpawnRandomEnemy(6, .35f, _SpawnPoints);
        _SpinBoss._SpinSpeed = _startSpinSpeed;
        _BossPlayerFollow.CanFollow = true;
        repeate();
    }
    private void PushBulet(Rigidbody2D rb)
    {
        rb.velocity = _BulletSpeed * rb.transform.up;
    }
}