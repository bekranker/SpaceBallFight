using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ScoreManager : MonoBehaviour
{
    public int Score;
    private int _bestScore;
    [SerializeField] ObjectPool _Pool;
    [SerializeField] private TMP_Text _Score;
    [SerializeField] private Transform _ScoreT;
    [SerializeField] private CrossHair _CrossHair;
    [SerializeField] private TMP_Text _BestScoreTMP, _CurrentScoreTMP;

    private bool _canEffect;

    private void Start()
    {
        _canEffect = true;
        _bestScore = PlayerPrefs.GetInt("BestScore", 0);
    }
    public void IncreaseScore(int value, Transform pos)
    {
        _CrossHair.ShootEffect();
        Score += value;
        _Score.text = Score.ToString();

        if (_canEffect)
        {
            _ScoreT.DOPunchScale(0.3f * Vector2.one, .15f).OnComplete(() => { _canEffect = true;});
            _canEffect = false;
        }

        GameObject effect = _Pool.TakeComboEffect(pos.position);
        effect.transform.GetChild(0).GetComponent<TMP_Text>().text = "X" + value.ToString();
    }
    public void SaveScore()
    {
        if(_bestScore == 0)
        {
            PlayerPrefs.SetInt("BestScore", Score);
        }
        else
        {
            if (Score > _bestScore)
            {
                PlayerPrefs.SetInt("BestScore", Score);
            }
        }
        PlayerPrefs.SetInt("CurrentScore", Score);

        _BestScoreTMP.text = "Best Score: " + _bestScore.ToString();
        _CurrentScoreTMP.text = "Score: " + Score.ToString();
    }
}
