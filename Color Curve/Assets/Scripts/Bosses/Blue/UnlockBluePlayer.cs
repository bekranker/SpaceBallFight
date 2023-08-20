using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _UnlockedCanvas;
    [SerializeField] private bool _BlueUnlocked, _GreenUnlocked;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController playerController))
        {
            SetUnlock();
        }
    }
    private void SetUnlock()
    {
        Canvas instiatedCanvas = Instantiate(_UnlockedCanvas).GetComponent<Canvas>();
        instiatedCanvas.worldCamera = Camera.main;
        instiatedCanvas.GetComponent<Animator>().SetTrigger("Unlocked");
        if (_BlueUnlocked)
        {
            PlayerPrefs.SetInt("BlueUnlocked", 1);
        }
        if (_GreenUnlocked)
        {
            PlayerPrefs.SetInt("GreenUnlocked", 1);
        }
        Destroy(gameObject);
    }
}
