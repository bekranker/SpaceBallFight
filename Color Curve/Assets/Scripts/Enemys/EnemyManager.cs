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
    [SerializeField] public EnemyColor EnemyColorTypes = new EnemyColor();
    [SerializeField] public EnemyTypes EnemyTypes = new EnemyTypes();
    [SerializeField] public ParticleSystem BackgroundParticle;
    [SerializeField] public List<TrailRenderer> Trail;
    [SerializeField] private GameObject _SkillPointRed, _SkillPointBlue, _SkillPointGreen;
    [SerializeField] private Vector2 _FromScale;
    [SerializeField] private GameObject _BulletBallon, _HealthAdBallon, _BulletAdBallon;
    [SerializeField] private Color _BlueColor;
    private PlayerController _playerController;
    //-----------------------------------Cut-----------------------------------

    private WaitForSeconds _sleepTime = new WaitForSeconds(.05f);
    private ScoreManager _scoreManager;
    private WaveManager _waveManager;
    private int _healthCount;
    private bool _canDie;
    private Camera _mainCamera;
    private Transform _t;
    private float _firsSpeed;

    private void Start()
    {
        StartCoroutine(ChangeStateRandom());
        _firsSpeed = Speed;
        _t = transform;
        float randScale = Random.Range(_FromScale.x, _FromScale.y);
        Vector2 scale = new Vector2(randScale, randScale);
        _t.localScale = scale;
        _scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        _waveManager = GameObject.FindGameObjectWithTag("WaveManager").GetComponent<WaveManager>();
        _playerController = FindObjectOfType<PlayerController>();
        _mainCamera = Camera.main;
        SetHealth(_Health);
        _canDie = true;
    }
    
    public void TakeDamage(int damage, Transform pos)
    {
        if(_healthCount - damage > 0)
        {
            Audio.PlayAudio("EnemyHit", .25f);
            DecreaseHealt(damage);
            StartCoroutine(damageAction());
            _scoreManager.IncreaseScore(damage, pos);
            OnHit?.Invoke();
        }
        else
        {
            Audio.PlayAudio("bom", .33f);
            if (!_canDie) return;
            if(_waveManager != null)
                _waveManager.IncreaseKilledEnemy();
            OnDead?.Invoke();
            _mainCamera.DOShakePosition(.1f, .5f);
            _scoreManager.IncreaseScore(damage, pos);
            CreateSkillPoint();
            CreateBullet();
            CreateHealthAd();
            CreateBulletAd();
            Destroy(gameObject);
            _canDie = false;
        }
    }
    private void CreateHealthAd()
    {
        if (_playerController.CanDropHealth())
        {
            Instantiate(_HealthAdBallon, _t.position, Quaternion.identity);
        }
    }
    private void CreateBulletAd()
    {
        if (_playerController.CanDropBullet)
        {
            Instantiate(_BulletAdBallon, _t.position, Quaternion.identity);
            _playerController.CanDropBullet = false;
        }
    }
    IEnumerator damageAction()
    {
        for (int i = 0; i < _Sp.Count; i++)
        {
            _Sp[i].color = _DamagedColor;
        }
        Speed = 0;
        yield return _sleepTime;
        if (_Sp == null) yield break;
        Speed = _firsSpeed;
        for (int i = 0; i < _Sp.Count; i++)
        {
            _Sp[i].color = _NormalColor;
        }
    }
    public IEnumerator ChangeStateRandom()
    {
        if (EnemyTypes == EnemyTypes.X)
        {
            int rand = Random.Range(0, 4);
            ParticleSystem.MainModule mainPArt;
            if (BackgroundParticle != null)
            {
                mainPArt = BackgroundParticle.main;
            }
            switch (rand)
            {
                case 0:
                    EnemyColorTypes = EnemyColor.Red;
                    for (int i = 0; i < _Sp.Count; i++)
                    {
                        _Sp[i].color = Color.red;
                    }
                    if (BackgroundParticle != null)
                        mainPArt.startColor = Color.red;
                    if (Trail != null)
                    {
                        Trail.ForEach((_trail) => { _trail.startColor = Color.red; });
                    }
                    _NormalColor = Color.red;
                    break;
                case 1:
                    EnemyColorTypes = EnemyColor.Green;
                    for (int i = 0; i < _Sp.Count; i++)
                    {
                        _Sp[i].color = Color.green;
                    }
                    if (BackgroundParticle != null)
                        mainPArt.startColor = Color.green;
                    if (Trail != null)
                    {
                        Trail.ForEach((_trail) => { _trail.startColor = Color.green; });
                    }
                    _NormalColor = Color.green;
                    break;
                case 2:
                    EnemyColorTypes = EnemyColor.Blue;
                    for (int i = 0; i < _Sp.Count; i++)
                    {
                        _Sp[i].color = _BlueColor;
                    }
                    if (BackgroundParticle != null)
                        mainPArt.startColor = _BlueColor;
                    if (Trail != null)
                    {
                        Trail.ForEach((_trail) => { _trail.startColor = _BlueColor; });
                    }
                    _NormalColor = _BlueColor;
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(3);
            StartCoroutine(ChangeStateRandom());
        }
   
    }
    private void SetHealth(int value)
    {
        _healthCount = value;
    }
    private void DecreaseHealt(int value)
    {
        _healthCount -= value;
    }
    private void CreateSkillPoint()
    {
        if (!DidFallPoint()) return;
        switch (EnemyColorTypes)
        {
            case EnemyColor.Red:
                if (_playerController.RedSkillOpened)
                    Instantiate(_SkillPointRed, _t.position, Quaternion.identity);
                break;
            case EnemyColor.Green:
                if (_playerController.GreenSkillOpened)
                    Instantiate(_SkillPointGreen, _t.position, Quaternion.identity);
                break;
            case EnemyColor.Blue:
                if (_playerController.BlueSkillOpened)
                    Instantiate(_SkillPointBlue, _t.position, Quaternion.identity);
                break;
            default:
                break;
        }
    }
    private void CreateBullet()
    {
        if (!DidFallBullet()) return;
        Instantiate(_BulletBallon, _t.position, Quaternion.identity);
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
    private bool DidFallBullet()
    {
        int rand = Random.Range(0, 5);
        if (rand <= 1)
        {
            return true;
        }
        return false;
    }
}