using DG.Tweening;
using System.Collections;
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
    [SerializeField] private List<ParticleSystem> _BossBeginParticles;
    [SerializeField] private AudioSource _AudioSource;
    [SerializeField] private AudioClip _NormalBGMusic, _BossFight;
    [SerializeField] private PlayerController _PlayerController;
    [SerializeField] private RemaningTimeManager _RemaningTimeManager;
    [SerializeField] public Transform _BossSliderT, _BossSliderText;
    private int _bossCount = 0;
    //------------------------Cut------------------------

    private WaitForSeconds _sleep = new WaitForSeconds(5);
    private Camera _mainCamera;
    private bool _canCallBoss, _canEffect;


    private void Start()
    {
        _canEffect = true;
        _canCallBoss = true;
        _mainCamera = Camera.main;
    }
    public void BeginBossFight()
    {
        if (!_canCallBoss) return;

        _RemaningTimeManager.CanDecrease = false;
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
        Audio.PlayAudio("BossBegin", .2f);
        _AudioSource.clip = _BossFight;
        _AudioSource.Play();
        Instantiate(_BossBeginParticles[_bossCount], _BossSpawnPoint.position, Quaternion.identity);
        _mainCamera.DOShakePosition(3, 2, 2, fadeOut: true);
        yield return _sleep;
        Audio.PlayAudio("BlueBossegin", .1f);
        _bossCount++;
        _mainCamera.DOShakePosition(.25f, 5, 10, fadeOut: true, randomnessMode: ShakeRandomnessMode.Harmonic);
        _SpawnManager.SpawnBoss();
    }
    private IEnumerator EndBossFightIE()
    {
        StartCoroutine(GoSlider());
        CrazyEvents.Instance.HappyTime();
        yield return _sleep;
        _PlayerController.LockPlayer = false;
        _AudioSource.clip = _NormalBGMusic;
        _AudioSource.Play();
        _CameraFollow.enabled = true;
        _WaveManager.WaveIndex++;
        _PlayerController.SetBulletSliderBeReady(_WaveManager._WaveData[_WaveManager.WaveIndex].MaxBulletSize);
        _PlayerController.CurrentHealth = _PlayerController.MaxHealth;
        _PlayerController.PlayerHealthSldier();
        _RemaningTimeManager.SetRemaningTime(_WaveManager._WaveData[_WaveManager.WaveIndex].WaveTimeCount);
        _GameManager.ChangeWave();
        _canCallBoss = true;
        _SpawnManager.CanSpawn = true;
        _RemaningTimeManager.CanDecrease = true;
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
        if (_canEffect)
        {
            _BossSliderText.DOPunchScale(.5f * Vector2.one, .1f).SetUpdate(true);
            _BossSliderT.DOPunchPosition(8 * Vector2.right, .1f).OnComplete(() => { _canEffect = true; }).SetUpdate(true);
            _canEffect = false;
        }
    }
}