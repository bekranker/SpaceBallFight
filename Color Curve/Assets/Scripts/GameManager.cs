using System.Collections;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    [SerializeField] WaveManager _WaveManager;
    [SerializeField] private Animator _Animatior;
    [SerializeField] ShockWaveManager _ShockWaveManager;
    [SerializeField] private PlayerController _PlayerController;
    [SerializeField] private UIManager _UIManager;
    private WaitForSecondsRealtime _sleepTime = new WaitForSecondsRealtime(1.75f);
    public void ChangeWaveInfos()
    {
        StartCoroutine(ChangeWaveInfoIE());
    }
    IEnumerator ChangeWaveInfoIE()
    {
        _ShockWaveManager.CallShockWave();
        _Animatior.SetTrigger("WaveCompleted");
        Time.timeScale = 0;
        yield return _sleepTime;
        Time.timeScale = 1;
    }
}