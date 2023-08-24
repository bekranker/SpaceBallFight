using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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


    private void Start()
    {
        _PlayerController.OnPlayerStateChange += ActionChange;
    }

    private void LateUpdate()
    {
        _TurnedPart.Rotate(0, 0, _Speed * Time.deltaTime * 90);
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

    private void ChangeColors(Sprite sprite)
    {
        _ShieldIconsSp.ForEach((spRenderer) => { spRenderer.sprite = sprite; });
    }
}
