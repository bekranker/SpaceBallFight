using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerController _PlayerController;
    [SerializeField, Range(0.05f, 1)] private float _ShootRange;
    [SerializeField] private GameObject _BulletPrefab;
    [SerializeField] private ObjectPool _ObjectPool;
    [SerializeField] private SpriteRenderer _PlayeRSpriteRenderer;
    public bool CanUseFire, CanUseFreeze, CanUseToxic;
    private float _shootCounter;


    private void Start()
    {
        _shootCounter = _ShootRange;
    }

    void Update()
    {
        _shootCounter -= Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            if (_shootCounter <= 0)
            {
                BulletSpawn();
                _shootCounter = _ShootRange;
            }
        }
    }

    private void BulletSpawn()
    {
        GameObject spawnedBulelt = _ObjectPool.TakeBullet(transform.position);
        spawnedBulelt.transform.position = transform.position;
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
    }
}