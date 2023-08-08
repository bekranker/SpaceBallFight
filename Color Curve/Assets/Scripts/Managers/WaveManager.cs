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

    private WaitForSeconds _waitForWave = new WaitForSeconds(1.75f);
    public int WaveIndex;



    private void Start()
    {
        WaveIndex = 0;
        ChangeScrore();
        _RemaningTimeManager.SetRemaningTime(_WaveData[WaveIndex].WaveTimeCount);
    }
    public IEnumerator NextWave()
    {
        yield return _waitForWave;
        //ChangeScrore();
        _RemaningTimeManager.CanIncrease = true;
        _RemaningTimeManager.SetRemaningTime(_WaveData[WaveIndex].WaveTimeCount);
    }
    public void ChangeScrore() => _ScoreText.text = _SpawnerManager.KilledEnemyCount.ToString();
    public void IncreaseKilledEnemy() 
    {
        _SpawnerManager.KilledEnemyCount++;
        ChangeScrore();
    }
}
