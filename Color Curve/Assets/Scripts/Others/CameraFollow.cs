using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField, Range(0.05f, 100)] float _Speed;
    [SerializeField] Transform _Player;
    Vector3 vect3 = new Vector3(0,0,-10);


    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(_Player.position.x, _Player.position.y, -10), ref vect3, _Speed);
    }
}
