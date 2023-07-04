using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HexagonManager : EnemyMovementAbstract
{
    [HideInInspector] public PlayerController _PlayerController;
    [SerializeField] private float _Speed;
    [SerializeField] private Transform BodyOne, BodyTwo;

    public override void OnStart(AbstractmovementManager abstractmovementManager)
    {
        _PlayerController = FindObjectOfType<PlayerController>();
        LookToPlayer();
    }
    public override void OnUpdate(AbstractmovementManager abstractmovementManager) 
    {
        transform.position += transform.up * _Speed * Time.deltaTime;
        TurnBody();
    }
    public override void OnLateUpdate(AbstractmovementManager abstractmovementManager) { }
    private void LookToPlayer() => transform.up = new Vector3(_PlayerController.gameObject.transform.position.x, _PlayerController.gameObject.transform.position.y, 0) - new Vector3(transform.position.x, transform.position.y, 0);
    public override void TriggerEnter2D(AbstractmovementManager abstractmovementManager, Collider2D collision)
    {
        BounceFromEdge();
    }
    private void BounceFromEdge()
    {
        LookToPlayer();
    }
    private void TurnBody()
    {
        if (BodyOne != null)
            BodyOne.transform.Rotate(0, 0, 90 * Time.deltaTime * -2.5f);
        if (BodyTwo != null)
            BodyTwo.transform.Rotate(0, 0, 90 * Time.deltaTime * 2.5f);
    }

}