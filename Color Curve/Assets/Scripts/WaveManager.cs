using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    [SerializeField] public List<WaveDatas> _WaveData = new List<WaveDatas>();
    [SerializeField] private Spawneranager _SpawnerManager;
    [SerializeField] private Animator _Animatior;
    [SerializeField] private PlayerController _PlayerController;
    private float _timeCounter;
    public int _waveIndex;
    public int KilledEnemyCount;
    public int _spawnedEnemyCount;

    private void Start()
    {
        KilledEnemyCount = 0;
        _spawnedEnemyCount = 0;
        _waveIndex = 0;
        _timeCounter = _WaveData[_waveIndex].SpawnDelay;
    }

    private void Update()
    {
        if (_timeCounter <= 0 && _spawnedEnemyCount < _WaveData[_waveIndex].EnemyCount)
        {
            _spawnedEnemyCount++;
            GameObject spawnedEnemey = _SpawnerManager.SpawnEnemy(_WaveData[_waveIndex].EnemyTypes[Random.Range(0, _WaveData[_waveIndex].EnemyTypes.Count)]);
            ChangeEnemyState(spawnedEnemey.GetComponent<EnemyManager>());
            _timeCounter = _WaveData[_waveIndex].SpawnDelay;
        }
        if(_timeCounter > 0)
            _timeCounter -= Time.deltaTime;
    }
    public IEnumerator NextWave()
    {
        _Animatior.SetTrigger("WaveCompleted");
        yield return new WaitForSeconds(2f);
        _waveIndex++;
        _timeCounter = _WaveData[_waveIndex].SpawnDelay;
        KilledEnemyCount = 0;
        _spawnedEnemyCount = 0;
    }
    private void ChangeEnemyState(EnemyManager enemyManager)
    {
        EnemyColor colorType = _WaveData[_waveIndex].StartColor[Random.Range(0, _WaveData[_waveIndex].StartColor.Count)];
        ParticleSystem.MainModule mainPArt;
        if (enemyManager.BackgroundParticle != null)
        {
            mainPArt = enemyManager.BackgroundParticle.main;
        }
        
        
        switch (colorType)
        {
            case EnemyColor.Red:
                for (int i = 0; i < enemyManager._Sp.Count; i++)
                {
                    enemyManager._Sp[i].color = _PlayerController._RedPlayerColor;
                }
                enemyManager.gameObject.GetComponent<EnemyManager>().EnemyColorTypes = EnemyColor.Red;
                enemyManager.gameObject.GetComponent<EnemyManager>()._NormalColor = Color.red;
                if (enemyManager.BackgroundParticle != null)
                    mainPArt.startColor = _PlayerController._RedPlayerColor;
                if (enemyManager.Trail != null)
                {
                    enemyManager.Trail.ForEach((_trail) => { _trail.startColor = _PlayerController._RedPlayerColor; });
                }

                break;
            case EnemyColor.Green:
                for (int i = 0; i < enemyManager._Sp.Count; i++)
                {
                    enemyManager._Sp[i].color = _PlayerController._GreenPlayerColor;
                }
                enemyManager.gameObject.GetComponent<EnemyManager>().EnemyColorTypes = EnemyColor.Green;
                enemyManager.gameObject.GetComponent<EnemyManager>()._NormalColor = _PlayerController._GreenPlayerColor;
                if (enemyManager.BackgroundParticle != null)
                    mainPArt.startColor = Color.green;
                if (enemyManager.Trail != null)
                {
                    enemyManager.Trail.ForEach((_trail) => { _trail.startColor = _PlayerController._GreenPlayerColor; });
                }
                break;
            case EnemyColor.Blue:
                for (int i = 0; i < enemyManager._Sp.Count; i++)
                {
                    enemyManager._Sp[i].color = _PlayerController._BluePlayerColor;
                }
                enemyManager.gameObject.GetComponent<EnemyManager>().EnemyColorTypes = EnemyColor.Blue;
                enemyManager.gameObject.GetComponent<EnemyManager>()._NormalColor = _PlayerController._BluePlayerColor;
                if (enemyManager.BackgroundParticle != null)
                    mainPArt.startColor = _PlayerController._BluePlayerColor;
                if (enemyManager.Trail != null)
                {
                    enemyManager.Trail.ForEach((_trail) => { _trail.startColor = _PlayerController._BluePlayerColor; });
                }
                break;
            case EnemyColor.White:
                for (int i = 0; i < enemyManager._Sp.Count; i++)
                {
                    enemyManager._Sp[i].color = Color.white;
                }
                enemyManager.gameObject.GetComponent<EnemyManager>().EnemyColorTypes = EnemyColor.White;
                enemyManager.gameObject.GetComponent<EnemyManager>()._NormalColor = Color.white;
                if (enemyManager.BackgroundParticle != null)
                    mainPArt.startColor = Color.white;
                if (enemyManager.Trail != null)
                {
                    enemyManager.Trail.ForEach((_trail) => { _trail.startColor = Color.white; });
                }
                break;
            default:
                break;
        }
    }
}
