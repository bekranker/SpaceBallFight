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
            PlayerPrefs.SetInt("BlueUnlocked", 1);
            _PlayerController.Blue = true;
        }
        if (_GreenUnlocked)
        {
            PlayerPrefs.SetInt("GreenUnlocked", 1);
            _PlayerController.Green = true;
        }
        if (_RedSkillUnlocked)
        {
            PlayerPrefs.SetInt("RedUnlockedSkill", 1);
            _PlayerController.LockedOrUnlockedSlider("Red", _PlayerController._RedSliderTMP, _PlayerController._RedSliderSpriteR, _PlayerController._RedSliderSprite, _PlayerController._RedSliderSpriteUnlocked);
        }
        if (_GreenSkillUnlocked)
        {
            PlayerPrefs.SetInt("GreenUnlockedSkill", 1);
            _PlayerController.LockedOrUnlockedSlider("Green", _PlayerController._GreenSliderTMP, _PlayerController._GreenSliderSpriteR, _PlayerController._GreenSliderSprite, _PlayerController._GreenSliderSpriteUnlocked);
        }
        if (_BlueSkillUnlocked)
        {
            PlayerPrefs.SetInt("BlueUnlockedSkill", 1);
            _PlayerController.LockedOrUnlockedSlider("Blue", _PlayerController._BlueSliderTMP, _PlayerController._BlueSliderSpriteR, _PlayerController._BlueSliderSprite, _PlayerController._BlueSliderSpriteUnlocked);
        }
        Audio.PlayAudio("Unlocked", .2f);
        Destroy(gameObject);
    }
}
