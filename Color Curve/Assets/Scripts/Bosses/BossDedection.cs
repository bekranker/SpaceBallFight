using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDedection : MonoBehaviour
{
    [SerializeField] private BossTag _BossTag;
    private ObjectPool _ObjectPool;
    private Transform _t;


    private void Start()
    {
        _ObjectPool = GameObject.FindGameObjectWithTag("Pool").GetComponent<ObjectPool>();
        _t = transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BulletDedection(collision.gameObject.tag, collision.gameObject);
    }
    private void BulletDedection(string bulletTag, GameObject bullet)
    {
        switch (bulletTag)
        {
            case "BulletRed":
                if (_BossTag.EnemyColor == EnemyColor.Red)
                {
                    _BossTag.TakeDamage(Random.Range(200, 500), _t, "BulletRed");
                }
                else
                {
                    _BossTag.TakeDamage(Random.Range(20,50), _t, "BulletRed");
                }
                _ObjectPool.GiveBullet(bullet);
                break;
            case "BulletBlue":
                if (_BossTag.EnemyColor == EnemyColor.Red)
                {
                    _BossTag.TakeDamage(Random.Range(200, 500), _t, "BulletBlue");
                }
                else
                {
                    _BossTag.TakeDamage(Random.Range(20, 50), _t, "BulletBlue");
                }
                _ObjectPool.GiveBullet(bullet);
                break;
            case "BulletGreen":
                if (_BossTag.EnemyColor == EnemyColor.Red)
                {
                    _BossTag.TakeDamage(Random.Range(200, 500), _t, "BulletGreen");
                }
                else
                {
                    _BossTag.TakeDamage(Random.Range(20, 50), _t, "BulletGreen");
                }
                _ObjectPool.GiveBullet(bullet);
                break;
            case "SBulletRed":
                _BossTag.TakeDamage(1250, bullet.transform, "BulletRed");
                break;
            case "SBulletBlue":
                _BossTag.TakeDamage(1250, bullet.transform, "BulletBlue");
                StartCoroutine(_BossTag.Slow());
                break;
            case "SBulletGreen":
                _BossTag.TakeDamage(1250, bullet.transform, "BulletGreen");
                break;
            default:
                break;
        }
    }
}
