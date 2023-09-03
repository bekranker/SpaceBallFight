using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPoint : MonoBehaviour
{
    [SerializeField] private int _Delay;
    [SerializeField] private bool _Skill, _Bullet, _Health;
    [SerializeField] private bool _CantDestroy;
    private PlayerCanvas _playerCanvas;


    void Start()
    {
        _playerCanvas = GameObject.FindGameObjectWithTag("PlayerCanvas").GetComponent<PlayerCanvas>();
        if (!_CantDestroy)
        {
            StartCoroutine(ToDestroy());
        }
    }
    private IEnumerator ToDestroy()
    {
        yield return new WaitForSeconds(_Delay);
        GetComponent<SpriteRenderer>().DOFade(0, 2).OnComplete(() => { Destroy(gameObject); });
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Red")||
            collision.gameObject.CompareTag("Green")||
            collision.gameObject.CompareTag("Blue")
            )
        {
            if (_Skill)
            {
                _playerCanvas.EffectFireSlider();
            }
            if (_Bullet)
            {
                _playerCanvas.EffectBulletSlider();
            }
            if (_Health)
            {
                _playerCanvas.EffectHealthSlider();
            }
        }
        return;
    }
}
