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
    private Transform _t;


    private void Start()
    {
        _t = transform;
        _playerController = FindObjectOfType<PlayerController>();
        _firstSpeed = _EnemyManager.Speed;
        _objectPool = FindObjectOfType<ObjectPool>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var direction = _t.position - collision.gameObject.transform.position;


        if (collision.CompareTag("Shield"))
        {
            _EnemyManager.TakeDamage(999, collision.gameObject.transform);
            return;
        }
        switch (collision.tag)
        {
            case "BulletRed":
                if(_EnemyManager.EnemyColorTypes == EnemyColor.Red)
                    TakeDamage(collision, 10 * _playerController.DamageMultipilier);
                else
                    TakeDamage(collision, 1 * _playerController.DamageMultipilier);
                break;
            case "BulletBlue":
                if (_EnemyManager.EnemyColorTypes == EnemyColor.Blue)
                    TakeDamage(collision, 10 * _playerController.DamageMultipilier);
                else
                    TakeDamage(collision, 1 * _playerController.DamageMultipilier);
                break;
            case "BulletGreen":
                if (_EnemyManager.EnemyColorTypes == EnemyColor.Green)
                    TakeDamage(collision, 10 * _playerController.DamageMultipilier);
                else
                    TakeDamage(collision, 1 * _playerController.DamageMultipilier);
                break;
            default:
                break;
        }
        
        TakeDamageFromSpecialBullets(collision, direction);
    }
    private void TakeDamageFromSpecialBullets(Collider2D collision, Vector2 pos)
    {
        switch (collision.tag)
        {
            case "SBulletRed":
                _EnemyManager.TakeDamage(50, collision.gameObject.transform);
                break;
            case "SBulletBlue":
                _EnemyManager.TakeDamage(25, collision.gameObject.transform);
                StartCoroutine(delay());
                break;
            case "SBulletGreen":
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