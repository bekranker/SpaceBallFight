using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicBullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem _ToxicParticle;
    [SerializeField] private LayerMask _AfectedLayers;
    private bool _can;
    private Transform _t;

    private void Start()
    {
        _t = transform;
        _can = true;
        StartCoroutine(effect());
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
    private IEnumerator effect()
    {
        yield return new WaitForSeconds(4.5f);
        ParticleSystem.MainModule mainPart = _ToxicParticle.main;
        mainPart.startLifetime = 0;
    }
}
