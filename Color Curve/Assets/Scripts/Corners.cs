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
}
