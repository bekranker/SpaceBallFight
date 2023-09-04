using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GhostAttack : MonoBehaviour
{
    [SerializeField] private BossFightCreateEnemy _BossFightCreateEnemy;
    [SerializeField] private List<Transform> _Spawner;
    [SerializeField] private BossAttackManager _BossAttackManager;
    [SerializeField] private BossTag _BossTag;
    [SerializeField] private Collider2D _Collider;
    [SerializeField] private GameObject _BulletPrefab;
    [SerializeField] private int _ShootAttackCounter;
    [SerializeField] private float _BulletSpeed;
    [SerializeField] private BossPlayerFollow _BossPlayerFollow;
    private WaitForSeconds _sleepTime = new WaitForSeconds(5);
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
        _BossFightCreateEnemy.SpawnRandomEnemy(5, .35f, _Spawner);
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
        Rigidbody2D rb = Instantiate(_BulletPrefab, _Spawner[0].position, Quaternion.identity).GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * _BulletSpeed * 100 * Time.unscaledDeltaTime;
        switch (_BossTag.EnemyColor)
        {
            case EnemyColor.Red:
                rb.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case EnemyColor.Green:
                rb.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case EnemyColor.Blue:
                rb.GetComponent<SpriteRenderer>().color = _BossTag._ColorBlue;
                break;
            default:
                break;
        }
    }
}
