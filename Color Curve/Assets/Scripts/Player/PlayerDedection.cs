using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDedection : MonoBehaviour
{
    [SerializeField] public int _RedPointIndex, _BluePointIndex, _GreenPointIndex;
    [SerializeField] private PlayerAttack _PlayerAttack;
    [SerializeField] private PlayerController _PlayerController;
    [SerializeField] private ScoreManager _ScoreManager;
    [SerializeField] private Slider _RedSlider, _GreenSlider, _BlueSlider;
    [SerializeField] private ParticleSystem _SkillPointRedP, _SkillPointBlueP, _SkillPointGreenP;
    private WaitForSeconds WaitForSeconds = new WaitForSeconds(1);
    private Transform _t;
    private void Start()
    {
        _t = transform;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CollectPoints(collision);
        CollectXPs(collision);
        if (collision.CompareTag("FreezeBullet"))
        {
            StartCoroutine(decreaseSpeed());
            _PlayerController.TakeDamage(7);
        }
        if (collision.CompareTag("BossBullet"))
        {
            _PlayerController.TakeDamage(7);
        }
        if (collision.CompareTag("Enemy"))
        {
            _PlayerController.TakeDamage(3);
            Destroy(collision.gameObject);
        }
    }
    private IEnumerator decreaseSpeed()
    {
        _PlayerController._Speed /= 2;
        yield return WaitForSeconds;
        _PlayerController._Speed = _PlayerController.FirstSpeed;
    }
    private void CollectXPs(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("XP"))
        {
            _ScoreManager.IncreaseScore(15, _t);
            Destroy(collision.gameObject);
        }
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
            }
        }
        if (collision.gameObject.CompareTag("BluePoint"))
        {
            if (_PlayerController.IsTrueState(PlayerState.Blue))
            {
                _BluePointIndex++;
                _BlueSlider.value = _BluePointIndex;
                IncreasePointIndex(collision.gameObject, _SkillPointBlueP);
            }
            
        }
        if (collision.gameObject.CompareTag("GreenPoint"))
        {
            if (_PlayerController.IsTrueState(PlayerState.Green))
            {
                _GreenPointIndex++;
                _GreenSlider.value = _GreenPointIndex;
                IncreasePointIndex(collision.gameObject, _SkillPointGreenP);
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
        }
        if (_GreenPointIndex >= 5)
        {
            _PlayerAttack.CanUseToxic = true;
        }
        if (_BluePointIndex >= 5)
        {
            _PlayerAttack.CanUseFreeze = true;
        }
    }
}