using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class BossTag : MonoBehaviour
{
    public static event Action OnDie, OnHit;
    [SerializeField] public EnemyColor EnemyColor = EnemyColor.Red; 
    [SerializeField] private float _MaxHealth;
    [SerializeField] public List<SpriteRenderer> _SpriteRenderer = new List<SpriteRenderer>();
    [SerializeField] private Color _DamageColor, _NormalColor;
    [SerializeField] private List<Transform> _Eyes = new List<Transform>();
    [SerializeField] private ScoreManager _ScoreManager;
    [SerializeField] private BossAttackManager _BossAttackManager;
    [HideInInspector] public ShockWaveManager _ShockWave;
    [SerializeField] public Color _ColorBlue;
    [SerializeField] public float Speed;
    //----------------------------------------------------Cut----------------------------------------------------


    private BossManager _BossManager;
    private float _currentHealth;
    private WaitForSecondsRealtime _sleepTime = new WaitForSecondsRealtime(.05f);
    private WaitForSecondsRealtime _sleepTimeForDelay = new WaitForSecondsRealtime(1f);
    [HideInInspector] public Transform _player;
    private Vector3 _playerPosition;
    private Transform _t;
    private Vector2 _direction;
    private bool _didDead;
    private bool _canDamage;
    private float _firstSpeed;


    private void Start()
    {
        _canDamage = true;
        _firstSpeed = Speed;
        _t = transform;
        _player = FindAnyObjectByType<PlayerController>().transform;
        _BossManager = GameObject.FindGameObjectWithTag("BossManager").GetComponent<BossManager>();
        _ScoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        _ShockWave = GameObject.FindGameObjectWithTag("ShockWave").GetComponent<ShockWaveManager>();
        _BossManager.SetHealthSlider(_MaxHealth, 0, _MaxHealth);
        _BossManager.SetHealthSlider(_MaxHealth, _MaxHealth);
        _currentHealth = _MaxHealth;

        Setcolor(EnemyColor);
    }
    private void Update()
    {
        LookToPlayer();
    }
    
    private void LookToPlayer()
    {
        _playerPosition = _player.position;

        _Eyes?.ForEach((_eyes) =>
        {
            _direction = new Vector2(_playerPosition.x - _eyes.position.x, _playerPosition.y - _eyes.position.y);
            _eyes.up = _direction;
        });
    }
    public void TakeDamage(float damage, Transform pos, string tag)
    {
        if (_didDead) return;
        if (!_canDamage) return;
        if (_currentHealth - damage <= 0)
        {
            Audio.PlayAudio("BossDie", .5f);
            _didDead = true;
            OnDie?.Invoke();
            _BossManager.EndBossFight();
            _BossAttackManager.CanFight = false;
        }
        else
        {
            Audio.PlayAudio("EnemyHit", .25f);
            _ScoreManager.IncreaseScore(Mathf.RoundToInt(damage), pos);
            OnHit?.Invoke();
            StartCoroutine(DamageEffect());

            _currentHealth -= (IsCorrectColor(tag) ? damage : damage / 3);
        }
        _BossManager.SetHealthSlider(_currentHealth, _MaxHealth);
    }
    private IEnumerator DamageEffect()
    {
        _SpriteRenderer?.ForEach((_sprites) => 
        {
            _sprites.color = _DamageColor;
        });
        yield return _sleepTime;
        _SpriteRenderer?.ForEach((_sprites) =>
        {
            _sprites.color = _NormalColor;
        });
    }
    public void Setcolor(EnemyColor enemyC)
    {
        _ShockWave.CallShockWave();
        EnemyColor = enemyC;
        switch (enemyC)
        {
            case EnemyColor.Red:
                _NormalColor = Color.red;
                break;
            case EnemyColor.Green:
                _NormalColor = Color.green;
                break;
            case EnemyColor.Blue:
                _NormalColor = _ColorBlue;
                break;
            default:
                break;
        }
        _SpriteRenderer?.ForEach((_sp) => 
        {
            _sp.color = _NormalColor;
        });

    }
    private bool IsCorrectColor(string tag)
    {
        if (tag == "BulletRed" && EnemyColor == EnemyColor.Red)
        {
            return true;
        }
        if (tag == "BulletGreen" && EnemyColor == EnemyColor.Green)
        {
            return true;
        }
        if (tag == "BulletBlue" && EnemyColor == EnemyColor.Blue)
        {
            return true;
        }
        return false;
    }
    public IEnumerator Slow()
    {
        Speed *= 2;
        yield return _sleepTimeForDelay;
        Speed = _firstSpeed;
    }
}