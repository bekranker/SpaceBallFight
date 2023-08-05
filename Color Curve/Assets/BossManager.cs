using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BossManager : MonoBehaviour
{
    [SerializeField] private WaveManager _WaveManager;
    [SerializeField] private ParticleSystem _BossBeginParticleSystem, _BossHerePS;
    [SerializeField] public Transform _BossSpawnPoint;
    [SerializeField] private Spawneranager _SpawnManager;
    [SerializeField] private CameraFollow _CameraFollow;
    [SerializeField] public Slider _BossHealthSlider;
    [SerializeField] private TMP_Text _BossHealthText;

    //------------------------Cut------------------------

    private WaitForSeconds _sleep = new WaitForSeconds(3);
    private Camera _mainCamera;
    private bool _canCallBoss;



    private void Start()
    {
        _canCallBoss = true;
        _mainCamera = Camera.main;
    }
    public void BossFight()
    {
        if (!_canCallBoss) return;

        _CameraFollow.enabled = false;
        StartCoroutine(beginBossFight());
        _canCallBoss = false;
    }
    private IEnumerator beginBossFight()
    {
        Instantiate(_BossBeginParticleSystem, _BossSpawnPoint.position, Quaternion.identity);
        _mainCamera.DOShakePosition(3, 2, 2, fadeOut: true);
        yield return _sleep;
        _mainCamera.DOShakePosition(.25f, 5, 10, fadeOut: true, randomnessMode: ShakeRandomnessMode.Harmonic);
        Instantiate(_BossHerePS, _BossSpawnPoint.position, Quaternion.identity);
        _SpawnManager.SpawnBoss();
    }
    private IEnumerator endBossFight()
    {
        yield return null;
    }
    public void SetHealthSlider(float value, float min, float max)
    {
        _BossHealthSlider.gameObject.SetActive(true);
        _BossHealthSlider.maxValue = max;
        _BossHealthSlider.minValue = min;
        _BossHealthSlider.value = value;
    }
    public void SetHealthSlider(float value, float max)
    {
        _BossHealthSlider.value = value;
        _BossHealthText.text = value.ToString() + " / " + max.ToString();
    }
}