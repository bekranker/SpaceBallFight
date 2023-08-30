using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RemaningTimeManager : MonoBehaviour
{
    [SerializeField] private float _StartValue = 0f;
    [SerializeField] private float _IncreaseSpeed = 1f;
    [SerializeField] private WaveManager _WaveManager;
    [SerializeField] private TMP_Text _Text;
    [SerializeField] private GameManager _GameManager;
    [SerializeField] private BossManager _BossManager;
    [SerializeField] private Spawneranager _SpawnerManager;
    public float CurrentCounter;
    public bool CanIncrease;
    public bool CanDecrease;

    void Start()
    {
        CanIncrease = true;
        CurrentCounter = _StartValue;
        RefreshText(_Text);
        StartCoroutine(waitForBegin());
    }
    void Update()
    {
        if (CanDecrease)
        {
            DecreaseTime();
            RefreshText(_Text);
        }
        SwitchWave();
    }
    private IEnumerator waitForBegin()
    {
        yield return new WaitForSeconds(1);
        CanDecrease = true;
    }
    private void SwitchWave()
    {
        if (CurrentCounter <= 0)
        {
            if (CanIncrease)
            {
                if (_WaveManager._WaveData[_WaveManager.WaveIndex].Boss)
                {
                    //Boss call
                    _SpawnerManager.CanSpawn = false;
                    _BossManager.BeginBossFight();
                }
                else
                {
                    _WaveManager.WaveIndex++;
                    _GameManager.ChangeWave();
                }
            }
            StartCoroutine(_WaveManager.NextWave());
            CanIncrease = false;
        }
    }
    private void DecreaseTime()
    {
        if (CurrentCounter <= 0) return;
        CurrentCounter -= _IncreaseSpeed * Time.deltaTime;
    }
    public void RefreshText(TMP_Text text)
    {
        if (CurrentCounter <= 0) return;

        int minute = Mathf.FloorToInt(CurrentCounter / 60f);
        int second = Mathf.FloorToInt(CurrentCounter % 60f);

        text.text = minute.ToString("00") + ":" + second.ToString("00");
    }
    public void SetRemaningTime(float value)
    {
        CurrentCounter = value;
        RefreshText(_Text);
    }
}
