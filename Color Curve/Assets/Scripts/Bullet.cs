using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _BulletSpeed;
    void Start()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.up = mousePos - transform.position;
        GetComponent<Rigidbody2D>().velocity = transform.up * _BulletSpeed;
    }
}
