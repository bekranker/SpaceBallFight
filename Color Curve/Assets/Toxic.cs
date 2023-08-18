using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toxic : MonoBehaviour
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
        if(!_can) return;
        if (Physics2D.OverlapCircle(_t.position, _t.localScale.x, _AfectedLayers))
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(_t.position, _t.localScale.x, _AfectedLayers);
            if (cols[0].TryGetComponent(out PlayerController player))
            {
                StartCoroutine(damageDelay());
                player.DecreaseHealth(1);
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