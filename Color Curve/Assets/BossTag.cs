using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class BossTag : MonoBehaviour
{

    public static event Action OnDie, OnHit;

    [SerializeField] private float _MaxHealth;
    [SerializeField] private List<SpriteRenderer> _SpriteRenderer = new List<SpriteRenderer>();
    [SerializeField] private Color _DamageColor, _NormalColor;
    [SerializeField] private List<Transform> _Eyes = new List<Transform>();
    [SerializeField] private ParticleSystem _HitParticle;
    [SerializeField] private ScoreManager _ScoreManager;


    //----------------------------------------------------Cut----------------------------------------------------


    private BossManager _BossManager;
    private float _currentHealth;
    private WaitForSecondsRealtime _sleepTime = new WaitForSecondsRealtime(.05f);
    private Transform _player;
    private Vector3 _playerPosition;
    private Transform _t;
    private Vector2 _direction;





    private void Start()
    {
        _t = transform;
        _player = FindAnyObjectByType<PlayerController>().transform;
        _BossManager = GameObject.FindGameObjectWithTag("BossManager").GetComponent<BossManager>();
        _ScoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        _BossManager.SetHealthSlider(_MaxHealth, 0, _MaxHealth);
        _BossManager.SetHealthSlider(_MaxHealth, _MaxHealth);
        _currentHealth = _MaxHealth;
    }
    private void Update()
    {
        LookToPlayer();
    }
    private IEnumerator SetBossBeReady()
    {
        _BossManager.SetHealthSlider(_MaxHealth, 0, _MaxHealth);
        yield return new WaitForSeconds(2);
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
    public void TakeDamage(float damage, Transform pos)
    {
        if (_currentHealth - damage <= 0)
        {
            OnDie?.Invoke();
        }
        else
        {
            for (int i = 0; i < _SpriteRenderer.Count; i++)
            {
                Instantiate(_HitParticle, _SpriteRenderer[i].gameObject.transform.position, Quaternion.identity);
            }
            _ScoreManager.IncreaseScore(Mathf.RoundToInt(damage), pos);
            OnHit?.Invoke();
            StartCoroutine(DamageEffect());
            _currentHealth -= damage;
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
}