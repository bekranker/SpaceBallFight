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
    [SerializeField] private WaveManager _WaveManager;
    [SerializeField] private List<ButtonClickManager> _No;
    [SerializeField] private ButtonClickManager _YesBullet, _YesHealth, _YesShield;
    [SerializeField] private List<GameObject> _AdPAnels;
    public bool CanClick;
    [SerializeField] private Animator _CutScene;




    private void Start()
    {
        _PauseButton.DoSomething += PauseTheGame;
        _ResumeButton.DoSomething += ResumeTheGame;
        _ReturnToMenuButton.DoSomething += () => StartCoroutine(ReturnToMenu());
        _RestartTheGame.DoSomething += RestartTheGame;
        _BackToMenu.DoSomething += ()=> StartCoroutine(ReturnToMenu());

        _AdsWatchAndResume.DoSomething += SetWatchAndResumeAds;
        _AdsBulletButton.DoSomething += SetBulletAds;
        _AdsShieldButton.DoSomething += SetShieldAds;
        _AdsHealthButton.DoSomething += SetHealthAds;

        _YesBullet.DoSomething += SetBulletAds;
        _YesHealth.DoSomething += SetHealthAds;
        _YesShield.DoSomething += SetShieldAds;

        _No?.ForEach((no) =>
        {
            no.DoSomething += CloseAdPanels;
        });
    }

    private void CloseAdPanels()
    {
        _AdPAnels?.ForEach((panel) =>
        {
            panel.SetActive(false);
        });
        Time.timeScale = 1;
        CanClick = true;
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
        CloseAdPanels();
        CrazyAds.Instance.beginAdBreakRewarded(HealthReward, () => CanClick = true);
        CanClick = false;
    }
    private void HealthReward()
    {
        Time.timeScale = 1;
        _PlayerController.CurrentHealth = _PlayerController.MaxHealth;
        _PlayerController.PlayerHealthSldier();
        CanClick = true;

    }
    public void SetShieldAds()
    {
        CloseAdPanels();
        CrazyAds.Instance.beginAdBreakRewarded(ShieldReward, () => CanClick = true);
        CanClick = false;
    }
    private void ShieldReward()
    {
        Time.timeScale = 1;
        StartCoroutine(_PlayerController.GetAShield());
        CanClick = true;

    }
    public void SetBulletAds()
    {
        CloseAdPanels();
        CrazyAds.Instance.beginAdBreakRewarded(BulletReward, () => CanClick = true);
        CanClick = false;
    }
    private void BulletReward()
    {
        Time.timeScale = 1;
        _PlayerController.BulletCount = _PlayerController.MaXbulletCount;
        _PlayerController.BulletSlider();
        CanClick = true;
    }
    #endregion

    private void RestartTheGame()
    {
        if (!CanClick) return;
        Time.timeScale = 1;
        StartCoroutine(RestartGameCallBack());
    }
    private IEnumerator RestartGameCallBack()
    {
        _CutScene.SetTrigger("Exit");
        PlayerPrefs.SetInt("WaveIndex", (_WaveManager.WaveIndex - 1 <= 0) ? 0 : _WaveManager.WaveIndex - 1);
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ResumeTheGame()
    {
        if (!CanClick) return;
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
        if (!CanClick) return;
        if (_GameManager.Dead) return;
        Time.timeScale = 0;
        _Panel.SetActive(true);
    }
    public void OpenOrCloseUI(bool value)
    {
        if (!CanClick) return;
        _CloseTheese?.ForEach((closetheese) => { closetheese.SetActive(value); });
    }
}