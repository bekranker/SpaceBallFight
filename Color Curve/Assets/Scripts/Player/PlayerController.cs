using DG.Tweening;
using System;
using System.Collections;
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
    public bool Green, Blue, Red;
    [HideInInspector] public float FirstSpeed;
    private WaitForSecondsRealtime _sleepTime = new WaitForSecondsRealtime(0.1f);
    private int _index;
    private float _previousScrollPosition;
    [SerializeField] private Slider _RedSlider, _GreenSlider, _BlueSlider;
    [SerializeField] public Text _RedSliderTMP, _GreenSliderTMP, _BlueSliderTMP;
    [SerializeField] public Sprite _RedSliderSprite, _GreenSliderSprite, _BlueSliderSprite;
    [SerializeField] public Sprite _RedSliderSpriteUnlocked, _GreenSliderSpriteUnlocked, _BlueSliderSpriteUnlocked;
    [SerializeField] public Image _RedSliderSpriteR, _GreenSliderSpriteR, _BlueSliderSpriteR;
    [SerializeField] private GameObject _RedSliderG, _GreenSliderG, _BlueSliderG;
    [SerializeField] private PlayerDedection _PlayerDedection;
    [SerializeField] private PlayerAttack _PlayerAttack;
    [SerializeField] private Slider _PlayerHealth;
    [SerializeField] private Text _PlayerHealthTMP;
    [SerializeField] private Transform _PlayerHealthT;
    [SerializeField] private float _SliderEffectSpeed, _SliderEffectScale;
    [SerializeField] private GameManager _GameManager;
    [SerializeField] private GameObject _Shield, _RingUpgrade;
    [SerializeField] private ParticleSystem _DieEffect;
    [SerializeField] public int BulletCount, MaXbulletCount;
    [SerializeField] private Slider _BulletSlider;
    [SerializeField] private Transform _BulletSliderT;
    [SerializeField] private Text _BulletSliderTMP;
    [SerializeField] private CameraFollow _CameraFollow;
    [SerializeField] public Action OnPlayerStateChange;
    [SerializeField] private AudioSource _Bg;
    [SerializeField] private WaveManager _WaveManager;
    [SerializeField] private VariableJoystick _VariableJoystick;
    private Transform _t;
    private bool _canChange, _canEffect, _canDamage;
    private WaitForSeconds _delayForAdsReward = new WaitForSeconds(10);
    private Vector2 _lockPosition;
    public bool LockPlayer, RedSkillOpened, GreenSkillOpened, BlueSkillOpened;
    private Camera _camera;
    private float _inputX, _inputY;
    Vector3 _go;
    public bool CanDropBullet, CanDropHealth;
    
    
    private void Start()
    {
        _camera = Camera.main;
        _t = transform;
        CurrentHealth = MaxHealth;
        _canDamage = true;
        _canEffect = true;
        _canChange = true;
        CanChangestate = true;
        FirstSpeed = _Speed;
        _index = 1;
        SetBulletSliderBeReady(_WaveManager._WaveData[_WaveManager.WaveIndex].MaxBulletSize);
        PlayerHealthSldier();
        ChangeState();
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
        _inputX = Input.GetAxisRaw("Horizontal") * _Speed;
        _inputY = Input.GetAxisRaw("Vertical") * _Speed;
        _go = new Vector3(_inputX, _inputY).normalized * 10;

        if (LockPlayer)
        {
            if (Mathf.Abs(_t.position.x - _camera.transform.position.x) < 18 && Mathf.Abs(_t.position.y - _camera.transform.position.y) < 9)
            {
                _t.position += _go * Time.deltaTime;
            }
            else
            {
                var direction = new Vector3(_camera.transform.position.x, _camera.transform.position.y, 0) -  _t.position ;
                _t.position += Vector3.ClampMagnitude(direction.normalized, 0.01f);
            }
        }
        else
        {
            _t.position += _go * Time.deltaTime;
        }
        
    }
    //taking damage
    public void TakeDamage(float damageValue)
    {
        if(CurrentHealth - damageValue <= 0)
        {
            _Bg.Stop();
            _GameManager.DeadTime();
            Dead();
            DecreaseHealth((int)damageValue);
            Audio.PlayAudio("amsesi", .25f);
            CanDropHealthF();
            PlayerHealthSldier();
            PlayerPrefs.SetInt("WaveIndex", (_WaveManager.WaveIndex - 1 < 0) ? 0 : _WaveManager.WaveIndex - 1);
            return;
        }
        else
        {
            //hitted by something
            Audio.PlayAudio("EnemyHit", .25f);
            if (_canDamage)
            {
                StartCoroutine(takeDamageIE());
                _canDamage = false;
            }
            DecreaseHealth((int)damageValue);
            PlayerHealthSldier();
            CanDropHealthF();
        }
    }
    private IEnumerator takeDamageIE()
    {
        Color currentColor = _Sp.color;
        CanChangestate = false;
        _Sp.color = Color.white;
        SetBGPColorToWhite();
        Time.timeScale = 0f;
        yield return _sleepTime;
        CanChangestate = true;
        _canDamage = true;
        Time.timeScale = 1;
        _Sp.color = currentColor;
        SetParticleColor();
    }
    public void Dead()
    {
        Color currentColor = _Sp.color;
        SetParticleColor();
        _Sp.color = currentColor;
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
        OnPlayerStateChange?.Invoke();
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
                    if (Blue)
                    {
                        _index = 3;
                        ChangeState();
                        return;
                    }
                    else
                    {
                        _index = 1;
                        ChangeState();
                        return;
                    }
                }
                _PlayerStates = PlayerState.Green;
                _Sp.color = _GreenPlayerColor;
                _SpFace.sprite = _GreenPlayerSprite;
                OpenSlider(_GreenSliderG);
                break;
            case 3:
                if (!Blue)
                {
                    if (Green)
                    {
                        _index = 2;
                        ChangeState();
                        return;
                    }
                    else
                    {
                        _index = 1;
                        ChangeState();
                        return;
                    }
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
    public void UnlockedSkill(Text slidertext, Image sliderSpriteRenderer, Sprite sliderSpriteUnlocked)
    {
        slidertext.text = "0/5";
        sliderSpriteRenderer.sprite = sliderSpriteUnlocked;
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
    public void DecreaseHealth(int value) => CurrentHealth -= (CurrentHealth - value < 0) ? CurrentHealth : value;
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
                _PlayerDedection.SetText(_PlayerDedection._RedSliderT, _PlayerDedection._RedSliderTMP, "0/5");
                break;
            case PlayerState.Green:
                _GreenSlider.value = 0;
                _PlayerDedection._GreenPointIndex = 0;
                _PlayerDedection.SetText(_PlayerDedection._GreenSliderT, _PlayerDedection._GreenSliderTMP, "0/5");
                break;
            case PlayerState.Blue:
                _BlueSlider.value = 0;
                _PlayerDedection._BluePointIndex = 0;
                _PlayerDedection.SetText(_PlayerDedection._BlueSliderT, _PlayerDedection._BlueSliderTMP, "0/5");
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
        _PlayerHealth.value = CurrentHealth;
        if (!_canChange) return;
        if (_PlayerHealthT == null) return;
        _PlayerHealthT.DOPunchPosition(_SliderEffectScale * new Vector3(1, 0), _SliderEffectSpeed).OnComplete(() =>
        {
            _canChange = true;
        }).SetUpdate(true);
        _canChange = false;
    }
    public IEnumerator IncreaseSpeedAndAttack()
    {
        _RingUpgrade.SetActive(true);
        DamageMultipilier *= 3;
        _Speed *= 2;
        _CameraFollow._Speed = 0.1f;
        _PlayerAttack.ShootRange = 0.05f;
        yield return _delayForAdsReward;
        _PlayerAttack.ShootRange = _PlayerAttack.FirstShootRange;
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
    public void SetBulletSliderBeReady(int max = 1000)
    {
        MaXbulletCount = max;
        BulletCount = MaXbulletCount;
        _BulletSlider.maxValue = MaXbulletCount;
        _BulletSlider.value = BulletCount;
        SetBulletText(MaXbulletCount);
    }
    public void BulletSlider()
    {
        _BulletSlider.value = BulletCount;
        SetBulletText(BulletCount);
        if (!_canEffect) return;
        _BulletSliderT.DOPunchScale(.2f * Vector2.one, 0.07f).OnComplete(() => _canEffect = true).SetUpdate(true);
        _canEffect = false;
    }
    public void SetBulletText(int count)
    {
        _BulletSliderTMP.text = $"{count}";
    }
    public void SetHealthText(string value)
    {
        _PlayerHealthTMP.text = $"{value}";
    }
    public void CanDropHealthF()
    {
        if (CanDropHealth) return;
        
        else if (CurrentHealth <= 50 && CurrentHealth > 49)
        {
            CanDropHealth = true;
        }
        else if (CurrentHealth <= 20 && CurrentHealth > 19)
        {
            CanDropHealth = true;
        }
    }
    public void CanDropBulletF()
    {
        if (CanDropBullet) return;
        if (BulletCount <= (MaXbulletCount * 20) / 100 && BulletCount > ((MaXbulletCount * 20) / 100) - .1f)
        {
            CanDropBullet = true;
        }
        else if(BulletCount <= (MaXbulletCount * 50) / 100 && BulletCount > ((MaXbulletCount * 50) / 100) - .1f)
        {
            CanDropBullet = true;
        }
        else if (BulletCount <= (MaXbulletCount * 70) / 100 && BulletCount > ((MaXbulletCount * 70) / 100) - .1f)
        {
            CanDropBullet = true;
        }
    }
}