using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttack : MonoBehaviour
{
    [SerializeField] private Animator _Animation;
    [SerializeField] private BossFightCreateEnemy _EnemyCreateManager;
    [SerializeField] private List<Transform> _SpawnPoint;
    [SerializeField] private BossTag _Tag;
    private int _rand;
    private WaitForSeconds _sleep = new WaitForSeconds(5);
    private List<EnemyColor> _enemyColors = new List<EnemyColor> 
    {
        EnemyColor.Red,
        EnemyColor.Green,
        EnemyColor.Blue,
    };


    private void Start()
    {
        _Tag.Setcolor(_enemyColors[Random.Range(0, _enemyColors.Count)]);
        Invoke("Attack", 2);
    }
    private void Attack() => StartCoroutine(AttackIE());
    IEnumerator AttackIE()
    {
        yield return _sleep;
        _Tag.Setcolor(_enemyColors[Random.Range(0, _enemyColors.Count)]);
        _rand = Random.Range(1, 3);
        _Animation.SetTrigger($"Attack{_rand}");
        int randEnemyCount = 7;
        _EnemyCreateManager.SpawnRandomEnemy(randEnemyCount, .4f, _SpawnPoint);
        Attack();
    }
}
    