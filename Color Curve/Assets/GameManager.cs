using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;



public class GameManager : MonoBehaviour
{
    [SerializeField] WaveManager _WaveManager;
    [SerializeField] private Animator _Animatior;

    [SerializeField] private List<GameObject> _TexTList;
    [SerializeField] ShockWaveManager _ShockWaveManager;

    public void ChangeWaveInfos()
    {
        StartCoroutine(ChangeWaveInfoIE());
    }
    IEnumerator ChangeWaveInfoIE()
    {
        //for (int i = 0; i < _TexTList.Count; i++)
        //{
        //    _TexTList[i].SetActive(false);
        //}
        _ShockWaveManager.CallShockWave();
        _Animatior.SetTrigger("WaveCompleted");
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1;

        for (int i = 0; i < _TexTList.Count; i++)
        {
            _TexTList[i].SetActive(true);
        }
    }
}