using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    FlyingEye,
    Goblin,
    MushRoom,
    Skeleton
}

[CreateAssetMenu(menuName = "ScriptableObject/MonsterData")] 
public class MonsterData : ScriptableObject
{
    public MonsterType Type;
    public int nearTraceRange;
    public int farTraceRange;
    public int attackRange;
    public int speed;
    public int damage;
}
