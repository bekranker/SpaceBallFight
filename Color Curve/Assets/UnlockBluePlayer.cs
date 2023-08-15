using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockPlayer : MonoBehaviour
{
	public enum PlayerType
	{
		Blue,
		Green,
		Red
	}
	public PlayerType playerType;
    [SerializeField] private Sprite _Sprite;
    private Image image;
    private Animator _unlockedAnimation;
    private GameObject _blueBallBG, _greenBallBG;
    private void Start()
    {
        _unlockedAnimation = GameObject.FindGameObjectWithTag("Unlocked").GetComponent<Animator>();
        _blueBallBG = GameObject.FindGameObjectWithTag("BlueBallBG");
        _greenBallBG = GameObject.FindGameObjectWithTag("GreenBallBG");
        image = GameObject.FindGameObjectWithTag("UnlockedImage").GetComponent<Image>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController playerController))
        {
            SetUnlock(playerController);
        }
    }
    private void SetUnlock(PlayerController playerController)
    {
        switch (playerType)
        {
            case PlayerType.Blue:
                playerController.Blue = true;
                image.sprite = _Sprite;
                break;
                
            case PlayerType.Green:
                playerController.Green = true;
                break;
            default:
                break;
        }
        _unlockedAnimation.SetTrigger("Unlocked");
        Destroy(gameObject);
    }
}
