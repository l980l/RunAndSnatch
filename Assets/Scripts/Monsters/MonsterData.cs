using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    FlyingEye,
    Goblin,
    MushRoom,
    Skeleton,
    Max
}

[CreateAssetMenu(menuName = "ScriptableObject/MonsterData")] 
public class MonsterData : ScriptableObject
{
    public MonsterType Type;
    public int nearTraceRange;
    public int farTraceRange;
    public float attackRange;
    public int speed;
    public int damage;
    public float idleTime;
    public float patrolTime;
    public float nearTraceTime;
    public float restTime;
    public float FTSCoef;
    public float NTSCoef;
}
