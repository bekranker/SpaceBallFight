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
    [SerializeField] private Animator _Animator;




    private void Start()
    {
        _StartButton.DoSomething += () => StartCoroutine(StartButton());
        _SettingsButton.DoSomething += SettingsButton;
    }
    private IEnumerator StartButton()
    {
        _Animator.SetTrigger("Exit");
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void SettingsButton()
    {
        _SettingsPanel.SetActive(true);
        CloseOrOpen(false);
    }
    public void CloseOrOpen(bool state)
    {
        _Objects.ForEach((_objects)=>_objects.SetActive(state));
    }
}
