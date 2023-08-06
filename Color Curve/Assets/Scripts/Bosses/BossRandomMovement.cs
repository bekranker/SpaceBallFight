using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRandomMovement : MonoBehaviour
{
    [SerializeField] private float _Speed;
    public bool CanMove;
    private Camera _mainCamera;
    private Transform _t;
    private GameObject _g;
    private Transform _cameraT;
    private Vector3 _point;
    private bool _canMove;
    private float randomX, randomY;
    [SerializeField] private BossAttackManager _BossAttackManager;
    private float _RandomFightBeginCounter;

    private void Start()
    {
        InvokeRepeating("BeginAttack", 0, .5f);
        _t = transform;
        _g = gameObject;
        _mainCamera = Camera.main;
        _cameraT = transform;
    }
    private void LateUpdate()
    {
        GoToPoint();
    }
    private void GoToPoint()
    {
        if (!CanMove) return;
        if (_canMove)
        {
            randomX = Random.Range(_cameraT.position.x + 5, _cameraT.position.x - 5);
            randomY = Random.Range(_cameraT.position.y + 3, _cameraT.position.y - 3);
            _point = new Vector2(randomX, randomY);
            _canMove = false;
        }
        _t.position = Vector2.MoveTowards(_t.position, _point, _Speed * Time.deltaTime);
        SetRandNumbers();
    }
    private void SetRandNumbers()
    {
        if (_t.position == _point)
        {
            _canMove = true;
        }
    }
    private void BeginAttack()
    {
        if (_BossAttackManager.CanFight) return;
        StartCoroutine(BeginAttackIE());
    }
    private IEnumerator BeginAttackIE()
    {
        _RandomFightBeginCounter = Random.Range(1, 2);
        yield return new WaitForSeconds(_RandomFightBeginCounter);
        CanMove = false;
        _BossAttackManager.CanFight = true;
    }
}
