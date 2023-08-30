using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Toxic : MonoBehaviour
{
    [SerializeField] private LayerMask _AfectedLayers;
    [SerializeField] private ParticleSystem _ToxicParticle;
    WaitForSeconds _WaitForSeconds = new WaitForSeconds(1);
    private bool _can;
    private Transform _t;

    private void Start()
    {
        _t = transform;
        _can = true;
        StartCoroutine(effect());
    }
    void Update()
    {
        if(!_can) return;
        if (Physics2D.OverlapCircle(_t.position, _t.localScale.x, _AfectedLayers))
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(_t.position, 1, _AfectedLayers);
            if (cols.Length > 0)
            {
                if (cols[0].TryGetComponent(out PlayerController player))
                {
                    StartCoroutine(damageDelay());
                    player.TakeDamage(1);
                }
            }
            
        }
    }
    private IEnumerator effect()
    {
        yield return new WaitForSeconds(4);
        ParticleSystem.MainModule mainPart = _ToxicParticle.main;
        mainPart.startLifetime = 0;
        GetComponent<SpriteRenderer>().DOFade(0, 1).OnComplete(() => { Destroy(gameObject); });
    }
    private IEnumerator damageDelay()
    {
        _can = false;
        yield return _WaitForSeconds;
        _can = true;
    }
}