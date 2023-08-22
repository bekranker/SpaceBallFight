using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDedection : MonoBehaviour
{
    private ObjectPool _objectPool;
    [SerializeField] private EnemyManager _EnemyManager;
    private float _firstSpeed;
    private WaitForSeconds _delay = new WaitForSeconds(2f);
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _firstSpeed = _EnemyManager.Speed;
        _objectPool = FindObjectOfType<ObjectPool>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shield"))
        {
            _objectPool.TakeParticle(collision.gameObject.transform.position);
            _EnemyManager.TakeDamage(999, collision.gameObject.transform);
            return;
        }
        switch (collision.tag)
        {
            case "BulletRed":
                _objectPool.TakeParticle(collision.gameObject.transform.position);
                if(_EnemyManager.EnemyColorTypes == EnemyColor.Red)
                    TakeDamage(collision, 10 * _playerController.DamageMultipilier);
                else
                    TakeDamage(collision, 1 * _playerController.DamageMultipilier);
                break;
            case "BulletBlue":
                _objectPool.TakeParticle(collision.gameObject.transform.position);
                if (_EnemyManager.EnemyColorTypes == EnemyColor.Blue)
                    TakeDamage(collision, 10 * _playerController.DamageMultipilier);
                else
                    TakeDamage(collision, 1 * _playerController.DamageMultipilier);
                break;
            case "BulletGreen":
                _objectPool.TakeParticle(collision.gameObject.transform.position);
                if (_EnemyManager.EnemyColorTypes == EnemyColor.Green)
                    TakeDamage(collision, 10 * _playerController.DamageMultipilier);
                else
                    TakeDamage(collision, 1 * _playerController.DamageMultipilier);
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
                _EnemyManager.TakeDamage(50, collision.gameObject.transform);
                break;
            case "SBulletBlue":
                _objectPool.TakeParticle(collision.gameObject.transform.position);
                _EnemyManager.TakeDamage(25, collision.gameObject.transform);
                StartCoroutine(delay());
                break;
            case "SBulletGreen":
                _objectPool.TakeParticle(collision.gameObject.transform.position);
                _EnemyManager.TakeDamage(15, collision.gameObject.transform);
                break;
            default:
                break;
        }
    }
    private void TakeDamage(Collider2D collision, int damageCount)
    {
        _objectPool.GiveBullet(collision.gameObject);
        _EnemyManager.TakeDamage(damageCount, collision.gameObject.transform);
    }
    private IEnumerator delay()
    {
        _EnemyManager.Speed /= 2;
        yield return _delay;
        _EnemyManager.Speed = _firstSpeed;
    }
}
