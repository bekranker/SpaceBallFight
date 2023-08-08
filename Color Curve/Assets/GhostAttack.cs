using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAttack : MonoBehaviour
{
    [SerializeField] private BossFightCreateEnemy _BossFightCreateEnemy;
    [SerializeField] private Transform _Spawner;
    [SerializeField] private BossAttackManager _BossAttackManager;
    [SerializeField] private BossTag _BossTag;
    [SerializeField] private Collider2D _Collider;
    [SerializeField] private GameObject _BulletPrefab;
    [SerializeField] private int _ShootAttackCounter;
    [SerializeField] private float _BulletSpeed;
    [SerializeField] private BossPlayerFollow _BossPlayerFollow;
    private WaitForSeconds _sleepTime = new WaitForSeconds(3);
    private WaitForSeconds _sleepTimeForShoot = new WaitForSeconds(.1f);
    private Transform _playerT;
    private Transform _t;
    private List<EnemyColor> _EnemyColor = new List<EnemyColor>
    {
        EnemyColor.Red,
        EnemyColor.Green,
        EnemyColor.Blue,
    };


    void Start()
    {
        _playerT = FindObjectOfType<PlayerController>().transform;
        _t = transform;
    }
    private void OnEnable()
    {
        BossAttackManager.AttackEventStart += Attack;
    }
    private void OnDisable()
    {
        BossAttackManager.AttackEventStart -= Attack;
    }
    private void Attack()
    {
        if (_BossAttackManager.CanFight)
        {
            StartCoroutine(AttackIE());
        }
    }
    private IEnumerator AttackIE()
    {
        _BossTag.Setcolor(_EnemyColor[Random.Range(0, _EnemyColor.Count)]);
        _BossTag._SpriteRenderer.ForEach((_sprite) => 
        {
            _sprite.DOFade(.25f, 2.5f);
        });
        _Collider.enabled = false;
        _BossFightCreateEnemy.SpawnRandomEnemy(Random.Range(5, 10), .25f, _Spawner.position);
        _BossPlayerFollow.enabled = true;
        yield return _sleepTime;
        _BossTag._SpriteRenderer.ForEach((_sprite) =>
        {
            _sprite.DOFade(1, 2.5f);
        });
        _Collider.enabled = true;
        _BossPlayerFollow.enabled = false;
        for (int i = 0; i < _ShootAttackCounter; i++)
        {
            Shoot();
            yield return _sleepTimeForShoot;
        }
        Attack();
    }
    private void Shoot()
    {
        var direction = _playerT.position - _t.position;
        Rigidbody2D rb = Instantiate(_BulletPrefab, _Spawner.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        rb.velocity = direction * _BulletSpeed * 100 * Time.deltaTime;
    }
}
