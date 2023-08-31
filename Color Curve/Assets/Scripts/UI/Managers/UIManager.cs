using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CrazyGames;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ButtonClickManager _PauseButton, _ResumeButton, _ReturnToMenuButton, _RestartTheGame, _BackToMenu;
    [SerializeField] private GameObject _Panel;
    [SerializeField] private List<GameObject> _CloseTheese = new List<GameObject>();
    [SerializeField] private PlayerController _PlayerController;
    [SerializeField] private ButtonClickManager _AdsHealthButton, _AdsShieldButton, _AdsBulletButton, _AdsWatchAndResume;
    [SerializeField] private GameManager _GameManager;
    private bool _canClick;
    [SerializeField] private Animator _CutScene;




    private void Start()
    {
        _canClick = true;
        _PauseButton.DoSomething += PauseTheGame;
        _ResumeButton.DoSomething += ResumeTheGame;
        _ReturnToMenuButton.DoSomething += () => StartCoroutine(ReturnToMenu());
        _RestartTheGame.DoSomething += RestartTheGame;
        _BackToMenu.DoSomething += ()=> StartCoroutine(ReturnToMenu());

        _AdsWatchAndResume.DoSomething += SetWatchAndResumeAds;
        _AdsBulletButton.DoSomething += SetBulletAds;
        _AdsShieldButton.DoSomething += SetShieldAds;
        _AdsHealthButton.DoSomething += SetHealthAds;
    
    }
    
    #region AdsArea
    private void SetWatchAndResumeAds()
    {
        CrazyAds.Instance.beginAdBreakRewarded(watchAndResumeReward);
    }
    private void watchAndResumeReward()
    {
        _GameManager.BornTime();
        HealthReward();
        BulletReward();
        ShieldReward();
    }
    public void SetHealthAds()
    {
        CrazyAds.Instance.beginAdBreakRewarded(HealthReward, () => print("add didnt be loaded"));
    }
    private void HealthReward()
    {
        _PlayerController.CurrentHealth = _PlayerController.MaxHealth;
        _PlayerController.PlayerHealthSldier();
    }
    public void SetShieldAds()
    {
        CrazyAds.Instance.beginAdBreakRewarded(ShieldReward, ()=> print("add didnt be loaded"));
    }
    private void ShieldReward()
    {
        StartCoroutine(_PlayerController.GetAShield());
    }
    public void SetBulletAds()
    {
        CrazyAds.Instance.beginAdBreakRewarded(BulletReward, () => print("add didnt be loaded"));
    }
    private void BulletReward()
    {
        _PlayerController.BulletCount = _PlayerController.MaXbulletCount;
        _PlayerController.BulletSlider();
    }
    #endregion

    private void RestartTheGame()
    {
        Time.timeScale = 1;
        RestartGameCallBack();
    }
    private void RestartGameCallBack() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    public void ResumeTheGame()
    {
        Time.timeScale = 1;
        _Panel.SetActive(false);
    }
    public IEnumerator ReturnToMenu()
    {
        _CutScene.SetTrigger("Exit");
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void PauseTheGame()
    {
        if (_GameManager.Dead) return;
        Time.timeScale = 0;
        _Panel.SetActive(true);
    }
    public void OpenOrCloseUI(bool value)
    {
        _CloseTheese?.ForEach((closetheese) => { closetheese.SetActive(value); });
    }
}