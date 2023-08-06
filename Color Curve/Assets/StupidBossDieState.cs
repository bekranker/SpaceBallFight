using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StupidBossDieState : MonoBehaviour
{
    [SerializeField] private BossTag _BossTag;
    [SerializeField] private GameObject _GreenPlayerParticle;

    private void OnEnable()
    {
        BossTag.OnDie += UnlockGreenPlayer;
    }
    private void OnDisable()
    {
        BossTag.OnDie -= UnlockGreenPlayer;
    }
    private void UnlockGreenPlayer()
    {
        Instantiate(_GreenPlayerParticle, _BossTag._player.position + new Vector3(1,0,0), Quaternion.identity);
    }
}
