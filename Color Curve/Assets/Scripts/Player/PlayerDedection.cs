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
    [SerializeField] private ParticleSystem _SkillPointRedP, _SkillPointBlueP, _SkillPointGreenP, _BulletCollectedParticle;
    [SerializeField] public TMP_Text _RedSliderTMP, _GreenSliderTMP, _BlueSliderTMP;
    [SerializeField] public Transform _RedSliderT, _GreenSliderT, _BlueSliderT;
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
        SetText(_RedSliderTMP, $"{_RedPointIndex}/5");
        SetText(_GreenSliderTMP, $"{_GreenPointIndex}/5");
        SetText(_BlueSliderTMP, $"{_BluePointIndex}/5");
        _t = transform;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            _PlayerController.TakeDamage(25);
        }
        if (collision.gameObject.CompareTag("Border"))
        {
            _PlayerController.TakeDamage(25);
            var direction = new Vector3(_cameraTransform.position.x, _cameraTransform.position.y, 0) - _t.position;
            transform.right = direction;
            GetComponent<Rigidbody2D>().velocity = transform.right * 15;
            if(gameObject.activeSelf)
                StartCoroutine(pushIt());
        }
        CollectPoints(collision);
        if (collision.gameObject.CompareTag("CollectBullet"))
        {
            _PlayerController.BulletCount += 15;
            _PlayerController.BulletSlider();
            Instantiate(_BulletCollectedParticle, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Audio.PlayAudio($"CollectSoundEffect", 1f);

        }
        if (!CanDedect) return;
        if (collision.CompareTag("FreezeBullet"))
        {
            StartCoroutine(decreaseSpeed());
            _PlayerController.TakeDamage(7);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("BossBullet"))
        {
            _PlayerController.TakeDamage(7);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Enemy"))
        {
            _PlayerController.TakeDamage(3);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Lazer"))
        {
            _PlayerController.TakeDamage(15);
        }
        if (collision.gameObject.CompareTag("Spike"))
        {
            _PlayerController.TakeDamage(10);
        }
    }
    private IEnumerator pushIt()
    {
        yield return new WaitForSeconds(.3f);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
            if (_PlayerController.IsTrueState(PlayerState.Red))
            {
                _RedPointIndex++;
                _RedSlider.value = _RedPointIndex;
                IncreasePointIndex(collision.gameObject, _SkillPointRedP);
                Audio.PlayAudio($"CollectSoundEffect", 1f);
            }
        }
        if (collision.gameObject.CompareTag("BluePoint"))
        {
            if (_PlayerController.IsTrueState(PlayerState.Blue))
            {
                _BluePointIndex++;
                _BlueSlider.value = _BluePointIndex;
                IncreasePointIndex(collision.gameObject, _SkillPointBlueP);
                Audio.PlayAudio($"CollectSoundEffect", 1f);
            }
        }
        if (collision.gameObject.CompareTag("GreenPoint"))
        {
            if (_PlayerController.IsTrueState(PlayerState.Green))
            {
                _GreenPointIndex++;
                _GreenSlider.value = _GreenPointIndex;
                IncreasePointIndex(collision.gameObject, _SkillPointGreenP);
                Audio.PlayAudio($"CollectSoundEffect", 1f);
            }
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
            SetText(_RedSliderTMP, "Full", true);
        }
        else
        {
            SetText(_RedSliderTMP, $"{_RedPointIndex}/5");
        }
        if (_GreenPointIndex >= 5)
        {
            _PlayerAttack.CanUseToxic = true;
            SetText(_GreenSliderTMP, "Full", true);
        }
        else
        {
            SetText(_GreenSliderTMP, $"{_GreenPointIndex}/5");
        }
        if (_BluePointIndex >= 5)
        {
            _PlayerAttack.CanUseFreeze = true;
            SetText(_BlueSliderTMP, "Full", true);
        }
        else
        {
            SetText(_BlueSliderTMP, $"{_BluePointIndex}/5");
        }
    }
    public void SetText(TMP_Text text, string to, bool full = false)
    {
        text.text = to;
        if (!_canEffect) return;
        if (!full)
        {
            _RedSliderT.DOPunchScale(0.75f * Vector2.one, 0.2f).OnComplete(() =>
            {
                _canEffect = true;
            });
        }
        else
        {
            _RedSliderT.DOPunchPosition(25 * new Vector2(1,0), 0.3f).OnComplete(() =>
            {
                _canEffect = true;
            });
        }
        _canEffect = false;
    }
}