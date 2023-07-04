using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Wave Datas" , menuName ="Wave", order = 0)]
public class WaveDatas : ScriptableObject
{
    public int EnemyCount;
    public float SpawnDelay;
    public List<EnemyTypes> EnemyTypes = new List<EnemyTypes>();
    public List<EnemyColor> StartColor = new List<EnemyColor>();
}
