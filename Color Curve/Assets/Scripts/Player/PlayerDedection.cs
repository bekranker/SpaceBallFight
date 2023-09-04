using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDedection : MonoBehaviour
{
    [SerializeField] public int _RedPointIndex, _BluePointIndex, _GreenPointIndex;
    [SerializeField] private PlayerAttack _PlayerAttack;
    [SerializeField] private PlayerController _PlayerController;
    [SerializeField] private ScoreManager _ScoreManager;
    [SerializeField] private Slider _RedSlider, _GreenSlider, _BlueSlider;
    [SerializeField] private ParticleSystem _SkillPointRedP, _SkillPointBlueP, _SkillPointGreenP, _BulletCollectedParticle, _HeathCollectedParticle;
    [SerializeField] public Text _RedSliderTMP, _GreenSliderTMP, _BlueSliderTMP;
    [SerializeField] public Transform _RedSliderT, _GreenSliderT, _BlueSliderT;
    [SerializeField] private UIManager _UIManager;
    [SerializeField] private GameObject _BulletAdPanel, _HealthAdPanel, _ShieldAdPanel;
    public bool CanDedect;
    private WaitForSeconds WaitForSeconds = new WaitForSeconds(1);
    private Transform _t;
    private bool _canEffect;
    private Transform _cameraTransform;
    private void Start()
    {
        CanDedect = true;
        _cameraTransform = Camera.main.transform;
        _canEffect = true;
        SetText(_RedSliderT, _RedSliderTMP, $"{_RedPointIndex}/5");
        SetText(_GreenSliderT, _GreenSliderTMP, $"{_GreenPointIndex}/5");
        SetText(_BlueSliderT, _BlueSliderTMP, $"{_BluePointIndex}/5");
        _t = transform;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        CollectPoints(collision);



        //------------------------------------------------


        if (!CanDedect) return;
        if (collision.gameObject.CompareTag("Boss"))
        {
            _PlayerController.TakeDamage(20);
        }

        if (collision.CompareTag("FreezeBullet"))
        {
            StartCoroutine(decreaseSpeed());
            _PlayerController.TakeDamage(10);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("BossBullet"))
        {
            _PlayerController.TakeDamage(10);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Enemy"))
        {
            _PlayerController.TakeDamage(10);
            collision.GetComponent<EnemyManager>().TakeDamage(999, _t, true);
        }
        if (collision.gameObject.CompareTag("Lazer"))
        {
            _PlayerController.TakeDamage(10);
        }
        if (collision.gameObject.CompareTag("Spike"))
        {
            _PlayerController.TakeDamage(10);
        }
    }
    private IEnumerator decreaseSpeed()
    {
        _PlayerController._Speed /= 2;
        yield return WaitForSeconds;
        _PlayerController._Speed = _PlayerController.FirstSpeed;
    }
    private void CollectPoints(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RedPoint"))
        {
            _RedPointIndex++;
            _RedSlider.value = _RedPointIndex;
            IncreasePointIndex(collision.gameObject, _SkillPointRedP);
            Audio.PlayAudio($"CollectSoundEffect", .7f);
        }
        if (collision.gameObject.CompareTag("BluePoint"))
        {
            _BluePointIndex++;
            _BlueSlider.value = _BluePointIndex;
            IncreasePointIndex(collision.gameObject, _SkillPointBlueP);
            Audio.PlayAudio($"CollectSoundEffect", .7f);
        }
        if (collision.gameObject.CompareTag("GreenPoint"))
        {
            _GreenPointIndex++;
            _GreenSlider.value = _GreenPointIndex;
            IncreasePointIndex(collision.gameObject, _SkillPointGreenP);
            Audio.PlayAudio($"CollectSoundEffect", .7f);
        }
        if (collision.gameObject.CompareTag("CollectBullet"))
        {
            Audio.PlayAudio($"CollectSoundEffect", .7f);
            _PlayerController.BulletCount = (_PlayerController.BulletCount + 20 > _PlayerController.MaXbulletCount) ? _PlayerController.MaXbulletCount : _PlayerController.BulletCount + 20;
            _PlayerController.BulletSlider();
            Instantiate(_BulletCollectedParticle, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Health"))
        {
            _PlayerController.CurrentHealth = (_PlayerController.CurrentHealth + 10 <= 100) ? _PlayerController.CurrentHealth + 10 : _PlayerController.MaxHealth;
            _PlayerController.PlayerHealthSldier();
            Instantiate(_HeathCollectedParticle, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("AdBullet"))
        {
            _BulletAdPanel.SetActive(true);
            Time.timeScale = 0;
            _UIManager.CanClick = false;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("AdHelath"))
        {
            _HealthAdPanel.SetActive(true);
            Time.timeScale = 0;
            _UIManager.CanClick = false;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("AdShield"))
        {
            _ShieldAdPanel.SetActive(true);
            Time.timeScale = 0;
            _UIManager.CanClick = false;
            Destroy(collision.gameObject);
        }
    }
    private void IncreasePointIndex(GameObject dedectedSkillPoint, ParticleSystem effect)
    {
        BeCanUsefuellTheSpecialAttack();
        Instantiate(effect, dedectedSkillPoint.transform.position, Quaternion.identity);
        Destroy(dedectedSkillPoint);
    }
    private void BeCanUsefuellTheSpecialAttack()
    {
        if (_RedPointIndex >= 5)
        {
            _PlayerAttack.CanUseFire = true;
            SetText(_RedSliderT, _RedSliderTMP, "Full", true);
        }
        else
        {
            SetText(_RedSliderT, _RedSliderTMP, $"{_RedPointIndex}/5");
        }
        if (_GreenPointIndex >= 5)
        {
            _PlayerAttack.CanUseToxic = true;
            SetText(_GreenSliderT, _GreenSliderTMP, "Full", true);
        }
        else
        {
            SetText(_GreenSliderT, _GreenSliderTMP, $"{_GreenPointIndex}/5");
        }
        if (_BluePointIndex >= 5)
        {
            _PlayerAttack.CanUseFreeze = true;
            SetText(_BlueSliderT, _BlueSliderTMP, "Full", true);
        }
        else
        {
            SetText(_BlueSliderT, _BlueSliderTMP, $"{_BluePointIndex}/5");
        }
    }
    public void SetText(Transform slider,Text text, string to, bool full = false)
    {
        text.text = to;
        if (!_canEffect) return;
        if (!full)
        {
            slider.DOPunchScale(0.75f * Vector2.one, 0.2f).OnComplete(() =>
            {
                _canEffect = true;
            }).SetUpdate(true);
        }
        else
        {
            slider.DOPunchPosition(1 * new Vector2(1,0), 0.3f).OnComplete(() =>
            {
                _canEffect = true;
            }).SetUpdate(true);
        }
        _canEffect = false;
    }
}