using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDedection : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == gameObject.tag)
        {
            print("true dedection");
        }
        else
        {
            print("false dedection");
        }
    }
}
