using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _UnlockedCanvas;
    [SerializeField] private bool _BlueUnlocked, _GreenUnlocked, _RedSkillUnlocked, _BlueSkillUnlocked, _GreenSkillUnlocked;
    [SerializeField] private PlayerController _PlayerController;

    private void Start()
    {
        _PlayerController = FindObjectOfType<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController playerController))
        {
            SetUnlock(collision);
        }
    }
    private void SetUnlock(Collider2D collision)
    {
        Canvas instiatedCanvas = Instantiate(_UnlockedCanvas, collision.transform.position, Quaternion.identity).GetComponent<Canvas>();
        instiatedCanvas.worldCamera = Camera.main;
        instiatedCanvas.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetTrigger("Unlocked");
        if (_BlueUnlocked)
        {
            _PlayerController.Blue = true;
        }
        if (_GreenUnlocked)
        {
            _PlayerController.Green = true;
        }
        if (_RedSkillUnlocked)
        {
            _PlayerController.UnlockedSkill(_PlayerController._RedSliderTMP, _PlayerController._RedSliderSpriteR, _PlayerController._RedSliderSpriteUnlocked);
        }
        if (_GreenSkillUnlocked)
        {
            _PlayerController.UnlockedSkill(_PlayerController._GreenSliderTMP, _PlayerController._GreenSliderSpriteR, _PlayerController._GreenSliderSpriteUnlocked);
        }
        if (_BlueSkillUnlocked)
        {
            _PlayerController.UnlockedSkill(_PlayerController._BlueSliderTMP, _PlayerController._BlueSliderSpriteR, _PlayerController._BlueSliderSpriteUnlocked);
        }
        Audio.PlayAudio("Unlocked", .2f);
        Destroy(gameObject);
    }
}
