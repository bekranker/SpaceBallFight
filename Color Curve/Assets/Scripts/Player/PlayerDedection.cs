using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDedection : MonoBehaviour
{
    [SerializeField] private int _RedPointIndex, _BluePointIndex, _GreenPointIndex;
    [SerializeField] private PlayerAttack _PlayerAttack;
    [SerializeField] private ScoreManager _ScoreManager;
    [SerializeField] private Slider _RedSlider, _GreenSlider, _BlueSlider;
    private Transform _t;
    private void Start()
    {
        _t = transform;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CollectPoints(collision);
        CollectXPs(collision);
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
            IncreasePointIndex(_RedPointIndex, collision.gameObject);
            _RedSlider.value++;
        }
        if (collision.gameObject.CompareTag("BluePoint"))
        {
            IncreasePointIndex(_BluePointIndex, collision.gameObject);
            _BlueSlider.value++;
        }
        if (collision.gameObject.CompareTag("GreenPoint"))
        {
            IncreasePointIndex(_GreenPointIndex, collision.gameObject);
            _GreenSlider.value++;
        }
    }
    private void IncreasePointIndex(int outValue, GameObject dedectedSkillPoint)
    {
        outValue++;
        BeCanUsefuellTheSpecialAttack();
        Destroy(dedectedSkillPoint);
    }
    private void BeCanUsefuellTheSpecialAttack()
    {
        //Increase there own slider right here.

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