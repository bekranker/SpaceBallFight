using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ScreenShake : MonoBehaviour
{
    Transform _t;
    Camera _mainCamera;
    private ShockWaveManager _ShockWaveManager;

    private void Start()
    {
        _t = transform;
        _mainCamera = Camera.main;
        _ShockWaveManager = GameObject.FindGameObjectWithTag("ShockWave").GetComponent<ShockWaveManager>();
    }
    public void Shake()
    {
        Audio.PlayAudio("bom", .35f);
        _mainCamera.DOShakePosition(.1f, 3, 12, fadeOut:true, randomnessMode: ShakeRandomnessMode.Harmonic);
        _ShockWaveManager.CallShockWave();
    }
}
