using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBulet : MonoBehaviour
{
    WaitForSeconds _sleepTime = new WaitForSeconds(1);
    [SerializeField] private GameObject _BulletExlopdePrefab;


    void Start()
    {
        StartCoroutine(greenParticle());
    }
    IEnumerator greenParticle()
    {
        yield return _sleepTime;
        Instantiate(_BulletExlopdePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
