using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    Red,
    Green,
    Blue,
}

public class PlayerController : MonoBehaviour
{
    public int MaxHealth, CurrentHealth;
    public PlayerState _PlayerStates;
    public bool CanChangestate;
    [SerializeField] private Transform _CursorPosition;
    [SerializeField] public float _Speed;
    [SerializeField] private ParticleSystem _BackgroundParticle, _PlayerBackgroundParticle;
    [SerializeField] private SpriteRenderer _Sp, _SpFace;
    [SerializeField] private CrossHair _CrossHair;
    [SerializeField] public Color _RedPlayerColor, _GreenPlayerColor, _BluePlayerColor;
    [SerializeField] private Color _RedParticleColor, _GreenParticleColor, _BlueParticleColor;
    [SerializeField] private Sprite _RedPlayerSprite, _GreenPlayerSprite, _BluePlayerSprite;
    [SerializeField] private Sprite _RedParticleSprite, _GreenParticleSprite, _BlueParticleSprite;
    [HideInInspector] public bool Green, Blue;
    [HideInInspector] public float FirstSpeed;
    private WaitForSecondsRealtime _sleepTime = new WaitForSecondsRealtime(0.1f);
    private int _index;
    private float _previousScrollPosition;
    [SerializeField] private Slider _RedSlider, _GreenSlider, _BlueSlider;
    [SerializeField] private PlayerDedection _PlayerDedection;
    [SerializeField] private PlayerAttack _PlayerAttack;
    
    private void Start()
    {
        FirstSpeed = _Speed;
        CanChangestate = true;
        _index = 1;
    }

    void Update()
    {
        LookAtTheMouse();
        WalkPlayer();
        ChangeCharacter();
    }
    
    //Movement
    private void WalkPlayer()
    {
        var inputX = Input.GetAxisRaw("Horizontal") * _Speed;
        var inputY = Input.GetAxisRaw("Vertical") * _Speed;

        transform.position += new Vector3(inputX, inputY, 0) * Time.deltaTime;
    }
    
    //taking damage
    public void TakeDamage(float damageValue)
    {
        if(CurrentHealth - damageValue <= 0)
        {
            //dead
            StartCoroutine(takeDamageIE(damageValue));
            //lose screen must be call here
            
            return;
        }
        else
        {
            //hitted by something
            StartCoroutine(takeDamageIE(damageValue));
        }
    }
    private IEnumerator takeDamageIE(float damageValue)
    {
        Color currentColor = _Sp.color;
        DecreaseHealth((int)damageValue);
        _Sp.color = Color.white;
        SetBGPColorToWhite();
        Time.timeScale = 0f;
        yield return _sleepTime;
        Time.timeScale = 1;
        _Sp.color = currentColor;
        SetParticleColor();
    }
    
    //changing player
    private void ChangeCharacter()
    {
        if (!CanChangestate) return;
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
                _SpFace.sprite = _RedPlayerSprite;
                break;
            case 2:
                if (!Green)
                {
                    return;
                }
                _PlayerStates = PlayerState.Green;
                _Sp.color = _GreenPlayerColor;
                _SpFace.sprite = _GreenPlayerSprite;
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
                _SpFace.sprite = _BluePlayerSprite;
                break;
            default:
                break;
        }
    }
    
    //looking mouse
    private void LookAtTheMouse()
    {
        var mousePos = _CursorPosition.position;
        mousePos.z = 0;
        _Sp.gameObject.transform.up = mousePos - _Sp.transform.position;
    }
    
    //changing effects
    private void SetParticleColor()
    {
        ParticleSystem.MainModule mainPart = _PlayerBackgroundParticle.main;
        ParticleSystem.TextureSheetAnimationModule texture = _PlayerBackgroundParticle.textureSheetAnimation;
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
    private void SetBGPColorToWhite()
    {
        ParticleSystem.MainModule mainPart = _PlayerBackgroundParticle.main;
        mainPart.startColor = Color.white;
    }

    //Health slider must be update here
    public void IncreaseHealth(int value) => CurrentHealth += value;
    public void DecreaseHealth(int value) => CurrentHealth -= value;
    public void IncreaseSlider() => CurrentSlider().value++;
    public void DecraseSlider() => CurrentSlider().value--;
    public Slider CurrentSlider()
    {
        switch (_PlayerStates)
        {
            case PlayerState.Red:
                return _RedSlider;
            case PlayerState.Green:
                return _GreenSlider;
            case PlayerState.Blue:
                return _BlueSlider;
            default:
                break;
        }
        return null;
    }
    public void ChangeValueOfCollectedSkillPoints()
    {
        switch (_PlayerStates)
        {
            case PlayerState.Red:
                _RedSlider.value = 0;
                _PlayerDedection._RedPointIndex = 0;
                break;
            case PlayerState.Green:
                _GreenSlider.value = 0;
                _PlayerDedection._GreenPointIndex = 0;
                break;
            case PlayerState.Blue:
                _BlueSlider.value = 0;
                _PlayerDedection._BluePointIndex = 0;
                break;
            default:
                break;
        }
    }
}