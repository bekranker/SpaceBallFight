using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDieState : MonoBehaviour
{
    [SerializeField] private ParticleSystem _DieParticle;

    private void OnEnable()
    {
        BossTag.OnDie += Die;
    }
    private void OnDisable()
    {
        BossTag.OnDie -= Die;
    }
    private void Die()
    {
        Instantiate(_DieParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
