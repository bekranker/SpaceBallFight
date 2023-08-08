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
                _BossTag.TakeDamage(Random.Range(2,5), _t, "BulletRed");
                break;
            case "BulletBlue":
                _BossTag.TakeDamage(Random.Range(0, 5), _t, "BulletBlue");
                break;
            case "BulletGreen":
                _BossTag.TakeDamage(Random.Range(0, 5), _t, "BulletGreen");
                break;
            default:
                break;
        }
        _ObjectPool.GiveBullet(bullet);
    }
}
