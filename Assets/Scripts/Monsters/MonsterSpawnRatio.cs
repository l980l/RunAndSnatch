using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/MonsterSpawnRatio")]
public class MonsterSpawnRatio : ScriptableObject
{
    // 난이도 0부터 4까지
    public List<List<int>> monsterRatio = new List<List<int>>();
}
