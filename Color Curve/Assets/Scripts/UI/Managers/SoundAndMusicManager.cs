using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Audio;
using TMPro;

public class SoundAndMusicManager : MonoBehaviour
{
    [SerializeField] private ButtonClickManager _RightButtonMusic, _LeftButtonMusic;
    [SerializeField] private ButtonClickManager _RightButtonSound, _LeftButtonSound;
    [SerializeField] private TMP_Text _MusicText, _SoundText;
    [SerializeField] private AudioMixer AudioMixer;
    [SerializeField] private Vector3 _ToEffect;

    public List<float> values = new List<float>();
    public List<float> percentile = new List<float>();

    public bool _canDoEffectMusic, _canDoEffectSound;
    private int _indexMusic, _indexSound;

    private void Awake()
    {
        _canDoEffectMusic = true;
        _canDoEffectSound = true;
    }
    private void Start()
    {
        SetMusicForStart();
        SetSoundForStart();

        SetMusicButtons();
        SetSoundButtons();
    }





    private void SetMusicButtons()
    {
        _RightButtonMusic.DoSomething += SetRightButtonMusic;
        _LeftButtonMusic.DoSomething += SetLeftButtonMusic;
    }
    private void SetRightButtonMusic()
    {
        _indexMusic = (_indexMusic + 1 < values.Count) ? _indexMusic + 1 : _indexMusic;
        SetMusic(_indexMusic);
    }
    private void SetLeftButtonMusic()
    {
        _indexMusic = (_indexMusic - 1 > -1) ? _indexMusic - 1 : _indexMusic;
        SetMusic(_indexMusic);
    }


    private void SetSoundButtons()
    {
        _RightButtonSound.DoSomething += SetRightButtonSound;
        _LeftButtonSound.DoSomething += SetLeftButtonSound;
    }
    private void SetRightButtonSound()
    {
        _indexSound = (_indexSound + 1 < values.Count) ? _indexSound + 1 : _indexSound;
        SetSound(_indexSound);
    }
    private void SetLeftButtonSound()
    {
        _indexSound = (_indexSound - 1 > -1) ? _indexSound - 1 : _indexSound;
        SetSound(_indexSound);
    }





    private void SetMusicForStart()
    {
        if (CheckMusicMuted(out _indexMusic))
        {
            SetMusicMuted();
        }
        else
        {
            SetMusic(_indexMusic);
        }
    }
    private void SetSoundForStart()
    {
        if (CheckSoundMuted(out _indexSound))
        {
            SetSoundMuted();
        }
        else
        {
            SetSound(_indexSound);
        }
    }
    private void SetMusicMuted()
    {
        SetMusic(8);
    }
    private void SetSoundMuted()
    {
        SetSound(8);
    }
    private void SetMusic(int index)
    {
        if (_canDoEffectMusic)
        {
            _MusicText.transform.DOPunchScale(_ToEffect, .1f).OnComplete(() => 
            {
                _MusicText.transform.localScale = Vector2.one;
                _canDoEffectMusic = true;
            }).SetUpdate(true);
            _canDoEffectMusic = false;
        }
        _MusicText.text = $"%{percentile[_indexMusic]}";
        AudioMixer.SetFloat("Music", values[index]);
        PlayerPrefs.SetInt("Music", _indexMusic);
    }
    private void SetSound(int index)
    {
        if (_canDoEffectSound)
        {
            _SoundText.transform.DOPunchScale(_ToEffect, .1f).OnComplete(() =>
            {
                _SoundText.transform.localScale = Vector2.one;
                _canDoEffectSound = true;
            }).SetUpdate(true);
            _canDoEffectSound = false;
        }
        _SoundText.text = $"%{percentile[_indexSound]}";
        AudioMixer.SetFloat("Sound", values[index]);
        PlayerPrefs.SetInt("Sound", _indexSound);
    }




    private bool CheckMusicMuted(out int value)
    {
        if (!PlayerPrefs.HasKey("MusicMuted"))
        {
            value = PlayerPrefs.GetInt("Music", 1);
            return false;
        }
        value = 1;
        return true;
    }
    private bool CheckSoundMuted(out int value)
    {
        if (!PlayerPrefs.HasKey("SoundMuted"))
        {
            value = PlayerPrefs.GetInt("Sound", 1);
            return false;
        }
        value = 1;
        return true;
    }
}