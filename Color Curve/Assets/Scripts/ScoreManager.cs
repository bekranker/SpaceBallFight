using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


public class ScoreManager : MonoBehaviour
{
    public int Score;
    [SerializeField] ObjectPool _Pool;
    [SerializeField] private TMP_Text _Score;
    [SerializeField] private CrossHair _CrossHair;
    [SerializeField] private WaveManager _WaveManager;
    public void IncreaseScore(int value, Transform pos)
    {

        if (_WaveManager._spawnedEnemyCount == _WaveManager._WaveData[_WaveManager._waveIndex].EnemyCount)
        {
            print(_WaveManager._waveIndex);
            StartCoroutine(_WaveManager.NextWave());
        }

        _CrossHair.ShootEffect();
        Score += value;
        if(Score >= 1000)
            _Score.text = Score.ToString("0.00") + "K";
        if(Score >= 1000000)
            _Score.text = Score.ToString("0.00") + "M";
        else
            _Score.text = Score.ToString("0.00");

        GameObject effect = _Pool.TakeComboEffect(pos.position);
        effect.transform.GetChild(0).GetComponent<TMP_Text>().text = "X"+ value.ToString();
    }

}
