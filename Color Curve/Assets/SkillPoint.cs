using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPoint : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ToDestroy());
    }
    private IEnumerator ToDestroy()
    {
        yield return new WaitForSeconds(3);
        GetComponent<SpriteRenderer>().DOFade(0, 2).OnComplete(() => { Destroy(gameObject); });
    }
}
