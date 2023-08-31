using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StupidBossDieState : MonoBehaviour
{
    [SerializeField] private BossTag _BossTag;
    [SerializeField] private GameObject _GreenPlayerParticle;
    [SerializeField] private ParticleSystem _DeadParticle;
    [SerializeField] private GameObject Tutorial;



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
        Instantiate(_GreenPlayerParticle, transform.position, Quaternion.identity);
        Instantiate(_DeadParticle, transform.position, Quaternion.identity);
        if(Tutorial != null)
            Instantiate(Tutorial, transform.position, Quaternion.identity);
        Camera.main.DOShakePosition(.1f, 3, 9, randomnessMode: ShakeRandomnessMode.Harmonic);
        _BossTag._ShockWave.CallShockWave();
        Destroy(gameObject);

       
    }
}