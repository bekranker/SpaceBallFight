using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovementAbstract : MonoBehaviour
{
    public abstract void OnUpdate(AbstractmovementManager abstractmovementManager);
    public abstract void OnStart(AbstractmovementManager abstractmovementManager);
    public abstract void OnLateUpdate(AbstractmovementManager abstractmovementManager);
    public abstract void TriggerEnter2D(AbstractmovementManager abstractmovementManager, Collider2D collision);
}
