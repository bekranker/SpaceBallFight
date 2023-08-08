using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightCreateEnemy : MonoBehaviour
{
    [SerializeField] private List<GameObject> _Enemy = new List<GameObject>();
    [SerializeField] private BossTag _Tag;
    public void SpawnRandomEnemy(int enemyCount, float spawnDelay, Vector3 pos)
    {
        StartCoroutine(SpawnRandomEnemyIE(enemyCount, spawnDelay, pos));
    }
    private IEnumerator SpawnRandomEnemyIE(int enemyCount, float spawnDelay, Vector3 spawnPos)
    {
        int randEnemy = Random.Range(0, _Enemy.Count);
        for (int i = 0; i < enemyCount; i++)
        {
            yield return new WaitForSeconds(spawnDelay);
            GameObject enemy = Instantiate(_Enemy[randEnemy], spawnPos, Quaternion.identity);
            ChangeEnemyState(enemy.GetComponent<EnemyManager>());
        }
    }
    private void ChangeEnemyState(EnemyManager enemyManager)
    {
        ParticleSystem.MainModule mainPArt;
        if (enemyManager.BackgroundParticle != null)
        {
            mainPArt = enemyManager.BackgroundParticle.main;
        }
        switch (_Tag.EnemyColor)
        {
            case EnemyColor.Red:
                for (int i = 0; i < enemyManager._Sp.Count; i++)
                {
                    enemyManager._Sp[i].color = Color.red;
                }
                enemyManager.gameObject.GetComponent<EnemyManager>().EnemyColorTypes = EnemyColor.Red;
                enemyManager.gameObject.GetComponent<EnemyManager>()._NormalColor = Color.red;
                if (enemyManager.BackgroundParticle != null)
                    mainPArt.startColor = Color.red;
                if (enemyManager.Trail != null)
                {
                    enemyManager.Trail.ForEach((_trail) => { _trail.startColor = Color.red; });
                }
                break;
            case EnemyColor.Green:
                for (int i = 0; i < enemyManager._Sp.Count; i++)
                {
                    enemyManager._Sp[i].color = Color.green;
                }
                enemyManager.gameObject.GetComponent<EnemyManager>().EnemyColorTypes = EnemyColor.Green;
                enemyManager.gameObject.GetComponent<EnemyManager>()._NormalColor = Color.green;
                if (enemyManager.BackgroundParticle != null)
                    mainPArt.startColor = Color.green;
                if (enemyManager.Trail != null)
                {
                    enemyManager.Trail.ForEach((_trail) => { _trail.startColor = Color.green; });
                }
                break;
            case EnemyColor.Blue:
                for (int i = 0; i < enemyManager._Sp.Count; i++)
                {
                    enemyManager._Sp[i].color = Color.blue;
                }
                enemyManager.gameObject.GetComponent<EnemyManager>().EnemyColorTypes = EnemyColor.Blue;
                enemyManager.gameObject.GetComponent<EnemyManager>()._NormalColor = Color.blue;
                if (enemyManager.BackgroundParticle != null)
                    mainPArt.startColor = Color.blue;
                if (enemyManager.Trail != null)
                {
                    enemyManager.Trail.ForEach((_trail) => { _trail.startColor = Color.blue; });
                }
                break;
            default:
                break;
        }
    }
}
