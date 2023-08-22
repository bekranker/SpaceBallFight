using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum PlayerState
{
    Red,
    Green,
    Blue,
}

public class PlayerController : MonoBehaviour
{
    public int DamageMultipilier;
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
    public bool Green, Blue;
    [HideInInspector] public float FirstSpeed;
    private WaitForSecondsRealtime _sleepTime = new WaitForSecondsRealtime(0.1f);
    private int _index;
    private float _previousScrollPosition;
    [SerializeField] private Slider _RedSlider, _GreenSlider, _BlueSlider;
    [SerializeField] private GameObject _RedSliderG, _GreenSliderG, _BlueSliderG;
    [SerializeField] private PlayerDedection _PlayerDedection;
    [SerializeField] private PlayerAttack _PlayerAttack;
    [SerializeField] private Slider _PlayerHealth;
    [SerializeField] private TMP_Text _PlayerHealthTMP;
    [SerializeField] private Transform _PlayerHealthT;
    [SerializeField] private float _SliderEffectSpeed, _SliderEffectScale;
    [SerializeField] private GameManager _GameManager;
    [SerializeField] private GameObject _Shield, _RingUpgrade;
    [SerializeField] private ParticleSystem _DieEffect;
    [SerializeField] public int BulletCount, MaXbulletCount;
    [SerializeField] private Slider _BulletSlider;
    [SerializeField] private Transform _BulletSliderT;
    [SerializeField] private TMP_Text _BulletSliderTMP;
    [SerializeField] private CameraFollow _CameraFollow;
    private bool _canChange, _canEffect;
    private WaitForSeconds _delayForAdsReward = new WaitForSeconds(10);

    private void Start()
    {
        CurrentHealth = MaxHealth;
        _canEffect = true;
        _canChange = true;
        CanChangestate = true;
        FirstSpeed = _Speed;
        _index = 1;
        Green = (PlayerPrefs.HasKey("GreenUnlocked")) ? true: false;
        Blue = (PlayerPrefs.HasKey("BlueUnlocked")) ? true: false;
        BulletSlider();
        PlayerHealthSldier();
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
            _GameManager.DeadTime();
            return;
        }
        else
        {
            //hitted by something
            StartCoroutine(takeDamageIE(damageValue));
        }
        PlayerHealthSldier();
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
    public void Dead()
    {
        Instantiate(_DieEffect, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
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
                OpenSlider(_RedSliderG);
                break;
            case 2:
                if (!Green)
                {
                    return;
                }
                _PlayerStates = PlayerState.Green;
                _Sp.color = _GreenPlayerColor;
                _SpFace.sprite = _GreenPlayerSprite;
                OpenSlider(_GreenSliderG);
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
                OpenSlider(_BlueSliderG);
                break;
            default:
                break;
        }
    }
    private void OpenSlider(GameObject slider)
    {
        _RedSliderG.SetActive(false);
        _GreenSliderG.SetActive(false);
        _BlueSliderG.SetActive(false);
        slider.SetActive(true);
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
                _PlayerDedection.SetText(_PlayerDedection._RedSliderTMP, "0/5");
                break;
            case PlayerState.Green:
                _GreenSlider.value = 0;
                _PlayerDedection._GreenPointIndex = 0;
                _PlayerDedection.SetText(_PlayerDedection._GreenSliderTMP, "0/5");
                break;
            case PlayerState.Blue:
                _BlueSlider.value = 0;
                _PlayerDedection._BluePointIndex = 0;
                _PlayerDedection.SetText(_PlayerDedection._BlueSliderTMP, "0/5");
                break;
            default:
                break;
        }
    }
    public bool IsTrueState(PlayerState states)
    {
        return states == _PlayerStates;
    }
    public void PlayerHealthSldier()
    {
        SetHealthText(CurrentHealth.ToString());
        if (!_canChange) return;
        _PlayerHealth.value = CurrentHealth;
        _PlayerHealthT.DOPunchPosition(_SliderEffectScale * new Vector3(1, 0), _SliderEffectSpeed).OnComplete(() =>
        {
            _canChange = true;
        });
        _canChange = false;
    }
    public IEnumerator IncreaseSpeedAndAttack()
    {
        _RingUpgrade.SetActive(true);
        DamageMultipilier *= 3;
        _Speed *= 2;
        _CameraFollow._Speed = 0.1f;
        yield return _delayForAdsReward;
        _CameraFollow._Speed = 0.28988f;
        _Speed = FirstSpeed;
        DamageMultipilier = 1;
        _RingUpgrade.SetActive(false);
    }
    public IEnumerator GetAShield()
    {
        _Shield.SetActive(true);
        yield return _delayForAdsReward;
        _Shield.SetActive(false);
    }
    public void BulletSlider() 
    {
        _BulletSlider.value = BulletCount;
        SetBulletText(BulletCount);
        if (!_canEffect) return;
        _BulletSliderT.DOPunchScale(.1f * Vector2.one, .1f).OnComplete(() => _canEffect = true);
        _canEffect = false;
    }
    public void SetBulletText(int count)
    {
        _BulletSliderTMP.text = $"{count}/{MaXbulletCount}";
    }
    public void SetHealthText(string value)
    {
        _PlayerHealthTMP.text = $"{value}/{MaxHealth}";
    }
}