using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractmovementManager : MonoBehaviour
{
    public EnemyMovementAbstract EnemyMovementAbstract;


    void Start()
    {
        EnemyMovementAbstract.OnStart(this);
    }

    void Update()
    {
        EnemyMovementAbstract.OnUpdate(this);
    }
    private void LateUpdate()
    {
        EnemyMovementAbstract.OnLateUpdate(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyMovementAbstract.TriggerEnter2D(this, collision);
    }
}
