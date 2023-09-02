using System.Collections;
using UnityEngine;

public class ToxicBullet : MonoBehaviour
{
    [SerializeField] private LayerMask _AfectedLayers;
    [SerializeField] private float _Size;
    private bool _can;
    private Transform _t;

    private void Start()
    {
        _t = transform;
        _can = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _Size);
    }
    void Update()
    {
        if (!_can) return;
        if (Physics2D.OverlapCircle(_t.position, _Size, _AfectedLayers))
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(_t.position, _Size, _AfectedLayers);
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].TryGetComponent(out EnemyManager enemy))
                {
                    enemy.TakeDamage(10, _t, false);
                }
                if (cols[i].TryGetComponent(out BossTag boss))
                {
                    boss.TakeDamage(150, _t, "BulletGreen");
                }
            }
            StartCoroutine(damageDelay());
        }
    }
    private IEnumerator damageDelay()
    {
        _can = false;
        yield return new WaitForSeconds(.25f);
        _can = true;
    }
}