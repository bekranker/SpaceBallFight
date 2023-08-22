using CrazyGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private ButtonClickManager _StartButton, _SettingsButton;
    [SerializeField] private GameObject _SettingsPanel;
    [SerializeField] private List<GameObject> _Objects = new List<GameObject>();


    private void Start()
    {
        CrazyEvents.Instance.GameplayStart();
        _StartButton.DoSomething += StartButton;
        _SettingsButton.DoSomething += SettingsButton;
    }
    private void StartButton()
    {
        CrazyAds.Instance.beginAdBreak(StartReward, StartReward);
    }
    private void StartReward() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    private void SettingsButton()
    {
        CrazyEvents.Instance.GameplayStop();
        _SettingsPanel.SetActive(true);
        CloseOrOpen(false);
    }
    public void CloseOrOpen(bool state)
    {
        _Objects.ForEach((_objects)=>_objects.SetActive(state));
    }
}
