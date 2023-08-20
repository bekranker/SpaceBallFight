using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{

    public event Action OnDead, OnHit;
    public float Speed;
    [SerializeField] private int _Health;
    [SerializeField] public Color _NormalColor, _DamagedColor;
    [SerializeField] public List<SpriteRenderer> _Sp;
    [SerializeField] private Slider _HealthSlider;
    [SerializeField] public EnemyColor EnemyColorTypes = new EnemyColor();
    [SerializeField] public EnemyTypes EnemyTypes = new EnemyTypes();
    [SerializeField] public ParticleSystem BackgroundParticle;
    [SerializeField] public List<TrailRenderer> Trail;
    [SerializeField] private GameObject _SkillPointRed, _SkillPointBlue, _SkillPointGreen;


    //-----------------------------------Cut-----------------------------------

    private WaitForSeconds _sleepTime = new WaitForSeconds(.05f);
    private ScoreManager _scoreManager;
    private WaveManager _waveManager;
    private int _healthCount;
    private bool _canDie;
    private Camera _mainCamera;
    private Transform _t;


    private void Start()
    {
        _t = transform;
        _scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        _waveManager = GameObject.FindGameObjectWithTag("WaveManager").GetComponent<WaveManager>();
        _mainCamera = Camera.main;
        SetHealth(_Health);
        _canDie = true;
    }
    public void TakeDamage(int damage, Transform pos)
    {
        if(_healthCount - damage > 0)
        {
            DecreaseHealt(damage);
            StartCoroutine(damageAction());
            _scoreManager.IncreaseScore(damage * Random.Range(5, 10), pos);
            OnHit?.Invoke();
        }
        else
        {
            if (!_canDie) return;
            if(_waveManager != null)
                _waveManager.IncreaseKilledEnemy();
            OnDead?.Invoke();
            _mainCamera.DOShakePosition(.1f, .5f);
            _scoreManager.IncreaseScore(damage * Random.Range(7, 12), pos);
            CreateSkillPoint();
            Destroy(gameObject);
            _canDie = false;
        }
    }
    IEnumerator damageAction()
    {
        for (int i = 0; i < _Sp.Count; i++)
        {
            _Sp[i].color = _DamagedColor;
        }
        yield return _sleepTime;
        for (int i = 0; i < _Sp.Count; i++)
        {
            _Sp[i].color = _NormalColor;
        }
    }
    private void SetHealth(int value)
    {
        _healthCount = value;
        _HealthSlider.value = _healthCount;
        _HealthSlider.maxValue = _Health;
        _HealthSlider.minValue = 0;
    }
    private void DecreaseHealt(int value)
    {
        _healthCount -= value;
        _HealthSlider.value = _healthCount;
    }
    private void CreateSkillPoint()
    {
        if (!DidFallPoint()) return;
        switch (EnemyColorTypes)
        {
            case EnemyColor.Red:
                Instantiate(_SkillPointRed, _t.position, Quaternion.identity);
                break;
            case EnemyColor.Green:
                Instantiate(_SkillPointGreen, _t.position, Quaternion.identity);
                break;
            case EnemyColor.Blue:
                Instantiate(_SkillPointBlue, _t.position, Quaternion.identity);
                break;
            default:
                break;
        }
    }
    private bool DidFallPoint()
    {
        int rand = Random.Range(0,10);
        if (rand <= 2)
        {
            return true;
        }
        return false;
    }
}
