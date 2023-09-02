using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerController _PlayerController;
    [SerializeField, Range(0.05f, 1)] public float ShootRange;
    [SerializeField] private GameObject _BulletPrefab;
    [SerializeField] private ObjectPool _ObjectPool;
    [SerializeField] private SpriteRenderer _PlayeRSpriteRenderer;
    [SerializeField] private GameObject _FireBullet, _FreezeBullet, _ToxicBullet;
    [SerializeField, Range(0, 25)] int _Range;
    public float FirstShootRange;
    public bool CanUseFire, CanUseFreeze, CanUseToxic;
    private float _shootCounter;
    private Transform _t;
    private bool _canAttackSpecial, _canAttackNormal;
    private WaitForSeconds _attackDelay2 = new WaitForSeconds(.05f);
    private WaitForSeconds _forSound = new WaitForSeconds(.3f);
    private int _attackCount;
    private int i, _forCount;


    private void Start()
    {
        FirstShootRange = ShootRange;
        _canAttackSpecial = false;
        _canAttackNormal = true;
        _t = transform;
        _shootCounter = ShootRange;
    }

    void Update()
    {
        _shootCounter -= (_shootCounter <= 0) ? 0 : Time.deltaTime;

        if (Input.GetMouseButton(0) && _canAttackNormal)
        {
            if (_PlayerController.BulletCount <= 0)
                return;
            if (_shootCounter <= 0)
            {
                BulletSpawn();
                _shootCounter = ShootRange;
            }
        }
        if ((Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Space)) && CanUseSpecialAttack())
        {
            Audio.PlayAudio($"BossShootBGNoise", .1f);
            UseSpecialAttack();
        }
    }

    private void BulletSpawn()
    {
        GameObject spawnedBulelt = _ObjectPool.TakeBullet(_t.position);
        spawnedBulelt.transform.position = _t.position;
        spawnedBulelt.GetComponent<SpriteRenderer>().color = _PlayeRSpriteRenderer.GetComponent<SpriteRenderer>().color;
        switch (_PlayerController._PlayerStates)
        {
            case PlayerState.Red:
                spawnedBulelt.transform.tag = "BulletRed";
                break;
            case PlayerState.Green:
                spawnedBulelt.transform.tag = "BulletGreen";
                break;
            case PlayerState.Blue:
                spawnedBulelt.transform.tag = "BulletBlue";
                break;
            default:
                spawnedBulelt.transform.tag = "BulletWhite";
                break;
        }
        _PlayerController.BulletCount--;
        _PlayerController.CanDropBulletF();
        _PlayerController.BulletSlider();
        Audio.PlayAudio($"shoot{_Range}", .12f);
    }
    private void UseSpecialAttack()
    {
        
        switch (_PlayerController._PlayerStates)
        {
            case PlayerState.Red:
                RedAttack();
                break;
            case PlayerState.Green:
                GreenAttack();
                break;
            case PlayerState.Blue:
                BlueAttack();
                break;
            default:
                break;
        }
        _PlayerController.CanChangestate = false;
    }
    private void RedAttack()
    {
        if (!CanUseFire) return;
        _canAttackNormal = false;
        _canAttackSpecial = true;
        StartCoroutine(SpawnBullet(_FireBullet));
    }
    private void BlueAttack()
    {
        if (!CanUseFreeze) return;
        _canAttackNormal = false;
        _canAttackSpecial = true;
        StartCoroutine(SpawnBullet(_FreezeBullet));
    }
    private void GreenAttack()
    {
        if (!CanUseToxic) return;
        _ToxicBullet.SetActive(true);
        _PlayerController.ChangeValueOfCollectedSkillPoints();
        _ToxicBullet.transform.DOScale(10 * Vector2.one, .5f).SetUpdate(true).OnComplete(() => 
        {
            StartCoroutine(ToxicDissolve());
        });
        
    }
    private IEnumerator ToxicDissolve()
    {
        yield return new WaitForSeconds(10);
        _ToxicBullet.transform.DOScale(0 * Vector2.one, .5f).SetUpdate(true).OnComplete(() =>
        {
            _ToxicBullet.SetActive(false);
            _PlayerController.CanChangestate = true;
            _canAttackNormal = true;
            _canAttackSpecial = false;
        });
    }
    private IEnumerator SpawnBullet(GameObject bullet)
    {
        _PlayerController.ChangeValueOfCollectedSkillPoints();
        while (_canAttackSpecial)
        {
            yield return _attackDelay2;
            i++;
            _forCount++;
            if (_forCount >= 16)
            {
                _attackCount++;
                if (_attackCount >= 5)
                {
                    _canAttackNormal = true;
                    switch (_PlayerController._PlayerStates)
                    {
                        case PlayerState.Red:
                            CanUseFire = false;
                            break;
                        case PlayerState.Green:
                            CanUseToxic = false;
                            break;
                        case PlayerState.Blue:
                            CanUseFreeze = false;
                            break;
                        default:
                            break;
                    }
                    _PlayerController.CanChangestate = true;
                    _attackCount = 0;
                    _forCount = 0;
                    i = 0;
                    _canAttackSpecial = false;
                    yield break;
                }
                _forCount = 0;
            }
            float angle = i * 22.5f;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Rigidbody2D rb = Instantiate(bullet, _t.position, rotation).GetComponent<Rigidbody2D>();
            rb.velocity = rb.transform.right * 15;
        }
    }
    private bool CanUseSpecialAttack()
    {
        if (_PlayerController.CurrentSlider().value >= 5)
        {
            return true;
        }
        return false;
    }
}