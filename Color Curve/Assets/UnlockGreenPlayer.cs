using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockGreenPlayer : MonoBehaviour
{
    private Animator _UnlockedAnimation;
    private void Start()
    {
        _UnlockedAnimation = GameObject.FindGameObjectWithTag("Unlocked").GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController playerController))
        {
            print("sa");
            playerController.Green = true;
            _UnlockedAnimation.SetTrigger("Unlocked");
            Destroy(gameObject);
        }
    }
}
