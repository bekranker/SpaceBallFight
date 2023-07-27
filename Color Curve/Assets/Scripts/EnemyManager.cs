using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject _DeadParticle;
    [SerializeField] private int _Health;
    [SerializeField] public Color _NormalColor, _DamagedColor;
    [SerializeField] public List<SpriteRenderer> _Sp;
    [SerializeField] Slider _HealthSlider;
    [SerializeField] public EnemyColor EnemyColorTypes = new EnemyColor();
    [SerializeField] public EnemyTypes EnemyTypes = new EnemyTypes();
    [SerializeField] public ParticleSystem BackgroundParticle;
    [SerializeField] public List<TrailRenderer> Trail;
    private ScoreManager _scoreManager;
    private WaveManager _waveManager;
    private int _healthCount;
    private bool _canDie;
    private Camera mainCamera;
    [SerializeField] private GameObject _Prefab;

    private void Start()
    {
        _scoreManager = FindObjectOfType<ScoreManager>();
        _waveManager = FindObjectOfType<WaveManager>();
        _healthCount = _Health;
        _HealthSlider.maxValue = _Health;
        _HealthSlider.minValue = 0;
        _HealthSlider.value = _healthCount;
        _canDie = true;
        mainCamera = Camera.main;
    }

    public void TakeDamage(int damage, Transform pos)
    {
        if(_healthCount - damage > 0)
        {
            _healthCount -= damage;
            _HealthSlider.value = _healthCount;
            StartCoroutine(damageAction());
            mainCamera.DOShakePosition(.05f, .1f);
            _scoreManager.IncreaseScore(damage * Random.Range(5, 10), pos);
        }
        else
        {
            if (!_canDie) return;
            _waveManager.KilledEnemyCount++;
            _waveManager.ChangeScrore();
            GameObject spawnedDeadParticle = Instantiate(_DeadParticle, transform.position, Quaternion.identity);
            ParticleSystem.MainModule mainPart = spawnedDeadParticle.GetComponent<ParticleSystem>().main;
            mainPart.startColor = _NormalColor;
            mainCamera.DOShakePosition(.1f, .5f);
            _scoreManager.IncreaseScore(damage * Random.Range(7, 12), pos);
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
        yield return new WaitForSeconds(.05f);
        for (int i = 0; i < _Sp.Count; i++)
        {
            _Sp[i].color = _NormalColor;
        }
    }
}
