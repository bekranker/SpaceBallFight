using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBulet : MonoBehaviour
{
    [SerializeField] private GameObject _BulletExlopdePrefab;
    private int _randWait;

    void Start()
    {
        _randWait = Random.Range(1, 3);
        greenParticle();
    }
    void greenParticle()
    {
        Destroy(gameObject, _randWait);
    }
    private void OnDestroy()
    {
        Instantiate(_BulletExlopdePrefab, transform.position, Quaternion.identity);
    }
}
