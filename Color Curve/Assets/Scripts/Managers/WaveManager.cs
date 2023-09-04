using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] public List<WaveDatas> _WaveData = new List<WaveDatas>();
    [SerializeField] private Spawneranager _SpawnerManager;
    [SerializeField] private GameManager _GameManager;
    [SerializeField] private TMP_Text _ScoreText;
    [SerializeField] private RemaningTimeManager _RemaningTimeManager;
    [SerializeField] private PlayerController _PlayerController;
    private WaitForSeconds _waitForWave = new WaitForSeconds(1.75f);
    public int WaveIndex;



    private void Start()
    {
        WaveIndex = PlayerPrefs.GetInt("WaveIndex", 0);
        SetPrefs();
        ChangeScrore();
        _RemaningTimeManager.SetRemaningTime(_WaveData[WaveIndex].WaveTimeCount);
    }
    public IEnumerator NextWave()
    {
        yield return _waitForWave;
        _RemaningTimeManager.CanIncrease = true;
        _RemaningTimeManager.SetRemaningTime(_WaveData[WaveIndex].WaveTimeCount);
    }
    public void ChangeScrore() => _ScoreText.text = _SpawnerManager.KilledEnemyCount.ToString();
    public void IncreaseKilledEnemy() 
    {
        _SpawnerManager.KilledEnemyCount++;
        ChangeScrore();
    }
    public void SaveWave()
    {
        PlayerPrefs.SetInt("WaveIndex", WaveIndex);
    }

    private void SetPrefs()
    {
        switch (WaveIndex)
        {
            case >= 7:
                _PlayerController.BlueSkillOpened = true;
                _PlayerController.GreenSkillOpened = true;
                _PlayerController.RedSkillOpened = true;
                _PlayerController.Blue = true;
                _PlayerController.Green = true;
                _PlayerController.Red = true;
                _PlayerController.UnlockedSkill(_PlayerController._RedSliderTMP, _PlayerController._RedSliderSpriteR, _PlayerController._RedSliderSpriteUnlocked);
                _PlayerController.UnlockedSkill(_PlayerController._GreenSliderTMP, _PlayerController._GreenSliderSpriteR, _PlayerController._GreenSliderSpriteUnlocked);
                _PlayerController.UnlockedSkill(_PlayerController._BlueSliderTMP, _PlayerController._BlueSliderSpriteR, _PlayerController._BlueSliderSpriteUnlocked);
                break;
            case >= 6:
                _PlayerController.BlueSkillOpened = true;
                _PlayerController.GreenSkillOpened = true;
                _PlayerController.RedSkillOpened = true;
                _PlayerController.Blue = true;
                _PlayerController.Green = true;
                _PlayerController.Red = true;
                _PlayerController.UnlockedSkill(_PlayerController._RedSliderTMP, _PlayerController._RedSliderSpriteR, _PlayerController._RedSliderSpriteUnlocked);
                _PlayerController.UnlockedSkill(_PlayerController._GreenSliderTMP, _PlayerController._GreenSliderSpriteR, _PlayerController._GreenSliderSpriteUnlocked);
                _PlayerController.UnlockedSkill(_PlayerController._BlueSliderTMP, _PlayerController._BlueSliderSpriteR, _PlayerController._BlueSliderSpriteUnlocked);
                break;
            case >= 5:
                _PlayerController.BlueSkillOpened = true;
                _PlayerController.GreenSkillOpened = true;
                _PlayerController.RedSkillOpened = true;
                _PlayerController.Blue = true;
                _PlayerController.Green = true;
                _PlayerController.Red = true;
                _PlayerController.UnlockedSkill(_PlayerController._RedSliderTMP, _PlayerController._RedSliderSpriteR, _PlayerController._RedSliderSpriteUnlocked);
                _PlayerController.UnlockedSkill(_PlayerController._GreenSliderTMP, _PlayerController._GreenSliderSpriteR, _PlayerController._GreenSliderSpriteUnlocked);
                _PlayerController.UnlockedSkill(_PlayerController._BlueSliderTMP, _PlayerController._BlueSliderSpriteR, _PlayerController._BlueSliderSpriteUnlocked);

                break;
            case >= 4:
                _PlayerController.GreenSkillOpened = true;
                _PlayerController.RedSkillOpened = true;
                _PlayerController.Blue = true;
                _PlayerController.Green = true;
                _PlayerController.Red = true;
                _PlayerController.UnlockedSkill(_PlayerController._RedSliderTMP, _PlayerController._RedSliderSpriteR, _PlayerController._RedSliderSpriteUnlocked);
                _PlayerController.UnlockedSkill(_PlayerController._GreenSliderTMP, _PlayerController._GreenSliderSpriteR, _PlayerController._GreenSliderSpriteUnlocked);

                break;
            case >= 3:
                _PlayerController.RedSkillOpened = true;
                _PlayerController.Blue = true;
                _PlayerController.Green = true;
                _PlayerController.Red = true;
                _PlayerController.UnlockedSkill(_PlayerController._RedSliderTMP, _PlayerController._RedSliderSpriteR, _PlayerController._RedSliderSpriteUnlocked);
                break;
            case >= 2:
                _PlayerController.Blue = true;
                _PlayerController.Green = true;
                _PlayerController.Red = true;
                break;
            case >= 1:
                _PlayerController.Green = true;
                _PlayerController.Red = true;
                break;
            case >= 0:
                _PlayerController.Red = true;
                break;
            default:
                break;
        }
    }
}
