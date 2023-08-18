using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicBullet : MonoBehaviour
{
    [SerializeField] private LayerMask _AfectedLayers;
    private bool _can;
    private Transform _t;

    private void Start()
    {
        _t = transform;
        _can = true;
        Destroy(gameObject, 5);
    }
    void Update()
    {
        if (!_can) return;
        if (Physics2D.OverlapCircle(_t.position, _t.localScale.x, _AfectedLayers))
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(_t.position, _t.localScale.x, _AfectedLayers);
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].TryGetComponent(out EnemyManager enemy))
                {
                    StartCoroutine(damageDelay());
                    enemy.TakeDamage(15, _t);
                }
                if (cols[i].TryGetComponent(out BossTag boss))
                {
                    boss.TakeDamage(150, _t, "BulletGreen");
                }
            }
        }
    }

    private IEnumerator damageDelay()
    {
        _can = false;
        yield return new WaitForSeconds(1);
        _can = true;
    }
}
