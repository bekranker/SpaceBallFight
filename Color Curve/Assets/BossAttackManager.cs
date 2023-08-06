using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackManager : MonoBehaviour
{
    public static event Action AttackEvent;
    public bool CanFight;


    private void Start()
    {
        AttackEvent?.Invoke();
    }
}
