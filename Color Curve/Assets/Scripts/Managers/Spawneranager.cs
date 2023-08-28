using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawneranager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _Enemys = new List<GameObject>();
    [SerializeField] private List<Transform> _Spawners = new List<Transform>();
    [SerializeField] private WaveManager _WaveManager;
    [SerializeField] private PlayerController _PlayerController;
    [SerializeField] private BossManager _BossManager;

    private float _timeCounter;
    public bool CanSpawn;
    public int KilledEnemyCount;
    private void Start()
    {
        CanSpawn = true;
        KilledEnemyCount = 0;
        _timeCounter = _WaveManager._WaveData[_WaveManager.WaveIndex].SpawnDelay;
    }
    private void Update()
    {
        if (!CanSpawn) return;
        if (_timeCounter <= 0)
        {
            GameObject spawnedEnemey = SpawnEnemy(_WaveManager._WaveData[_WaveManager.WaveIndex].EnemyTypes[Random.Range(0, _WaveManager._WaveData[_WaveManager.WaveIndex].EnemyTypes.Count)]);
            ChangeEnemyState(spawnedEnemey.GetComponent<EnemyManager>());
            _timeCounter = _WaveManager._WaveData[_WaveManager.WaveIndex].SpawnDelay;
        }
        if (_timeCounter > 0)
            _timeCounter -= Time.deltaTime;
    }
    public void SpawnBoss()
    {
        Instantiate(_WaveManager._WaveData[_WaveManager.WaveIndex].BossPrefab, new Vector3(_BossManager._BossSpawnPoint.position.x, _BossManager._BossSpawnPoint.position.y, 0), Quaternion.identity);
    }
    private void ChangeEnemyState(EnemyManager enemyManager)
    {
        EnemyColor colorType = _WaveManager._WaveData[_WaveManager.WaveIndex].StartColor[Random.Range(0, _WaveManager._WaveData[_WaveManager.WaveIndex].StartColor.Count)];
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
            default:
                break;
        }
    }
    public GameObject SpawnEnemy(EnemyTypes enemyType)
    {
        switch (enemyType)
        {
            case EnemyTypes.Triangle:
                GameObject spawnedEnemyTriangle = SpawnEnemy(_Enemys[0]);
                spawnedEnemyTriangle.GetComponent<EnemyManager>().EnemyTypes = EnemyTypes.Triangle;
                return spawnedEnemyTriangle;
            case EnemyTypes.Square:
                GameObject spawnedEnemySquare = SpawnEnemy(_Enemys[1]);
                spawnedEnemySquare.GetComponent<EnemyManager>().EnemyTypes = EnemyTypes.Square;
                return spawnedEnemySquare;
            case EnemyTypes.Hexagon:
                GameObject spawnedEnemyHexagon = SpawnEnemy(_Enemys[2]);
                spawnedEnemyHexagon.GetComponent<EnemyManager>().EnemyTypes = EnemyTypes.Hexagon;
                return spawnedEnemyHexagon;
            case EnemyTypes.X:
                GameObject spawnedEnemyX = SpawnEnemy(_Enemys[Random.Range(0, 3)]);
                spawnedEnemyX.GetComponent<EnemyManager>().EnemyTypes = EnemyTypes.X;
                return spawnedEnemyX;
            default:
                break;
        }
        return null;
    }
    private GameObject SpawnEnemy(GameObject enemyPrefab) 
    {
        int randSpawner = Random.Range(0, _Spawners.Count);

        GameObject spawnedEnemy = Instantiate(enemyPrefab, new Vector2(_Spawners[randSpawner].position.x - Random.Range(-_Spawners[randSpawner].localScale.x,
            _Spawners[randSpawner].localScale.x),
            _Spawners[randSpawner].position.y - Random.Range(-_Spawners[randSpawner].localScale.y,
            _Spawners[randSpawner].localScale.y)), Quaternion.identity);

        return spawnedEnemy;
    } 
}