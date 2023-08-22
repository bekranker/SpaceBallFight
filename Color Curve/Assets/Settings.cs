using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private MenuManager _MenuManager;
    [SerializeField] private ButtonClickManager _BackToMenuButton;

    private void Start()
    {
        _BackToMenuButton.DoSomething += ClosePanel;
    }

    private void ClosePanel()
    {
        _MenuManager.CloseOrOpen(true);
        gameObject.SetActive(false);
    }

}