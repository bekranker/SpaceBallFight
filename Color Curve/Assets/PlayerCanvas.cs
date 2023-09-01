using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] private Transform _HealthEffectPos, _SkillEffectPos, _BulletEffectPos;
    [SerializeField] private List<Animator> _Texts = new();
    [SerializeField] private List<Text> _TextsT = new();
    private int _index;
    
    public void EffectHealthSlider()
    {
        IncreaseIndex();
        _TextsT[_index].text = "+5";
        SelectText();
        SelectTextT().position = _HealthEffectPos.position;

    
    }
    public void EffectBulletSlider()
    {
        IncreaseIndex();
        _TextsT[_index].text = "+20";
        SelectText();
        SelectTextT().position = _BulletEffectPos.position;
    }
    public void EffectFireSlider()
    {
        IncreaseIndex();
        _TextsT[_index].text = "+1";
        SelectText();
        SelectTextT().position = _SkillEffectPos.position;
    }
    private void IncreaseIndex() => _index = (_index + 1 > _Texts.Count) ? 0 : _index + 1;
    private void SelectText() => _Texts[_index].SetTrigger("effect");
    private Transform SelectTextT() => _Texts[_index].transform;

}