using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _Enemys;
    [SerializeField] private List<Transform> _To, _From;
    [SerializeField] private Vector2Int _RandomTimeTostart;
    [SerializeField] private float _Speed;
    
    private void Start()
    {
        MoveAction();
    }
    private void MoveAction()
    {
        Move();
    }
    private void Move()
    {
        Transform to = RandTo();
        Transform from = RandFrom();
        GameObject enemy = RandEnemy();

        enemy.transform.position = from.position;
        enemy.transform.DOMove(to.position, Random.Range(_Speed - 2, _Speed + 2)).SetEase(Ease.InSine).OnComplete(()=> MoveAction());
        enemy.transform.up = to.position - from.position;
    }
    private Transform RandTo()
    {
        int rand = RandNumber(0, _Enemys.Count);
        Transform to = _To[rand];
        return to;
    }
    private Transform RandFrom() => _From[RandNumber(0, _From.Count)];
    private GameObject RandEnemy()=> _Enemys[RandNumber(0, _Enemys.Count)];
    private int RandNumber(int value, int value2) => Random.Range(value, value2);

}
