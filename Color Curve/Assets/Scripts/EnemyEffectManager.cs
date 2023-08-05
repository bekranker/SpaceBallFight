using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyEffectManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _ParticleSystem;
    [SerializeField] private EnemyManager _EnemyManager;
    private Camera _mainCamera;
    private void OnEnable()
    {
        _mainCamera = Camera.main;
        _EnemyManager.OnDead += ParticleDieState;
    }
    private void OnDisable()
    {
        _EnemyManager.OnDead -= ParticleDieState;
    }
    private void ParticleDieState()
    {
        ParticleSystem spawnedDeadParticle = Instantiate(_ParticleSystem, transform.position, Quaternion.identity);
        ParticleSystem.MainModule mainPart = spawnedDeadParticle.main;
        mainPart.startColor = _EnemyManager._NormalColor;
        _mainCamera.DOShakePosition(.05f, .1f);
    }
}
