using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
    Red,
    Green,
    Blue,
}

public class PlayerController : MonoBehaviour
{
    public PlayerState _PlayerStates;
    [SerializeField] private Transform _CursorPosition;
    [SerializeField] private float _Speed;
    [SerializeField] private ParticleSystem _BackgroundParticle;
    [SerializeField] private SpriteRenderer _Sp;
    [SerializeField] private CrossHair _CrossHair;
    [SerializeField] public Color _RedPlayerColor, _GreenPlayerColor, _BluePlayerColor;
    [SerializeField] private Color _RedParticleColor, _GreenParticleColor, _BlueParticleColor;
    [SerializeField] private Sprite _RedPlayerSprite, _GreenPlayerSprite, _BluePlayerSprite;
    [SerializeField] private Sprite _RedParticleSprite, _GreenParticleSprite, _BlueParticleSprite;
    public bool Green, Blue;
    private int _index;
    private float _previousScrollPosition;
    private void Start()
    {
        _index = 1;
    }

    void Update()
    {
        LookAtTheMouse();
        WalkPlayer();
        ChangeCharacter();
    }
    private void WalkPlayer()
    {
        var inputX = Input.GetAxisRaw("Horizontal") * _Speed;
        var inputY = Input.GetAxisRaw("Vertical") * _Speed;

        transform.position += new Vector3(inputX, inputY, 0) * Time.deltaTime;
    }
    private void ChangeCharacter()
    {
        float currentScrollPosition = Input.mouseScrollDelta.y;

        if (currentScrollPosition > .1f)
        {
            _index = (_index - 1 < 1) ? 3 : _index - 1;
            ChangeState();
            _CrossHair.ChangeColorCross();
            SetParticleColor();
        }
        else if (currentScrollPosition < -.1f)
        {
            _index = (_index + 1 > 3) ? 1 : _index + 1;
            ChangeState();
            _CrossHair.ChangeColorCross();
            SetParticleColor();
        }

        _previousScrollPosition = currentScrollPosition;
        if (Input.GetKeyDown(KeyCode.E))
        {
            _index = (_index + 1 > 3) ? 1 : _index + 1;
            ChangeState();
            _CrossHair.ChangeColorCross();
            SetParticleColor();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _index = (_index - 1 < 1) ? 3 : _index - 1;
            ChangeState();
            _CrossHair.ChangeColorCross();
            SetParticleColor();
        }
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            _index = 1;
            ChangeState();
            _CrossHair.ChangeColorCross();
            SetParticleColor();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            _index = 2;
            ChangeState();
            _CrossHair.ChangeColorCross();
            SetParticleColor();
        }
        if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            _index = 3;
            ChangeState();
            _CrossHair.ChangeColorCross();
            SetParticleColor(); 
        }
    }
    private void ChangeState()
    {
        switch (_index)
        {
            case 1:
                _PlayerStates = PlayerState.Red;
                _Sp.color = _RedPlayerColor;
                _Sp.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _RedPlayerSprite;
                break;
            case 2:
                if (!Green)
                {
                    return;
                }
                _PlayerStates = PlayerState.Green;
                _Sp.color = _GreenPlayerColor;
                _Sp.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _GreenPlayerSprite;
                break;
            case 3:
                if (!Blue)
                {
                    _index = 1;
                    ChangeState();
                    return;
                }
                _PlayerStates = PlayerState.Blue;
                _Sp.color = _BluePlayerColor;
                _Sp.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _BluePlayerSprite;
                break;
            default:
                break;
        }
    }
    private void LookAtTheMouse()
    {
        var mousePos = _CursorPosition.position;
        mousePos.z = 0;
        _Sp.gameObject.transform.up = mousePos - _Sp.transform.position;
    }
    private void SetParticleColor()
    {
        ParticleSystem.MainModule mainPart = gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().main;
        ParticleSystem.TextureSheetAnimationModule texture = gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().textureSheetAnimation;
        switch (_PlayerStates)
        {
            case PlayerState.Red:
                mainPart.startColor = _RedParticleColor;
                texture.SetSprite(0, _RedParticleSprite);
                break;
            case PlayerState.Green:
                if (!Green) return;
                mainPart.startColor = _GreenParticleColor;
                texture.SetSprite(0, _GreenParticleSprite);
                break;
            case PlayerState.Blue:
                if (!Blue) return;
                mainPart.startColor = _BlueParticleColor;
                texture.SetSprite(0, _BlueParticleSprite);
                break;
            default:
                break;
        }
    }
}
