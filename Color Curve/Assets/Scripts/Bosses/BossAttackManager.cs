using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackManager : MonoBehaviour
{
    public static event Action AttackEventStart, AttackEventUpdate;
    public bool CanFight;

    private void Start()
    {
        AttackEventStart?.Invoke();
    }
    private void Update()
    {
        AttackEventUpdate?.Invoke();
    }
}
