using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastBossAttack : MonoBehaviour
{

    [SerializeField] private BossTag _BossTag;
    [SerializeField] private SpinBoss _SpinBoss;
    [SerializeField] private GameObject _BulletPrefabForRed, _BulletPrefabForBlue, _BulletPrefabForGreen;
    [SerializeField] private BossFightCreateEnemy _BossFightCreateEnemy;
    [SerializeField] private float _BulletSpeed, _BulletCountForEachPoint;
    [SerializeField] private BossPlayerFollow _BossPlayerFollow;
    [SerializeField] private List<Transform> _SpawnPoint;
    [SerializeField] private List<GameObject> _Lazers;

    private WaitForSeconds _changeAttackDelay = new WaitForSeconds(3);
    private WaitForSeconds _sleepTimeFirst = new WaitForSeconds(2);
    private WaitForSeconds _shootDelay = new WaitForSeconds(.01f);
    private WaitForSeconds _stateChange = new WaitForSeconds(3f);
    private float _firstSpeed;
    private Transform _t;
  

    private void Start()
    {
        _firstSpeed = _SpinBoss._SpinSpeed;
        _t = transform;
        StartCoroutine(AttackAction());
    }

    private IEnumerator AttackAction()
    {
        yield return _sleepTimeFirst;
        RedAttack();
    }

    private void RedAttack()
    {
        StateChange(EnemyColor.Red);
        RedAction();
    }
    private void BlueAttack()
    {
        StateChange(EnemyColor.Blue);
        BlueAction();
    }
    private void GreenAttack()
    {
        StateChange(EnemyColor.Green);
        GreenAction();
    }

    //Red attack 
    #region RedAttack
    private void RedAction()=> StartCoroutine(AttackIE());
    private IEnumerator AttackIE()
    {
        yield return _sleepTimeFirst;
        _SpinBoss._SpinSpeed *= 1.5f;
        LazerSet(false);
        _BossPlayerFollow.CanFollow = false;
        //---------------------------------------Cut---------------------------------------
        StartCoroutine(SpawnBullets(_BulletPrefabForRed, _BulletSpeed));
        yield return _changeAttackDelay;
        _SpinBoss._SpinSpeed *= 1.2f;
        createEnemys();
        yield return _changeAttackDelay;
        _SpinBoss._SpinSpeed = _firstSpeed;
        _BossPlayerFollow.CanFollow = true;
        LazerSet(true);
        yield return _changeAttackDelay;
        BlueAttack();
    }

    #endregion

    //Blue Attack
    #region BlueAttack
    private void BlueAction()=> StartCoroutine(ShootIEForBlue());
    private IEnumerator ShootIEForBlue()
    {
        _BossPlayerFollow.CanFollow = false;
        yield return _sleepTimeFirst;
        LazerSet(false);
        StartCoroutine(SpawnBullets(_BulletPrefabForBlue, _BulletSpeed));
        yield return _changeAttackDelay;
        _SpinBoss._SpinSpeed *= 1.2f;
        createEnemys();
        yield return _changeAttackDelay;
        _SpinBoss._SpinSpeed = _firstSpeed;
        _BossPlayerFollow.CanFollow = true;
        LazerSet(true);
        yield return _changeAttackDelay;
        GreenAttack();
    }
    #endregion

    //Green Attack
    #region GreenAttack
    private void GreenAction() => StartCoroutine(ShootIEForGreen());
    private IEnumerator ShootIEForGreen()
    {
        _BossPlayerFollow.CanFollow = false;
        yield return _sleepTimeFirst;
        LazerSet(false);
        StartCoroutine(SpawnBullets(_BulletPrefabForGreen, Random.Range(_BulletSpeed, _BulletSpeed + 5)));
        yield return _changeAttackDelay;
        _SpinBoss._SpinSpeed += 0.1f;
        createEnemys();
        yield return _changeAttackDelay;
        _SpinBoss._SpinSpeed = _firstSpeed;
        _BossPlayerFollow.CanFollow = true;
        LazerSet(true);
        yield return _changeAttackDelay;
        StartCoroutine(AttackAction());
    }
    #endregion

    private void StateChange(EnemyColor enemyColor)
    {
        _BossTag.Setcolor(enemyColor);
    }
    private IEnumerator SpawnBullets(GameObject bullet, float speed)
    {
        _SpinBoss._SpinSpeed += 0.1f;
        for (int i = 0; i < _BulletCountForEachPoint; i++)
        {
            yield return _shootDelay;
            float angle = i * 45;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Rigidbody2D rb = Instantiate(bullet, _t.position, rotation).GetComponent<Rigidbody2D>();
            PushBulet(rb, speed);
        }
    }
    private void createEnemys()
    {
        _BossFightCreateEnemy.SpawnRandomEnemy(10, .3f, _SpawnPoint);
    }
    private void PushBulet(Rigidbody2D rb, float speed)
    {
        rb.velocity = speed * rb.transform.right;
    }
    private void LazerSet(bool value)
    {
        _Lazers.ForEach((lazer) =>
        {
            lazer.SetActive(value);
        });
    }
}
