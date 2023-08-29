using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ScreenShake : MonoBehaviour
{
    Transform _t;
    Camera _mainCamera;
    [SerializeField] private GameObject _CrashEffect;
    private ShockWaveManager _ShockWaveManager;

    private void Start()
    {
        _t = transform;
        _mainCamera = Camera.main;
        _ShockWaveManager = GameObject.FindGameObjectWithTag("ShockWave").GetComponent<ShockWaveManager>();
    }
    public void Shake()
    {
        Audio.PlayAudio("EnemyDie1", .6f, 1);
        _mainCamera.DOShakePosition(.1f, 3, 12, fadeOut:true, randomnessMode: ShakeRandomnessMode.Harmonic);
        Instantiate(_CrashEffect, _t.position, Quaternion.identity);
        _ShockWaveManager.CallShockWave();
    }
}
