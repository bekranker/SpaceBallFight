using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialSkill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FreezeBullet")||
            collision.gameObject.CompareTag("BossBullet")
            )
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
