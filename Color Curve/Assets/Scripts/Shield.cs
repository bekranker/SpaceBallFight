using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Color = UnityEngine.Color;

public class Shield : MonoBehaviour
{
    [SerializeField] private Transform _TurnedPart;
    [SerializeField] private List<Transform> _ShieldIcons;
    [SerializeField] private List<SpriteRenderer> _ShieldIconsSp;
    [SerializeField] private float _Speed;
    [SerializeField] private PlayerController _PlayerController;
    [SerializeField] SpriteRenderer _SpriteRenderer;
    [SerializeField] Sprite _SpriteRed, _SpriteGreen, _SpriteBlue;
    [SerializeField] Sprite _SpriteRedIcon, _SpriteGreenIcon, _SpriteBlueIcon;
    [SerializeField] private bool _CanDestroyBullets;
    [SerializeField] private PlayerDedection PlayerDedection;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_CanDestroyBullets)
        {
            if (collision.gameObject.CompareTag("BossBullet"))
            {
                Destroy(collision.gameObject);
            }
        }
    }
    private void Start()
    {
        _SpriteRenderer.color = new Color(255, 255, 255, 255);
        _ShieldIconsSp.ForEach((spRenderer) => { spRenderer.color = new Color(255, 255, 255, 255f); });
        if (_CanDestroyBullets)
        {
            PlayerDedection.CanDedect = false;
        }
        _PlayerController.OnPlayerStateChange += ActionChange;
        StartCoroutine(LastThreeSeconds());
    }

    private void LateUpdate()
    {
        _TurnedPart.Rotate(0, 0, _Speed * Time.unscaledDeltaTime * 90);
        _ShieldIcons.ForEach((_shield)=> _shield.transform.rotation = Quaternion.Euler(0,0,0));
    }

    private void ActionChange()
    {
        switch (_PlayerController._PlayerStates)
        {
            case PlayerState.Red:
                _SpriteRenderer.sprite = _SpriteRed;
                ChangeColors(_SpriteRedIcon);
                break;
            case PlayerState.Green:
                _SpriteRenderer.sprite = _SpriteGreen;
                ChangeColors(_SpriteGreenIcon);
                break;
            case PlayerState.Blue:
                _SpriteRenderer.sprite = _SpriteBlue;
                ChangeColors(_SpriteBlueIcon);
                break;
            default:
                break;
        }
    }
    private IEnumerator LastThreeSeconds()
    {
        yield return new WaitForSeconds(5);
        _SpriteRenderer.color = new Color(255, 255, 255, .2f);
        _ShieldIconsSp.ForEach((spRenderer) => { spRenderer.color = new Color(255,255,255, .2f); });
        yield return new WaitForSeconds(.5f);
        _SpriteRenderer.color = new Color(255, 255, 255, 255);
        _ShieldIconsSp.ForEach((spRenderer) => { spRenderer.color = new Color(255, 255, 255, 255f); });
        yield return new WaitForSeconds(.5f);
        _SpriteRenderer.color = new Color(255, 255, 255, .2f);
        _ShieldIconsSp.ForEach((spRenderer) => { spRenderer.color = new Color(255, 255, 255, .2f);});
        yield return new WaitForSeconds(.5f);
        _SpriteRenderer.color = new Color(255, 255, 255, 255);
        _ShieldIconsSp.ForEach((spRenderer) => { spRenderer.color = new Color(255, 255, 255, 255f); });
        yield return new WaitForSeconds(.5f);
        _SpriteRenderer.color = new Color(255, 255, 255, .2f);
        _ShieldIconsSp.ForEach((spRenderer) => { spRenderer.color = new Color(255, 255, 255, .2f); });
        yield return new WaitForSeconds(.5f);
        _SpriteRenderer.color = new Color(255, 255, 255, 255);
        _ShieldIconsSp.ForEach((spRenderer) => { spRenderer.color = new Color(255, 255, 255, 255); });
        yield return new WaitForSeconds(.5f);
        _SpriteRenderer.color = new Color(255, 255, 255, 255);
        _ShieldIconsSp.ForEach((spRenderer) => { spRenderer.color = new Color(255, 255, 255, 255f); });
        yield return new WaitForSeconds(.5f);
        _SpriteRenderer.color = new Color(255, 255, 255, .2f);
        _ShieldIconsSp.ForEach((spRenderer) => { spRenderer.color = new Color(255, 255, 255, .2f); });
        yield return new WaitForSeconds(.5f);
        _SpriteRenderer.color = new Color(255, 255, 255, 255);
        _ShieldIconsSp.ForEach((spRenderer) => { spRenderer.color = new Color(255, 255, 255, 255f); });
        yield return new WaitForSeconds(.5f);
        _SpriteRenderer.color = new Color(255, 255, 255, 255);
        _SpriteRenderer.color = new Color(255, 255, 255, 255);
        _ShieldIconsSp.ForEach((spRenderer) => { spRenderer.color = new Color(255, 255, 255, 255); if (_CanDestroyBullets)
            {
                PlayerDedection.CanDedect = true;
            }
        });
    }
    private void ChangeColors(Sprite sprite)
    {
        _ShieldIconsSp.ForEach((spRenderer) => { spRenderer.sprite = sprite; });
    }
}
