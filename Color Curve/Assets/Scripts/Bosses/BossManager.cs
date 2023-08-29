using DG.Tweening;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CrazyGames;

public class BossManager : MonoBehaviour
{
    [SerializeField] private WaveManager _WaveManager;
    [SerializeField] private ParticleSystem _BossBeginParticleSystem, _BossHerePS;
    [SerializeField] public Transform _BossSpawnPoint;
    [SerializeField] private Spawneranager _SpawnManager;
    [SerializeField] private CameraFollow _CameraFollow;
    [SerializeField] public Slider _BossHealthSlider;
    [SerializeField] private TMP_Text _BossHealthText;
    [SerializeField] private GameManager _GameManager;
    [SerializeField] private List<Image> _Borders = new List<Image>();
    [SerializeField] private List<Collider2D> _BordersCollider = new List<Collider2D>();
    [SerializeField] private List<ParticleSystem> _BossBeginParticles;
    [SerializeField] private AudioSource _AudioSource;
    [SerializeField] private AudioClip _NormalBGMusic, _BossFight;
    [SerializeField] private PlayerController _PlayerController;
    private int _bossCount = 0;
    //------------------------Cut------------------------

    private WaitForSeconds _sleep = new WaitForSeconds(2);
    private Camera _mainCamera;
    private bool _canCallBoss;



    private void Start()
    {
        _canCallBoss = true;
        _mainCamera = Camera.main;
    }
    public void BeginBossFight()
    {
        if (!_canCallBoss) return;

        _PlayerController.LockPlayer = true;
        _CameraFollow.enabled = false;
        StartCoroutine(BeginBossFightIE());
        _canCallBoss = false;
    }
    public void EndBossFight()
    {
        StartCoroutine(EndBossFightIE());
    }
    private IEnumerator BeginBossFightIE()
    {
        GC.Collect();
        Audio.PlayAudio("BossBegin", .2f);
        _AudioSource.clip = _BossFight;
        _AudioSource.Play();
        Instantiate(_BossBeginParticles[_bossCount], _BossSpawnPoint.position, Quaternion.identity);
        _Borders.ForEach((_border) => { _border.DOFade(1, 1); });
        _BordersCollider.ForEach((_border) => { _border.gameObject.SetActive(true);});
        _mainCamera.DOShakePosition(3, 2, 2, fadeOut: true);
        yield return _sleep;
        Audio.PlayAudio("BlueBossegin", .1f);
        _bossCount++;
        _mainCamera.DOShakePosition(.25f, 5, 10, fadeOut: true, randomnessMode: ShakeRandomnessMode.Harmonic);
        _SpawnManager.SpawnBoss();
    }
    private IEnumerator EndBossFightIE()
    {
        GC.Collect();
        StartCoroutine(GoSlider());
        _Borders.ForEach((_border) => { _border.DOFade(0, 1).OnComplete(() => {
            _BordersCollider.ForEach((_border) => { _border.gameObject.SetActive(false); });
        }); });

        yield return _sleep;
        _PlayerController.LockPlayer = false;
        _AudioSource.clip = _NormalBGMusic;
        CrazyEvents.Instance.HappyTime();
        _AudioSource.Play();
        _CameraFollow.enabled = true;
        _canCallBoss = true;
        _WaveManager.WaveIndex++;
        _GameManager.ChangeWaveInfos();
        _SpawnManager.CanSpawn = true;
    }
    private IEnumerator GoSlider()
    {
        _BossHealthSlider.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
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