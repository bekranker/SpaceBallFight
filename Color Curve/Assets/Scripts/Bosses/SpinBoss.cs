using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpinBoss : MonoBehaviour
{
    [SerializeField] Transform _t;
    [SerializeField] public float _SpinSpeed;
    [SerializeField] private bool _GoCenter;
    private void Start()
    {
        Vector2 posCam = Camera.main.transform.position;
        if(!_GoCenter)
            transform.DOMove(posCam, 1);
    }
    private void Update()
    {
        _t.Rotate(new Vector3(0,0,_SpinSpeed * 90 * Time.deltaTime));
    }
}
