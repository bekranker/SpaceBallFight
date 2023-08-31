using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDieState : MonoBehaviour
{
    [SerializeField] private ParticleSystem _DieParticle;
    [SerializeField] private GameObject _UnlockedCanvas, _Tutorial;
    [SerializeField] private bool _BlueUnlocked, _GreenUnlocked, _RedSkillUnlocked, _BlueSkillUnlocked, _GreenSkillUnlocked;
    private PlayerController _PlayerController;
    private Transform _t;

    private void Start()
    {
        _t = transform;
        _PlayerController = FindObjectOfType<PlayerController>();
    }
    private void OnEnable()
    {
        BossTag.OnDie += Die;
    }
    private void OnDisable()
    {
        BossTag.OnDie -= Die;
    }
    private void Die()
    {
        if(_DieParticle != null)
            Instantiate(_DieParticle, _t.position, Quaternion.identity);
        if (_Tutorial != null)
            Instantiate(_Tutorial, _t.position, Quaternion.identity);

        if (_BlueUnlocked)
        {
            Canvas instiatedCanvas = Instantiate(_UnlockedCanvas, _t.position, Quaternion.identity).GetComponent<Canvas>();
            instiatedCanvas.worldCamera = Camera.main;
            instiatedCanvas.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetTrigger("Unlocked");
            _PlayerController.Blue = true;
        }
        if (_GreenUnlocked)
        {
            Canvas instiatedCanvas = Instantiate(_UnlockedCanvas, _t.position, Quaternion.identity).GetComponent<Canvas>();
            instiatedCanvas.worldCamera = Camera.main;
            instiatedCanvas.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetTrigger("Unlocked");
            _PlayerController.Green = true;
        }
        if (_RedSkillUnlocked)
        {
            Canvas instiatedCanvas = Instantiate(_UnlockedCanvas, _t.position, Quaternion.identity).GetComponent<Canvas>();
            instiatedCanvas.worldCamera = Camera.main;
            instiatedCanvas.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetTrigger("Unlocked");
            _PlayerController.UnlockedSkill(_PlayerController._RedSliderTMP, _PlayerController._RedSliderSpriteR, _PlayerController._RedSliderSpriteUnlocked);
            _PlayerController.RedSkillOpened = true;
        }
        if (_GreenSkillUnlocked)
        {
            Canvas instiatedCanvas = Instantiate(_UnlockedCanvas, _t.position, Quaternion.identity).GetComponent<Canvas>();
            instiatedCanvas.worldCamera = Camera.main;
            instiatedCanvas.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetTrigger("Unlocked");
            _PlayerController.UnlockedSkill(_PlayerController._GreenSliderTMP, _PlayerController._GreenSliderSpriteR, _PlayerController._GreenSliderSpriteUnlocked);
            _PlayerController.GreenSkillOpened = true;
        }
        if (_BlueSkillUnlocked)
        {
            Canvas instiatedCanvas = Instantiate(_UnlockedCanvas, _t.position, Quaternion.identity).GetComponent<Canvas>();
            instiatedCanvas.worldCamera = Camera.main;
            instiatedCanvas.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetTrigger("Unlocked");
            _PlayerController.UnlockedSkill(_PlayerController._BlueSliderTMP, _PlayerController._BlueSliderSpriteR, _PlayerController._BlueSliderSpriteUnlocked);
            _PlayerController.BlueSkillOpened = true;
        }
        Destroy(gameObject);
    }
}
