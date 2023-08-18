using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDedection : MonoBehaviour
{
    private ObjectPool _objectPool;
    [SerializeField] private EnemyManager _EnemyManager;

    private void Start()
    {
        _objectPool = FindObjectOfType<ObjectPool>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "BulletRed":
                _objectPool.TakeParticle(collision.gameObject.transform.position);
                if(_EnemyManager.EnemyColorTypes == EnemyColor.Red)
                    TakeDamage(collision, 10);
                else
                    TakeDamage(collision, 1);
                break;
            case "BulletBlue":
                _objectPool.TakeParticle(collision.gameObject.transform.position);
                if (_EnemyManager.EnemyColorTypes == EnemyColor.Blue)
                    TakeDamage(collision, 10);
                else
                    TakeDamage(collision, 1);
                break;
            case "BulletGreen":
                _objectPool.TakeParticle(collision.gameObject.transform.position);
                if (_EnemyManager.EnemyColorTypes == EnemyColor.Green)
                    TakeDamage(collision, 10);
                else
                    TakeDamage(collision, 1);
                break;
            default:
                break;
        }
        TakeDamageFromSpecialBullets(collision);
    }
    private void TakeDamageFromSpecialBullets(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "SBulletRed":
                _objectPool.TakeParticle(collision.gameObject.transform.position);
                if (_EnemyManager.EnemyColorTypes == EnemyColor.Red)
                    TakeDamage(collision, 50);
                else
                    TakeDamage(collision, 1);
                break;
            case "SBulletBlue":
                _objectPool.TakeParticle(collision.gameObject.transform.position);
                if (_EnemyManager.EnemyColorTypes == EnemyColor.Blue)
                    TakeDamage(collision, 25);
                else
                    TakeDamage(collision, 1);
                break;
            case "SBulletGreen":
                _objectPool.TakeParticle(collision.gameObject.transform.position);
                if (_EnemyManager.EnemyColorTypes == EnemyColor.Green)
                    TakeDamage(collision, 15);
                else
                    TakeDamage(collision, 1);
                break;
            default:
                break;
        }
    }
    private void TakeDamage(Collider2D collision, int damageCount)
    {
        if( collision.gameObject.CompareTag("BulletRed") ||
            collision.gameObject.CompareTag("BulletGreen") ||
            collision.gameObject.CompareTag("BulletBlue"))
        _objectPool.GiveBullet(collision.gameObject);
        _EnemyManager.TakeDamage(damageCount, collision.gameObject.transform);
    }
}
