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
                _BossTag.TakeDamage(10, _t, "BulletRed");
                _ObjectPool.GiveBullet(bullet);
                break;
            case "BulletBlue":
                _BossTag.TakeDamage(10, _t, "BulletBlue");
                _ObjectPool.GiveBullet(bullet);
                break;
            case "BulletGreen":
                _BossTag.TakeDamage(10, _t, "BulletGreen");
                _ObjectPool.GiveBullet(bullet);
                break;
            case "SBulletRed":
                _BossTag.TakeDamage(50, bullet.transform, "BulletRed");
                Destroy(bullet);
                break;
            case "SBulletBlue":
                _BossTag.TakeDamage(50, bullet.transform, "BulletBlue");
                StartCoroutine(_BossTag.Slow());
                Destroy(bullet);
                break;
            case "SBulletGreen":
                _BossTag.TakeDamage(50, bullet.transform, "BulletGreen");
                Destroy(bullet);
                break;
            default:
                break;
        }
    }
}
