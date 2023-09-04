using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] private GameObject _BulletBallon, _HealthAdBallon, _BulletAdBallon, __HealthBallon, _ShieldBallon;
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
    private Vector2 _to;
    private bool _canEffect;

    private void Start()
    {
        _canEffect = true;
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
    
    public void TakeDamage(int damage, Transform pos, bool canStopIt)
    {
        if(_healthCount - damage > 0)
        {
            Audio.PlayAudio("EnemyHit", .25f);
            DecreaseHealt(damage);
            StartCoroutine(damageAction(canStopIt));
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
            
            CreateRedSkillPoint();
            CreateGreenSkillPoint();
            CreateBlueSkillPoint();
            CreateBullet();
            CreateHealthAd();
            CreateBulletAd();
            CreateHealthBallon();
            CreateShieldAd();
            Destroy(gameObject);
            _canDie = false;
        }
    }
    private void CreateHealthBallon()
    {
        float randX = _t.position.x + (Random.Range(-2, 2));
        float randY = _t.position.y + (Random.Range(-2, 2));
        _to = new Vector2(randX, randY);
        if (DidFallHealth())
        {
            Instantiate(__HealthBallon, _to, Quaternion.identity);
        }
    }
    private void CreateHealthAd()
    {
        float randX = _t.position.x + (Random.Range(-2, 2));
        float randY = _t.position.y + (Random.Range(-2, 2));
        _to = new Vector2(randX, randY);
        if (_playerController.CanDropHealth)
        {
            Instantiate(_HealthAdBallon, _to, Quaternion.identity);
            _playerController.CanDropHealth = false;
        }
    }
    private void CreateBulletAd()
    {
        float randX = _t.position.x + (Random.Range(-2, 2));
        float randY = _t.position.y + (Random.Range(-2, 2));
        _to = new Vector2(randX, randY);
        if (_playerController.CanDropBullet)
        {
            Instantiate(_BulletAdBallon, _to, Quaternion.identity);
            _playerController.CanDropBullet = false;
        }
    }
    private void CreateShieldAd()
    {
        float randX = _t.position.x + (Random.Range(-2, 2));
        float randY = _t.position.y + (Random.Range(-2, 2));
        _to = new Vector2(randX, randY);
        if (DidFallShield())
        {
            Instantiate(_ShieldBallon, _to, Quaternion.identity);
            _playerController.CanDropBullet = false;
        }
    }
    IEnumerator damageAction(bool canStopIt)
    {
        if (_canEffect)
        {
            _t.DOPunchScale(new Vector2(.5f, .6f), .075f).OnComplete(()=> _canEffect = true).SetUpdate(true);
            _canEffect = false;
        }
        for (int i = 0; i < _Sp.Count; i++)
        {
            _Sp[i].color = _DamagedColor;
        }
        if (canStopIt)
        {
            Speed = 0;
        }
        yield return _sleepTime;
        if (_Sp == null) yield break;
        if (canStopIt)
            Speed = _firsSpeed;
        for (int i = 0; i < _Sp.Count; i++)
        {
            _Sp[i].color = _NormalColor;
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public IEnumerator ChangeStateRandom()
    {
        _t.DOPunchScale(new Vector2(.7f,.9f), .5f).SetUpdate(true);
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
    private void CreateRedSkillPoint()
    {
        float randX = _t.position.x + (Random.Range(-2, 2));
        float randY = _t.position.y + (Random.Range(-2, 2));
        _to = new Vector2(randX, randY);
        int value;
        if (_playerController.BlueSkillOpened)
        {
            value = 10;
        }
        else if (_playerController.GreenSkillOpened)
        {
            value = 15;
        }
        else
        {
            value = 22;
        }
        if (!DidFallPoint(value)) return;
        if (_playerController.RedSkillOpened)
            Instantiate(_SkillPointRed, _to, Quaternion.identity);
    }
    private void CreateGreenSkillPoint()
    {
        float randX = _t.position.x + (Random.Range(-2, 2));
        float randY = _t.position.y + (Random.Range(-2, 2));
        _to = new Vector2(randX, randY);
        int value;
        if (_playerController.BlueSkillOpened)
        {
            value = 10;
        }
        else
        {
            value = 22;
        }
        if (!DidFallPoint(value)) return;
        if (_playerController.GreenSkillOpened)
            Instantiate(_SkillPointGreen, _to, Quaternion.identity);
    }
    private void CreateBlueSkillPoint()
    {
        float randX = _t.position.x + (Random.Range(-2, 2));
        float randY = _t.position.y + (Random.Range(-2, 2));
        _to = new Vector2(randX, randY);
        if (!DidFallPoint(10)) return;
        if (_playerController.BlueSkillOpened)
            Instantiate(_SkillPointBlue, _to, Quaternion.identity);
    }
    private void CreateBullet()
    {
        float randX = _t.position.x + (Random.Range(-2, 2));
        float randY = _t.position.y + (Random.Range(-2, 2));
        _to = new Vector2(randX, randY);
        if (!DidFallBullet()) return;
        Instantiate(_BulletBallon, _to, Quaternion.identity);
    }
    private bool DidFallPoint(int value = 5)
    {
        int rand = Random.Range(0, 100);
        if (rand <= value)
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
    private bool DidFallHealth()
    {
        int rand = Random.Range(0, 100);
        if (rand <= 10)
        {
            return true;
        }
        return false;
    }
    private bool DidFallShield()
    {
        int rand = Random.Range(0, 100);
        if (rand < 5)
        {
            return true;
        }
        return false;
    }
}