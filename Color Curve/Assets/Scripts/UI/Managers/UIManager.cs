using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CrazyGames;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ButtonClickManager _PauseButton, _ResumeButton, _ReturnToMenuButton;
    [SerializeField] private GameObject _Panel;
    [SerializeField] private List<GameObject> _CloseTheese = new List<GameObject>();
    [SerializeField] private PlayerController _PlayerController;
    [SerializeField] private ButtonClickManager _AdsHealthButton, _AdsEnergyButton, _AdsShieldButton, _AdsBulletButton;


    private bool _canClick;





    private void Start()
    {
        _canClick = true;
        _PauseButton.DoSomething += PauseTheGame;
        _ResumeButton.DoSomething += ResumeTheGame;
        _ReturnToMenuButton.DoSomething += ReturnToMenu;
    }

    public void ResumeTheGame()
    {
        Time.timeScale = 1;
        _Panel.SetActive(false);
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void PauseTheGame()
    {
        Time.timeScale = 0;
        _Panel.SetActive(true);
    }
    public void OpenOrCloseUI(bool value)
    {
        _CloseTheese?.ForEach((closetheese) => { closetheese.SetActive(value); });
    }
}
