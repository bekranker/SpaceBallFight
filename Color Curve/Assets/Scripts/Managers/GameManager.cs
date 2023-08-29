using CrazyGames;
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
    [SerializeField] private Animator _Animator;
    private WaitForSecondsRealtime _sleepTime = new WaitForSecondsRealtime(1.75f);
    private EnemyManager[] _enemys;
    private Transform _playerT;
    public bool Dead;
    private void Start()
    {
        Dead = false;
        _playerT = _PlayerController.transform;
    }
    public void ChangeWave()
    {
        StartCoroutine(ChangeWaveIE());
    }
    IEnumerator ChangeWaveIE()
    {
        _ShockWaveManager.CallShockWave();
        Audio.PlayAudio("EnemysComing", .5f);
        _Animatior.SetTrigger("WaveCompleted");
        Time.timeScale = 0;
        yield return _sleepTime;
        Time.timeScale = 1;
    }
    public void DeadTime()
    {
        Dead = true;
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
        Dead = false;
        Time.timeScale = 1;
        _playerT.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
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
            Destroy(enemy.gameObject);
        }
    }
}