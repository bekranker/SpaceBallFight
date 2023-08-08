using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform _playerT, _t;
    private Vector2 _direction;
    float targetRotationAngle;


    private void Start()
    {
        _t = transform;
        _playerT = FindObjectOfType<PlayerController>().transform;
        _direction = _playerT.position - _t.position;
        targetRotationAngle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, targetRotationAngle);
    }
    void Update()
    {
        _direction = _playerT.position - _t.position;
        targetRotationAngle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0,0, targetRotationAngle);
    }
}
