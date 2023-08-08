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
    [SerializeField] private GameManager _GameManager;
    [SerializeField] private List<SpriteRenderer> _Borders = new List<SpriteRenderer>();
    [SerializeField] private Animator _Animation;
    [SerializeField] private List<ParticleSystem> _BossBeginParticles;
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
        Instantiate(_BossBeginParticles[_bossCount], _BossSpawnPoint.position, Quaternion.identity);
        _Borders.ForEach((_border) => { _border.gameObject.SetActive(true);  _border.DOFade(1, 1); });
        _mainCamera.DOShakePosition(3, 2, 2, fadeOut: true);
        yield return _sleep;
        _bossCount++;
        _mainCamera.DOShakePosition(.25f, 5, 10, fadeOut: true, randomnessMode: ShakeRandomnessMode.Harmonic);
        _SpawnManager.SpawnBoss();
    }
    private IEnumerator EndBossFightIE()
    {
        StartCoroutine(GoSlider());
        _Borders.ForEach((_border) => { _border.DOFade(0, 1).OnComplete(() => { _border.gameObject.SetActive(false); }); });
        yield return _sleep;
        _CameraFollow.enabled = true;
        _canCallBoss = true;
        _WaveManager.WaveIndex++;
        _GameManager.ChangeWaveInfos();
        _SpawnManager.CanSpawn = true;
    }
    private IEnumerator GoSlider()
    {
        _Animation.SetTrigger("Go");
        yield return new WaitForSeconds(1);
        _Animation.gameObject.SetActive(false);
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