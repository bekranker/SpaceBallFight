using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SpikyBossAttack : MonoBehaviour
{
    [SerializeField] private List<GameObject> _SpikesParent = new List<GameObject>();
    private int _spikeIndex;
    [SerializeField] private float _SlideSpeed, _LookSpeed, _PushSpeed;
    private Queue<Transform> _spikeParentQueue = new Queue<Transform>();
    private WaitForSeconds _sleepTimeForAttack = new WaitForSeconds(.05f);
    private WaitForSeconds _sleepTime = new WaitForSeconds(.05f);
    private WaitForSeconds _sleepTime2 = new WaitForSeconds(1f);
    private WaitForSeconds _nextAttackSleepTime = new WaitForSeconds(.3f);
    private WaitForSeconds _nextAttackSleepTimeFirst = new WaitForSeconds(.5f);
    private WaitForSeconds _waitForEnemys = new WaitForSeconds(8.5f);
    private Transform _playerT;
    private bool _canAttack;
    private Queue<Vector3> _startPositions = new Queue<Vector3>();
    private Queue<Transform> _spikes = new Queue<Transform>();
    private int _selectedSpikeIndex;
    [SerializeField] private List<Transform> _SpawnerT;
    [SerializeField] private BossFightCreateEnemy _BossFightCreateEnemy;
    [SerializeField] private BossPlayerFollow _BossPlayerFollow;
    [SerializeField] private BossAttackManager _BossAttackManager;
    [SerializeField] private BossTag _BossTag;
    private bool _can;
    int _indexForSpawnPoint;
    private void Start()
    {
        _can = true;
        _indexForSpawnPoint = 0;
        _playerT = FindObjectOfType<PlayerController>().transform;
        _canAttack = true;
        int count = _SpikesParent.Count;
        for (int i = 0; i < count; i++)
        {
            _spikeParentQueue.Enqueue(_SpikesParent[i].transform);
        }
        _BossFightCreateEnemy.SpawnRandomEnemy(10, .75f, _SpawnerT);
        Repeate();
    }
    private void Repeate() => InvokeRepeating("SpikeMovement", 1, .5f);
    public void SpikeMovement()
    {
        if (_BossAttackManager.CanFight && _canAttack)
        {
            StartCoroutine(SpikeMovemenIE());
        }
    }
    private IEnumerator SpikeMovemenIE()
    {
        _canAttack = false;
        if (_can)
        {
            yield return _waitForEnemys;
            _can = false;
        }
        yield return _nextAttackSleepTimeFirst;
        _BossPlayerFollow.CanFollow = false;

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
            _BossPlayerFollow.CanFollow = true;
            _BossFightCreateEnemy.SpawnRandomEnemy(7, .75f, _SpawnerT);
            yield return _waitForEnemys;
            _BossAttackManager.CanFight = true;
            _canAttack = true;
            for (int i = 0; i < 4; i++)
            {
                _spikeParentQueue.Enqueue(_SpikesParent[i].transform);
            }
            _selectedSpikeIndex = 0;
            yield break;
        }
        //-----------------------Cut
        //Attacking with spikes

        yield return _sleepTime;
        Transform selectedSpikes = _spikeParentQueue.Dequeue();
        int childCount = selectedSpikes.childCount;
        for (int i = 0; i < childCount; i++)
        {
            _spikes.Enqueue(selectedSpikes.GetChild(i));
            _startPositions.Enqueue(selectedSpikes.GetChild(i).position);
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
        _BossFightCreateEnemy.SpawnRandomEnemy(1, .5f, _SpawnerT);
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
