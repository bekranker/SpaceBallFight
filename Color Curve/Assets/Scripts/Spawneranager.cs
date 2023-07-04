using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawneranager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _Enemys = new List<GameObject>();
    [SerializeField] private List<Transform> _Spawners = new List<Transform>();


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