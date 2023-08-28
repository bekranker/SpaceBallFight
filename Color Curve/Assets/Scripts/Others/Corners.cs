using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corners : MonoBehaviour
{
    [SerializeField] private ObjectPool _ObjectPool;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BulletWhite") ||
            collision.CompareTag("BulletGreen") ||
            collision.CompareTag("BulletBlue") ||
            collision.CompareTag("BulletRed"))
        {
            _ObjectPool.GiveBullet(collision.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BulletWhite") ||
            collision.gameObject.CompareTag("BulletGreen") ||
            collision.gameObject.CompareTag("BulletBlue") ||
            collision.gameObject.CompareTag("BulletRed"))
        {
            _ObjectPool.GiveBullet(collision.gameObject);
        }
    }
}
