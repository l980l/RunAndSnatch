using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/MonsterSpawnRatio")]
public class MonsterSpawnRatio : ScriptableObject
{
    public List<int> monsterRatio = new List<int>();
}
