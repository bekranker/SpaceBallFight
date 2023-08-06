using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;



public class SpikyBossAttack : MonoBehaviour
{
    [SerializeField] private List<GameObject> _SpikesParent = new List<GameObject>();
    private int _spikeIndex;
    [SerializeField] private float _SlideSpeed, _LookSpeed, _PushSpeed;
    private List<Transform> _spikeParentNull = new List<Transform>();
    private Queue<Transform> _spikeParentQueue = new Queue<Transform>();
    private WaitForSeconds _sleepTimeForAttack = new WaitForSeconds(.075f);
    private WaitForSeconds _sleepTime = new WaitForSeconds(.3f);
    private WaitForSeconds _nextAttackSleepTime = new WaitForSeconds(1f);
    private Transform _playerT;
    private bool _canAttack;
    private Queue<Vector3> _startPositions = new Queue<Vector3>();
    private Queue<Transform> _spikes = new Queue<Transform>();
    private int _selectedSpikeIndex;
    [SerializeField] private BossRandomMovement _BossRandomMovement;
    [SerializeField] private BossAttackManager _BossAttackManager;
    private void Start()
    {
        _playerT = FindObjectOfType<PlayerController>().transform;
        _canAttack = true;
        int count = _SpikesParent.Count;
        for (int i = 0; i < count; i++)
        {
            _spikeParentQueue.Enqueue(_SpikesParent[i].transform);
        }
        for (int i = 0; i < count; i++)
        {
            for (int a = 0; a < _SpikesParent[i].transform.childCount; a++)
            {
                _startPositions.Enqueue(_SpikesParent[i].transform.GetChild(a).position);
                _spikes.Enqueue(_SpikesParent[i].transform.GetChild(a));
            }
        }
    }
    private void OnEnable()
    {
        BossAttackManager.AttackEventUpdate += SpikeMovement;
    }
    private void OnDisable()
    {
        BossAttackManager.AttackEventUpdate -= SpikeMovement;
    }
    public void SpikeMovement()
    {
        if (!_BossAttackManager.CanFight) return; 
        if (!_canAttack) return;
        StartCoroutine(SpikeMovemenIE());
        _canAttack = false;
    }
    private IEnumerator SpikeMovemenIE()
    {
        if (_spikeParentQueue.Count == 0)
        {
            for (int i = 0; i < 20; i++)
            {
                _selectedSpikeIndex++;
                Transform spike = _spikes.Dequeue();
                Vector3 _startPos = _startPositions.Dequeue();
                if (_selectedSpikeIndex <= 5)
                    spike.rotation = Quaternion.Euler(0, 0, 90);
                else if (_selectedSpikeIndex <= 10)
                    spike.rotation = Quaternion.Euler(0, 0, -180);
                else if (_selectedSpikeIndex <= 15)
                    spike.rotation = Quaternion.Euler(0, 0, 270);
                else
                    spike.rotation = Quaternion.Euler(0, 0, 0);

                spike.position = _startPos;
                spike.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
            _BossRandomMovement.CanMove = true;
            _BossAttackManager.CanFight = false;
            yield break;
        }

        //-----------------------Cut
        //Attacking with spikes

        yield return _sleepTime;
        Transform selectedSpikes = _spikeParentQueue.Dequeue();
        int childCount = selectedSpikes.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform spike = selectedSpikes.GetChild(i);
            yield return _sleepTimeForAttack;
            MoveALittleBit(spike);
        }
        yield return _sleepTime;
        for (int i = 0; i < childCount; i++)
        {
            Transform spike = selectedSpikes.GetChild(i);
            yield return _sleepTimeForAttack;
            SetDirection(spike);
        }
        yield return _sleepTime;
        for (int i = 0; i < childCount; i++)
        {
            Rigidbody2D rb = selectedSpikes.GetChild(i).GetComponent<Rigidbody2D>();
            yield return _sleepTimeForAttack;
            PushSpike(rb);
        }
        yield return _nextAttackSleepTime;
        _canAttack = true;
        SpikeMovement();
    }
    private void MoveALittleBit(Transform spikeT)
    {
        spikeT.DOMove(spikeT.position + spikeT.right, _SlideSpeed);
    }
    private void SetDirection(Transform spikeT)
    {
        Vector3 direction = _playerT.position - spikeT.position;
        float targetRotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        spikeT.DORotate(new Vector3(0, 0, targetRotationAngle), _LookSpeed);
    }
    private void PushSpike(Rigidbody2D spikeRb)
    {
        spikeRb.velocity = spikeRb.transform.right * _PushSpeed;
    }
}
