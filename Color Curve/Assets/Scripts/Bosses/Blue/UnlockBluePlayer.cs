using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _UnlockedCanvas;
    [SerializeField] private bool _BlueUnlocked, _GreenUnlocked;
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
        Audio.PlayAudio("Unlocked", .5f);
        Destroy(gameObject);
    }
}
