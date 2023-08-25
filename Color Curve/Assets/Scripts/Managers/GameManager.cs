using System.Collections;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    [SerializeField] private WaveManager _WaveManager;
    [SerializeField] private Animator _Animatior;
    [SerializeField] private ShockWaveManager _ShockWaveManager;
    [SerializeField] private PlayerController _PlayerController;
    [SerializeField] private UIManager _UIManager;
    [SerializeField] private GameObject _GameOverScreen;
    [SerializeField] private ScoreManager _ScoreManager;
    [SerializeField] private AudioSource _BgMusic;
    private WaitForSecondsRealtime _sleepTime = new WaitForSecondsRealtime(1.75f);
    private EnemyManager[] _enemys;
    private Transform _playerT;
    private void Start()
    {
        _playerT = _PlayerController.transform;
    }

    public void ChangeWaveInfos()
    {
        StartCoroutine(ChangeWaveInfoIE());
    }
    IEnumerator ChangeWaveInfoIE()
    {
        _ShockWaveManager.CallShockWave();
        Audio.PlayAudio("EnemysComing");
        _Animatior.SetTrigger("WaveCompleted");
        Time.timeScale = 0;
        yield return _sleepTime;
        Time.timeScale = 1;
    }
    public void DeadTime()
    {
        Time.timeScale = 0;
        _GameOverScreen.SetActive(true);
        _ShockWaveManager.CallShockWave();
        _ScoreManager.SaveScore();
        _WaveManager.SaveWave();
        _UIManager.OpenOrCloseUI(false);
        DeleteAllEnemys();
        _PlayerController.gameObject.SetActive(false);
    }
    public void BornTime()
    {
        Time.timeScale = 1;
        _BgMusic.Play();
        _GameOverScreen.SetActive(false);
        _ScoreManager.SaveScore();
        _WaveManager.SaveWave();
        _UIManager.OpenOrCloseUI(true);
        _PlayerController.gameObject.SetActive(true);
    }
    private void DeleteAllEnemys()
    {
        _enemys = FindObjectsOfType<EnemyManager>();
        foreach (EnemyManager enemy in _enemys)
        {
            enemy.TakeDamage(1000, _playerT);
        }
    }
}